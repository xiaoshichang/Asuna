

class ModelNameConflictException(Exception):
    def __init__(self, rule1, rule2):
        self.rule1 = rule1
        self.rule2 = rule2

    def __str__(self):
        return "!!! error: rules has same model_name: %s" % self.rule1.model_name


class ConvertRuleChecker(object):

    @staticmethod
    def check_model_name_conflict(rules):
        model_names = {}
        for rule in rules:
            if rule.model_name not in model_names:
                model_names[rule.model_name] = rule
            else:
                exist_rule = model_names[rule.model_name]
                exception = ModelNameConflictException(rule, exist_rule)
                raise exception

    @staticmethod
    def check(rules):
        exceptions = []
        check_rules = \
            [
                ConvertRuleChecker.check_model_name_conflict
            ]

        for check_rule in check_rules:
            try:
                check_rule(rules)
            except Exception as e:
                exceptions.append(e)
        return exceptions
