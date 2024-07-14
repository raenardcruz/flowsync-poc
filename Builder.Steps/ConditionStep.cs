using NCalc;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Builder.Steps
{
    public class ConditionStep
    {
        private Dictionary<string, object> _params;
        public ConditionStep()
        {
            _params = new Dictionary<string, object>();
        }
        public bool EvaluateCondition(string expression)
        {
            expression = replaceOperators(expression);
            expression = findParams(expression);
            Expression expr = new Expression(replaceOperators(expression), EvaluateOptions.IgnoreCase);
            expr.EvaluateFunction += HandleFunctions;
            expr.EvaluateParameter += (name, args) =>
            {
                if (name.StartsWith("'") && name.EndsWith("'"))
                {
                    args.Result = name.Trim('\'');
                }
                else if (name.StartsWith("\"") && name.EndsWith("\""))
                {
                    args.Result = name.Trim('"');
                }
                else
                {
                    args.Result = name;
                }
            };

            bool result = (bool)expr.Evaluate();
            return result;
        }
        private void HandleFunctions(string name, FunctionArgs args)
        {
            if (name == "isempty")
            {
                if (args.Parameters.Length == 0)
                    args.Result = true;
                else if (args.Parameters.Length == 1)
                    args.Result = false;
                else
                    throw new Exception("IsEmpty canopnly have one parameter");
            }
        }

        private string replaceOperators(string expression)
        {
            return expression.ToUpper()
                .Replace(" OR ", " || ")
                .Replace(" AND ", " && ")
                .Replace(" NOT  ", " !");
        }

        private string findParams(string expression)
        {
            int count = 0;
            expression = expression.Replace("\n", "");
            var matches = Regex.Matches(expression, "\\{(.*?)\\}", RegexOptions.IgnorePatternWhitespace);
            foreach (Match match in matches)
            {
                var param = $"json_{count}";
                var input = match.Value;
                expression = expression.Replace($"{input}", $"{param}");
                _params.Add(param, input);
                count++;
            }
            return expression;
        }
    }
}
