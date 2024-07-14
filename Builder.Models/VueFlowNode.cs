using Newtonsoft.Json.Linq;

public class VueFlowNode
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Label { get; set; }
    public JObject? Data { get; set; }
}
