using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Builder.Common
{
    public class SharedMethods
    {
        #region BuildJsonSteps
        public static JArray BuildJsonSteps(List<VueFlowNode> nodesObj, List<VueFlowEdge> edgesObj, string source = "1", JArray transformedObj = null, string sourceHandle = "output")
        {
            if (transformedObj == null)
                transformedObj = new JArray();

            var targetResult = edgesObj.Where(f => f.Source == source && f.SourceHandle == sourceHandle).ToList();
            if (!targetResult.Any())
                return transformedObj;

            foreach (var targetObj in targetResult)
            {
                var target = targetObj.Target;
                var filteredNodes = nodesObj.Where(f => f.Id == target).ToList();

                foreach (var node in filteredNodes)
                {
                    JObject newObj = new JObject
                    {
                        ["id"] = node.Id,
                        ["step"] = node.Type,
                        ["label"] = node.Label
                    };

                    switch (node.Type)
                    {
                        case "setVariable":
                            newObj["name"] = node.Data["name"];
                            newObj["value"] = node.Data["value"];
                            break;

                        case "condition":
                            newObj["expression"] = node.Data["expression"];
                            newObj["outputTrue"] = BuildJsonSteps(nodesObj, edgesObj, target, new JArray(), "outputTrue");
                            newObj["outputFalse"] = BuildJsonSteps(nodesObj, edgesObj, target, new JArray(), "outputFalse");
                            break;

                        case "loop":
                            newObj["times"] = node.Data["times"];
                            newObj["output"] = BuildJsonSteps(nodesObj, edgesObj, target, new JArray());
                            break;

                        case "foreach":
                            newObj["list"] = node.Data["list"];
                            newObj["output"] = BuildJsonSteps(nodesObj, edgesObj, target, new JArray());
                            break;

                        case "while":
                            newObj["limit"] = node.Data["limit"];
                            newObj["expression"] = node.Data["expression"];
                            newObj["output"] = BuildJsonSteps(nodesObj, edgesObj, target, new JArray());
                            break;

                        case "api":
                            newObj["url"] = node.Data["url"];
                            newObj["method"] = node.Data["method"];
                            newObj["headers"] = BuildHeaderJson(node.Data["headers"]);
                            newObj["payload"] = BuildPayload(node.Data["payload"], node.Data["headers"][0]["value"].ToString());
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "log":
                            newObj["message"] = node.Data["message"];
                            break;

                        case "getGuid":
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "text":
                            newObj["message"] = node.Data["message"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "math":
                            newObj["expression"] = node.Data["expression"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "list":
                            newObj["list"] = BuildListObject(node.Data["list"], node.Data["type"].ToString());
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "count":
                            newObj["list"] = node.Data["list"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "map":
                            newObj["list"] = node.Data["list"];
                            newObj["template"] = node.Data["template"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "replace":
                            newObj["text"] = node.Data["text"];
                            newObj["pattern"] = node.Data["pattern"];
                            newObj["replaceText"] = node.Data["replaceText"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "regexfind":
                            newObj["text"] = node.Data["text"];
                            newObj["pattern"] = node.Data["pattern"];
                            newObj["variable"] = node.Data["variable"];
                            break;

                        case "image":
                            newObj["value"] = node.Data["value"];
                            break;

                        case "subprocess":
                            newObj["value"] = node.Data["value"];
                            break;
                    }

                    transformedObj.Add(newObj);

                    BuildJsonSteps(nodesObj, edgesObj, target, transformedObj);
                }
            }

            return transformedObj;
        }

        private static JObject BuildHeaderJson(JToken datas)
        {
            var obj = new JObject();

            foreach (var data in datas)
            {
                obj["Name"] = data["key"];
                obj["Value"] = data["value"];
            }

            return obj;
        }

        private static JToken BuildPayload(JToken payload, string contentType)
        {
            if (contentType == "application/x-www-form-urlencoded" || contentType == "multipart/form-data")
            {
                var objData = new JObject();

                foreach (var data in payload)
                {
                    objData[(string)data["key"]] = data["value"];
                }

                return objData;
            }

            return payload;
        }

        private static JArray BuildListObject(JToken list, string type)
        {
            var tmpList = new JArray();
            if (type == "object")
            {
                foreach (var item in list)
                {
                    tmpList.Add(JObject.Parse((string)item["value"]));
                }
                return tmpList;
            }
            foreach (var item in list)
            {
                tmpList.Add(item["value"]);
            }
            return tmpList;
        }
        #endregion

        #region ReplaceNestedProperties
        public static string ReplaceNestedProperties(string input, string variableName, object variableValue)
        {
            string pattern = $"{{{variableName}.";
            int startIndex = input.IndexOf(pattern);
            while (startIndex != -1)
            {
                startIndex += pattern.Length;
                int endIndex = input.IndexOf("}", startIndex);
                if (endIndex > startIndex)
                {
                    string nestedProperty = input.Substring(startIndex, endIndex - startIndex);
                    string fullPattern = $"{{{variableName}.{nestedProperty}}}";

                    var nestedValue = GetNestedPropertyValue(variableValue, nestedProperty);
                    if (nestedValue != null)
                    {
                        input = input.Replace(fullPattern, nestedValue.ToString());
                    }
                }

                startIndex = input.IndexOf(pattern);
            }

            return input;
        }

        private static object GetNestedPropertyValue(object obj, string propertyPath)
        {
            try
            {
                if (obj == null || string.IsNullOrEmpty(propertyPath))
                    return null;

                List<string> prop = propertyPath.Split(".").ToList();
                foreach (string propPath in prop)
                {
                    var objStr = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj.ToString());
                    obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj.ToString())[propPath];
                }

                return obj;
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
