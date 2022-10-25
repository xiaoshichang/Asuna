import os
import argparse
from converter import TableConverter
from rule_parser import ConvertRuleParser
from code_generator import CoderGenerator
import utils

parser = argparse.ArgumentParser(description="a tool to convert excel table to txt format data")
parser.add_argument("--ExcelDir", required=True, help="directory of excel tables")
parser.add_argument("--DataExportDir", required=True,  help="directory to export data")
parser.add_argument("--CodeExportDir", required=True, help="directory to export generated code")
args = parser.parse_args()


def check_excel_dir_exist():
    if not os.path.exists(args.ExcelDir):
        print("ExcelDir(%s) not exist!" % args.ExcelDir)
        exit(1)


def parse_rules():
    print("parse_rules start")
    rules = []
    try:
        rules = ConvertRuleParser.parse(args.ExcelDir)
    except Exception as e:
        print(e)
        exit(1)
    print("parse_rules finish, %d rules found." % len(rules))
    return rules


def generate_code(rules):
    CoderGenerator.generate_from_rules(rules, args.CodeExportDir)


def do_convert_tasks(rules):
    tasks, errors = TableConverter.convert(rules)
    if len(errors) != 0:
        TableConverter.print_all_errors(errors)
        exit(1)
    return tasks


def post_check(tasks):
    """
    导表规则验证
    """
    print("post_check start")
    print("post_check finish")


def export_data(tasks):
    print("export_data start")
    utils.ensure_output_dir(args.DataExportDir)
    print("export_data finish")


def main():
    check_excel_dir_exist()
    rules = parse_rules()
    generate_code(rules)
    tasks = do_convert_tasks(rules)
    post_check(tasks)
    export_data(tasks)
    return 0


if __name__ == "__main__":
    main()
