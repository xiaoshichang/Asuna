import os.path
import xml.etree.ElementTree as ET
from side import ConvertSide
from field_parser import ConvertRuleFieldParser


class SingleRule(object):
    def __init__(self):
        self.source_excel_path = None
        self.model_name = None
        self.side = None
        self.desc = None
        self.client_fields = None
        self.server_fields = None

    def need_export(self):
        return len(self.client_fields) > 0 or len(self.server_fields) > 0


class ConvertRuleParser(object):

    @staticmethod
    def parse(excel_dir_path):
        rules = []
        rule_file_path = os.path.join(excel_dir_path, "rules.xml")
        root = ET.parse(rule_file_path).getroot()
        if root.tag != "rules":
            raise Exception("root node unknown")

        for child in root:
            rule = ConvertRuleParser.parse_single_rule_node(excel_dir_path, child)

            if not rule.need_export():
                print("skip rule(%s), no export fields" % rule.model_name)
                continue

            rules.append(rule)
        return rules

    @staticmethod
    def get_source_file_path(excel_dir_path, node):
        relative_path = node.attrib.get("source_file")
        if not relative_path:
            raise Exception("source_file attribute is missing.")

        excel_full_path = os.path.join(excel_dir_path, relative_path)
        return excel_full_path

    @staticmethod
    def get_side_from_rule_node(rule_node):
        side = rule_node.attrib.get("side")
        if not side:
            raise Exception("side is missing.")

        if side == "client_only":
            return ConvertSide.client_only
        elif side == "server_only":
            return ConvertSide.server_only
        elif side == "client_server":
            return ConvertSide.client_server
        else:
            raise Exception("unknown side %s" % side)

    @staticmethod
    def get_rule_name_from_rule_node(rule_node):
        rule_name = rule_node.attrib.get("model_name")
        if not rule_name:
            raise Exception("rule_name is missing")
        return rule_name

    @staticmethod
    def get_rule_desc_from_rule_node(rule_node):
        desc = rule_node.attrib.get("desc")
        return desc

    @staticmethod
    def parse_fields(rule_node, parent_side, target_side):
        fields = []
        for field_node in rule_node:
            field = ConvertRuleFieldParser.parse_single_field(field_node, parent_side, target_side)
            if field:
                fields.append(field)
        return fields

    @staticmethod
    def parse_single_rule_node(excel_dir_path, node):
        if node.tag != "rule":
            raise Exception("unknown node with tag <%s>" % node.tag)

        rule = SingleRule()
        rule.source_excel_path = ConvertRuleParser.get_source_file_path(excel_dir_path, node)
        rule.model_name = ConvertRuleParser.get_rule_name_from_rule_node(node)
        rule.side = ConvertRuleParser.get_side_from_rule_node(node)
        rule.desc = ConvertRuleParser.get_rule_desc_from_rule_node(node)
        rule.client_fields = ConvertRuleParser.parse_fields(node, rule.side, ConvertSide.client_only)
        rule.server_fields = ConvertRuleParser.parse_fields(node, rule.side, ConvertSide.server_only)

        return rule

