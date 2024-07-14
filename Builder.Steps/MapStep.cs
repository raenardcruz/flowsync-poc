using Builder.Common;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Builder.Steps
{
    public class MapStep
    {
        public static object mapTransform(string objStr, string input)
        {
            if (Regex.IsMatch(objStr, "^[{|\\[]"))
            {
                var variables = JToken.Parse(objStr).ToObject<Dictionary<string, object>>();
                foreach (var variable in variables)
                {
                    string variableName = variable.Key;
                    object variableValue = variable.Value;

                    if (input.Contains($"{{{variableName}}}"))
                    {
                        input = input.Replace($"{{{variableName}}}", variableValue.ToString());
                    }

                    if (input.Contains($"{{{variableName}."))
                    {
                        input = SharedMethods.ReplaceNestedProperties(input, variableName, variableValue);
                    }
                }
                if (Regex.IsMatch(input, "^[{|\\[]"))
                    return JToken.Parse(input);
                return input;
            }
            return input.Replace($"{{stringtext}}", objStr);
        }
    }
}
