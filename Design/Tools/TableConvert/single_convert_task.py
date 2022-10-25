import xlrd


class SingleConvertTask(object):
    def __init__(self, convert_rule):
        self.convert_rule = convert_rule
        self.data = None

    def convert(self):
        pass