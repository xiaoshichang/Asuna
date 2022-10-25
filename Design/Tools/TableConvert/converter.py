from single_convert_task import SingleConvertTask


class ConvertError(Exception):
    def __init__(self, table_path, reason):
        self.table_path = table_path
        self.reason = reason


class TableConverter(object):

    @staticmethod
    def print_all_errors(errors):
        print("%d errors during this convert operation" % len(errors))
        for error in errors:
            print("************* convert table error ******************")
            print("table path: %s" % error.table_path)
            print("reason: %s" % error.reason)
            print("****************************************************")

    @staticmethod
    def convert(all_convert_rules):
        errors = []
        tasks = []
        for rule in all_convert_rules:
            try:
                task = SingleConvertTask(rule)
                task.convert()
                tasks.append(task)
            except Exception as e:
                errors.append(e)
        return tasks, errors
