using FsTask.ApplicationServices;
using FsTask.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FsTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly IEventQueue _eventQueue;
        private readonly IStoreSensorEventService _storeSensorEventService;
        public SensorController(IEventQueue eventQueue, IStoreSensorEventService storeSensorEventService)
        {
            _eventQueue = eventQueue;
            _storeSensorEventService = storeSensorEventService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> Get(string? from ="0", string? to = "0")
        {
            return Ok(await _storeSensorEventService.Get(from , to));

        }

        [HttpGet("events/at/{at}")]
        public async Task<IActionResult> Get(string at)
        {
            return Ok(await _storeSensorEventService.Get(at));

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
