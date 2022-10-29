import xlrd


class SingleConvertTask(object):
    def __init__(self, convert_rule):
        self.convert_rule = convert_rule
        self.data = None

    def convert(self):
        print("start convert with rule(%s, %s)" % (self.convert_rule.source_excel_path, self.convert_rule.model_name))
        pass