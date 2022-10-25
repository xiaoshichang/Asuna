import os
import utils


class CoderGenerator(object):

    @staticmethod
    def generate_from_rules(rules, code_export_dir):
        utils.ensure_output_dir(code_export_dir)

        csharp_dir = os.path.join(code_export_dir, "CSharp")
        CoderGenerator.generate_csharp_from_rules(rules, csharp_dir)

    @staticmethod
    def generate_csharp_from_rules(rules, code_export_dir):
        utils.ensure_output_dir(code_export_dir)
        for rule in rules:
            CoderGenerator.generate_csharp_from_rule(rule, code_export_dir)

    @staticmethod
    def generate_csharp_from_rule(rule, code_export_dir):
        pass
