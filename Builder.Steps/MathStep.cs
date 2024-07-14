using NCalc;

namespace Builder.Steps
{
    public class MathStep
    {
        public static string EvaluateMathExpression(string expression)
        {
            Expression expr = new Expression(expression, EvaluateOptions.IgnoreCase);
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

            var result = expr.Evaluate();
            return result.ToString();
        }

        private static void HandleFunctions(string name, FunctionArgs args)
        {
            if (name == "==" && args.Parameters.Length == 2)
            {
                // Implementing a custom equality check
                object param1 = args.Parameters[0].Evaluate();
                object param2 = args.Parameters[1].Evaluate();
                if (param1 is string && param2 is string)
                {
                    // Compare strings
                    args.Result = (string)param1 == (string)param2;
                }
                else
                {
                    // Default to standard equality
                    args.Result = param1.Equals(param2);
                }
            }
        }
    }
}
