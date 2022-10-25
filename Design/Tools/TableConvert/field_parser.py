from side import ConvertSide


class FieldRule(object):
    def __init__(self):
        self.field_name = None
        self.field_type = None


class ConvertRuleFieldParser(object):

    @staticmethod
    def get_side_from_field_node(node):
        side = node.attrib.get("side")
        if side is None:
            return None

        if side == "client_only":
            return ConvertSide.client_only
        elif side == "server_only":
            return ConvertSide.server_only
        elif side == "client_server":
            return ConvertSide.client_server
        else:
            raise Exception("unknown side %s" % side)

    @staticmethod
    def decide_field_side(field_side, parent_side):
        if parent_side == ConvertSide.client_server:
            return field_side
        if field_side == parent_side:
            return field_side
        return None

    @staticmethod
    def get_field_name_from_field_node(field_node):
        field_name = field_node.attrib.get("field_name")
        if field_name is None:
            raise Exception("field_name is missing.")
        return field_name

    @staticmethod
    def get_field_type_from_field_node(field_node):
        field_type = field_node.attrib.get("type")
        if field_type is None:
            raise Exception("field type is missing.")
        return field_type

    @staticmethod
    def parse_single_field(field_node, parent_side, target_side):
        if field_node.tag != "field":
            raise Exception("unknown node with tag <%s>" % field_node.tag)

        field_side = ConvertRuleFieldParser.get_side_from_field_node(field_node)
        if field_side is None:
            field_side = parent_side

        real_side = ConvertRuleFieldParser.decide_field_side(field_side, parent_side)
        if real_side is None:
            return None
        if real_side != ConvertSide.client_server and real_side != target_side:
            return None

        field = FieldRule()
        field.field_name = ConvertRuleFieldParser.get_field_name_from_field_node(field_node)
        field.field_type = ConvertRuleFieldParser.get_field_type_from_field_node(field_node)
        return field
