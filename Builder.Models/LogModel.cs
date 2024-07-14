public class LogModel
{
    public string runId { get; set; }
    public string stepId { get; set; }
    public string label { get; set; }
    public string type { get; set; }
    public string dateTime { get; set; }
    public object input { get; set; }
    public object output { get; set; }
    public List<string> messages { get; set; }
}