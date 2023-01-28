using FsTask.ApplicationServices;
using FsTask.Domain.Contract;
using Microsoft.AspNetCore.Mvc;

namespace FsTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly IEventQueue _eventQueue;
        private readonly ISensorEventsService _sensorEventsService;

        public SensorController(IEventQueue eventQueue, ISensorEventsService sensorEventsService)
        {
            _eventQueue = eventQueue;
            _sensorEventsService = sensorEventsService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> Get(string? from = "0", string? to = "0")
        {
            return Ok(await _sensorEventsService.Get(from, to));

        }
        
        [HttpGet("events/report")]
        public async Task<IActionResult> Get(long from, long to, string filter = "human")
        {
            return Ok(await _sensorEventsService.GetReport(from, to, filter));

        }

        [HttpGet("events/at/{at}")]
        public async Task<IActionResult> Get(string at)
        {
            return Ok(await _sensorEventsService.Get(at));

        }

        [HttpPut("events/at/{at}")]
        public async Task<IActionResult> Add(string at, [FromBody] StoreSensorDataCommand command)
        {
            _eventQueue.Queue(at, command);
            return Ok(new
            {
                _code = 200,
                _links = new Links
                {
                    _self =
                        new Link
                        {
                            _url = "api/sensor?at" + at,
                            _type = "get"
                        }
                }
            });
        }
    }
}
