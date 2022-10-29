import os
import utils
import jinja2
from code_template import csharp_template


class CoderGenerator(object):

    @staticmethod
    def generate_from_rules(rules, code_export_dir):
        utils.ensure_output_dir(code_export_dir)
        client_export_dir = os.path.join(code_export_dir, "Client")
        utils.ensure_output_dir(client_export_dir)
        server_export_dir = os.path.join(code_export_dir, "Server")
        utils.ensure_output_dir(server_export_dir)
        for rule in rules:
            CoderGenerator.generate_csharp_from_rule(rule, code_export_dir)

    @staticmethod
    def generate_csharp_from_rule_fields(rule, fields, file_path):
        print("generating file: %s" % file_path)
        with open(file_path, "w") as fp:
            template = jinja2.Template(csharp_template)
            kwargs = {}
            kwargs["class_name"] = rule.model_name
            kwargs["fields"] = fields
            content = template.render(**kwargs)
            fp.write(content)

    @staticmethod
    def generate_csharp_from_rule(rule, code_export_dir):

        if len(rule.client_fields) != 0:
            client_export_dir = os.path.join(code_export_dir, "Client")
            code_file_path = os.path.join(client_export_dir, "%s.cs" % rule.model_name)
            CoderGenerator.generate_csharp_from_rule_fields(rule, rule.client_fields, code_file_path)

        if len(rule.server_fields) != 0:
            server_export_dir = os.path.join(code_export_dir, "Server")
            code_file_path = os.path.join(server_export_dir, "%s.cs" % rule.model_name)
            CoderGenerator.generate_csharp_from_rule_fields(rule, rule.server_fields, code_file_path)
