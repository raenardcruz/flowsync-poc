using Builder.Steps;
using Newtonsoft.Json.Linq;
using Builder.Common;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Builder.SignalR;

namespace Builder.Main
{
    public class WorkflowProcessor
    {
        private readonly NotifyService _notifyService;
        public List<LogModel> processLogs = new List<LogModel>();
        public List<JToken> stepOutputs = new List<JToken>();
        public Dictionary<string, object> variables = new Dictionary<string, object>();
        private LogicHub _hub;
        private string _runId;
        public WorkflowProcessor(string runId, NotifyService notifyService)
        {
            _runId = runId;
            _hub = new LogicHub();
            variables["currentdata"] = "";
            _notifyService = notifyService;
        }
        public async Task<object> ProcessSteps(string json)
        {
            var vueFlowObj = JToken.Parse(json);
            var workflow = SharedMethods.BuildJsonSteps(
                vueFlowObj["nodes"].ToObject<List<VueFlowNode>>(),
                vueFlowObj["edges"].ToObject<List<VueFlowEdge>>());
            var response = await ProcessSteps(workflow);
            await _notifyService.ProcessComplete(_runId);
            return response;
        }
        private async Task<Object> ProcessSteps(JToken steps)
        {
            foreach (var step in steps)
            {
                string stepId = step["id"].ToString();
                string stepType = step["step"].ToString();
                string stepLabel = step["label"].ToString();
                var logModel = new LogModel()
                { 
                    runId = _runId,
                    label = stepLabel,
                    stepId = stepId,
                    type = stepType,
                    input = variables["currentdata"].ToString(),
                    messages = new List<string>(),
                    output = ""
                };

                try
                {
                    switch (stepType)
                    {
                        case "condition":
                            var conditionStep = new ConditionStep();
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string conditionExpression = ReplaceVariables(step["expression"].ToString());
                            bool conditionMet = conditionStep.EvaluateCondition(conditionExpression);
                            var actions = conditionMet ? step["outputTrue"] : step["outputFalse"];
                            logModel.messages.Add($"({conditionExpression}) = {conditionMet.ToString()}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            logModel.output = "";
                            await ProcessSteps(actions);
                            break;

                        case "loop":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string timesStr = ReplaceVariables(step["times"].ToString());
                            int times = int.Parse(timesStr);
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            logModel.output = "";
                            for (int i = 0; i < times; i++)
                            {
                                logModel.messages.Add($"Loop {i}");
                                await ProcessSteps(step["actions"]);
                            }
                            break;

                        case "foreach":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            JArray foreachList = JArray.Parse(step["list"].ToString());
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            logModel.output = "";
                            foreach (var item in foreachList)
                            {
                                logModel.messages.Add($"Loop Item {item.ToString()}");
                                await ProcessSteps(step["actions"]);
                            }
                            break;

                        case "while":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            int counter = 0;
                            string limitStr = ReplaceVariables(step["limit"].ToString());
                            int limit = int.Parse(limitStr);
                            string originalExpression = step["expression"].ToString();
                            string whileExpression = ReplaceVariables(originalExpression);
                            var whileconditionStep = new ConditionStep();
                            bool whileConditionMet = whileconditionStep.EvaluateCondition(whileExpression);
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            logModel.output = "";
                            while (whileConditionMet)
                            {
                                if (counter >= limit)
                                    break;
                                logModel.messages.Add($"Loop {counter}. {originalExpression} = {whileConditionMet.ToString()}");
                                await ProcessSteps(step["actions"]);
                                whileExpression = ReplaceVariables(originalExpression);
                                whileConditionMet = whileconditionStep.EvaluateCondition(whileExpression);
                                counter++;
                            }
                            break;

                        case "api":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string apiVariableName = step["variable"].ToString();
                            string url = ReplaceVariables(step["url"].ToString());
                            string method = step["method"].ToString();
                            JToken? headers = JToken.Parse(
                                ReplaceVariables(step["headers"].ToString())
                                );
                            JToken? payload = step["payload"].ToString().Length > 0 ? JToken.Parse(
                                ReplaceVariables(step["payload"].ToString())
                                ) : null;
                            var response = await ApiStep.CallApi(url, method, headers, payload);
                            logModel.messages.Add($"-Method:{method.ToUpper()} -Url:{url} -Status:{response.StatusCode} -Response:{response.Response}");
                            if (apiVariableName.Length > 0)
                                variables[apiVariableName] = JToken.FromObject(response.Response);
                            logModel.output = variables["currentdata"] = JToken.FromObject(response.Response);
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "log":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string message = step["message"].ToString();
                            logModel.messages.Add($"Logging {message}");
                            Console.WriteLine(ReplaceVariables(message));
                            logModel.output = "";
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "getGuid":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string getGuidVariable = step["variable"].ToString();
                            var generatedGuid = Guid.NewGuid().ToString();
                            if (getGuidVariable.Length > 0)
                                variables[getGuidVariable] = generatedGuid;
                            logModel.output = variables["currentdata"] = generatedGuid;
                            logModel.messages.Add($"Guid Generated {generatedGuid}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "setVariable":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string name = step["name"].ToString();
                            string value = ReplaceVariables(step["value"].ToString());
                            variables[name] = value.ToJToken();
                            logModel.output = variables["currentdata"] = value.ToJToken();
                            logModel.messages.Add($"Variable Set {name}={value}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "text":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string textVariable = step["variable"].ToString();
                            string inputMessage = ReplaceVariables(step["message"].ToString());
                            variables["currentdata"] = inputMessage.ToJToken();
                            if (textVariable.Length > 0)
                                variables[textVariable] = inputMessage.ToJToken();
                            logModel.output = inputMessage;
                            logModel.messages.Add($"{inputMessage}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "math":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string mathExpression = ReplaceVariables(step["expression"].ToString());
                            string mathVariable = step["variable"].ToString();
                            string result = MathStep.EvaluateMathExpression(mathExpression);
                            if (mathVariable.Length > 0)
                                variables[mathVariable] = result;
                            variables["currentdata"] = result;
                            logModel.output = result;
                            logModel.messages.Add($"Evaluate Expression ({mathExpression}) = {result}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "list":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            JArray list = JArray.Parse(step["list"].ToString());
                            string listVariable = step["variable"].ToString();
                            if (listVariable.Length > 0)
                                variables[listVariable] = list;
                            variables["currentdata"] = list;
                            logModel.output = list;
                            logModel.messages.Add($"Created List {list.ToString()}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "count":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            JArray countList = JArray.Parse(ReplaceVariables(step["list"].ToString()));
                            string countVariable = step["variable"].ToString();
                            if (countVariable.Length > 0)
                                variables[countVariable] = countList.Count();
                            variables["currentdata"] = countList.Count();
                            logModel.output = countList.Count();
                            logModel.messages.Add($"Counting List is {variables[countVariable]}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "map":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            List<object> transformerData = new List<object>();
                            JArray mapList = JArray.Parse(ReplaceVariables(step["list"].ToString()));
                            string inputTemplate = ReplaceVariables(step["template"].ToString());
                            string mapVariable = step["variable"].ToString();
                            foreach (var item in mapList)
                            {
                                transformerData.Add(MapStep.mapTransform(item.ToString(), inputTemplate));
                            }
                            if (mapVariable.Length > 0)
                                variables[mapVariable] = JArray.Parse(JsonConvert.SerializeObject(transformerData));
                            variables["currentdata"] = JArray.Parse(JsonConvert.SerializeObject(transformerData));
                            logModel.output = transformerData;
                            logModel.messages.Add($"Transformed Data {variables[mapVariable]}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "replace":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string replaceTextValue = ReplaceVariables(step["text"].ToString());
                            string replacePatternValue = ReplaceVariables(step["pattern"].ToString());
                            string replaceReplacementValue = ReplaceVariables(step["replaceText"].ToString());
                            string replaceVariable = step["variable"].ToString();
                            if (replaceVariable.Length > 0)
                                variables[replaceVariable] = Regex.Replace(replaceTextValue, replacePatternValue, replaceReplacementValue);
                            logModel.output = variables["currentdata"] = Regex.Replace(replaceTextValue, replacePatternValue, replaceReplacementValue);
                            logModel.messages.Add($"Original: {replaceTextValue} Modified: {variables[replaceVariable]}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "regexfind":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string regexTextValue = ReplaceVariables(step["text"].ToString());
                            string regexPatternValue = ReplaceVariables(step["pattern"].ToString());
                            string regexVariable = step["variable"].ToString();
                            var tmpValue = Regex.Matches(regexTextValue, regexPatternValue, RegexOptions.IgnoreCase);
                            if (regexVariable.Length > 0)
                                variables[regexVariable] = Regex.Matches(regexTextValue, regexPatternValue, RegexOptions.IgnoreCase).ToList().Select(s => s.Value);
                            logModel.output = variables["currentdata"] = Regex.Matches(regexTextValue, regexPatternValue, RegexOptions.IgnoreCase).ToList().Select(s => s.Value);
                            logModel.messages.Add($"Found {tmpValue.Count} matches");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;

                        case "image":
                            await _notifyService.StepProgress(_runId, stepId, "running");
                            string imageValue = ReplaceVariables(step["value"].ToString());
                            variables["currentdata"] = imageValue;
                            logModel.output = imageValue;
                            logModel.messages.Add($"URL: {imageValue}");
                            await _notifyService.StepProgress(_runId, stepId, "success");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await _notifyService.StepProgress(_runId, stepId, "failed");
                    logModel.messages.Add(ex.Message);
                    logModel.dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                    await _notifyService.Log(logModel);
                    await _notifyService.ProcessComplete(_runId);
                    throw ex;
                }
                logModel.dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                await _notifyService.Log(logModel);
            }
            return variables["currentdata"];
        }

        private string ReplaceVariables(string input)
        {
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
                    input = SharedMethods.ReplaceNestedProperties(input, variableName, variableValue.ToString());
                }
            }

            return input;
        }
    }
}
