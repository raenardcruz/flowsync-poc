using Microsoft.AspNetCore.SignalR;

namespace Builder.SignalR
{
    public class LogicHub : Hub
    {
        public async Task stepProgress(string runId, string stepId, string status)
        {
            await Clients.All.SendAsync("stepProgress", runId, stepId, status);
        }

        public async Task log(LogModel log)
        {
            await Clients.All.SendAsync("log", log);
        }

        public async Task processComplete(string runId)
        {
            await Clients.All.SendAsync("processComplete", runId);
        }

        public async Task logPath(string runId, string source, string target)
        {
            await Clients.All.SendAsync("logPath", runId, source, target);
        }
    }
}