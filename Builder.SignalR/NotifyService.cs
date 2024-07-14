using Microsoft.AspNetCore.SignalR;

namespace Builder.SignalR
{
    public class NotifyService
    {
        private readonly IHubContext<LogicHub> _hubContext;
        public NotifyService(IHubContext<LogicHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task StepProgress(string runId, string stepId, string status)
        {
            await _hubContext.Clients.All.SendAsync("stepProgress", runId, stepId, status);
        }

        public async Task Log(LogModel log)
        {
            await _hubContext.Clients.All.SendAsync("log", log);
        }

        public async Task ProcessComplete(string runId)
        {
            await _hubContext.Clients.All.SendAsync("processComplete", runId);
        }
    }
}
