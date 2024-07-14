using Newtonsoft.Json.Linq;

public class ProcessesDBModel
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string type { get; set; }
    public string intervalType { get; set; }
    public string intervalStartTime { get; set; }
    public string intervalEndTime { get; set; }
    public string intervalStartDate { get; set; }
    public int interval { get; set; }
    public bool[] intervalDaysofWeek { get; set; }
    public string webhookName { get; set; }
    public string eventName { get; set; }
    public string components { get; set; }
    public DateTime createdOn { get; set; }
    public string createdBy { get; set; }
    public DateTime updatedOn { get; set; }
    public string updatedBy { get; set; }
}