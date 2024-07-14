using Builder.Common;
using Builder.Database;
using Builder.Main;
using Builder.SignalR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace PageBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : ControllerBase
    {

        private readonly ILogger<DevController> _logger;
        private readonly NotifyService _notifyService;
        private ConfigModel config;

        public DevController(ILogger<DevController> logger, NotifyService service, IOptions<ConfigModel> options)
        {
            _logger = logger;
            _notifyService = service;
            config = options.Value;
        }

        /// <summary>
        /// Delete Process
        /// </summary>
        /// <param name="id">Process Id</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        [Route("api/{name}")]
        public async Task<ActionResult> runAPI([FromRoute] string name, [FromQuery] string id, [FromBody] object payload)
        {
            ProcessesDB db = new ProcessesDB(config.mongoDB.connectionString, config.mongoDB.databaseName);
            var response = await db.ReadWebhook(id, name);
            if (response == null)
                return NotFound($"Webhook {name}-{id} not found");
            else
            {
                WorkflowProcessor processor = new WorkflowProcessor(Guid.NewGuid().ToString(), _notifyService);
                processor.variables["currentdata"] = JToken.Parse(payload.ToString());
                var currentData = await processor.ProcessSteps(response.components);
                return Ok(currentData.ToString().ToJsonObject());
            }
        }

        /// <summary>
        /// Quick Run Process by providing the process in json format
        /// </summary>
        /// <param name="runId">Unique guid representing each run</param>
        /// <param name="payload">JSON string containing process component</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        [Route("workflow/quickrun")]
        public async Task<ActionResult> BuilderRun([FromHeader] string runId, [FromBody] string encodedString)
        {
            _logger.Log(LogLevel.Information, "Workflow Started");
            WorkflowProcessor processor = new WorkflowProcessor(runId, _notifyService);
            Task.Run(() => processor.ProcessSteps(encodedString.Decrypt(config.secret)));
            return Ok("Background process running");
        }

        /// <summary>
        /// Add/Create a Process
        /// </summary>
        /// <param name="payload">Payload for creating new process</param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        [Route("workflow")]
        public async Task<ActionResult> BuilderAddUpdate([FromBody] string encodedString)
        {
            ProcessesDBModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcessesDBModel>(encodedString.Decrypt(config.secret));
            ProcessesDB db = new ProcessesDB(config.mongoDB.connectionString, config.mongoDB.databaseName);
            var searchResult = await db.Read(model.id);
            if (searchResult == null)
            {
                model.createdOn = DateTime.UtcNow;
                model.updatedOn = DateTime.UtcNow;
                await db.Add(model);
                return Ok("Process Created");
            }
            else
            {
                model.updatedOn = DateTime.UtcNow;
                await db.Update(model);
                return Ok("Process Updated");
            }
        }

        /// <summary>
        /// Get all list of processes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")]
        [Route("workflow/all")]
        public async Task<ActionResult> BuilderGetAll()
        {
            ProcessesDB db = new ProcessesDB(config.mongoDB.connectionString, config.mongoDB.databaseName);
            var list = await db.ReadAll();
            var response = list.Select(s =>
            {
                return new
                {
                    id = s.id,
                    title = s.name,
                    description = s.description,
                    type = s.type
                };
            });
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(response).Encrypt(config.secret));
        }

        /// <summary>
        /// Get specific process details
        /// </summary>
        /// <param name="id">Process Id</param>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")]
        [Route("workflow/{id}")]
        public async Task<ActionResult> BuilderGet([FromRoute]string id)
        {
            ProcessesDB db = new ProcessesDB(config.mongoDB.connectionString, config.mongoDB.databaseName);
            var response = await db.Read(id);
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(response).Encrypt(config.secret));
        }

        /// <summary>
        /// Delete Process
        /// </summary>
        /// <param name="id">Process Id</param>
        /// <returns></returns>
        [HttpDelete]
        [EnableCors("AllowSpecificOrigin")]
        [Route("workflow/{id}")]
        public async Task<ActionResult> BuilderDelete([FromRoute] string id)
        {
            ProcessesDB db = new ProcessesDB(config.mongoDB.connectionString, config.mongoDB.databaseName);
            await db.Delete(id);
            return Ok($"Process {id} Deleted");
        }
    }
}
