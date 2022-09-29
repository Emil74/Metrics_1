using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotnetMetricsController : ControllerBase
    {

        private readonly ILogger<DotnetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;

        public DotnetMetricsController(IDotNetMetricsRepository dotNetMetricsRepository,
            ILogger<DotnetMetricsController> logger)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        {
            _logger.LogInformation("Create dot net metric.");
            _dotNetMetricsRepository.Create(new Models.DotNetMetric
            {
                Value = request.Value,
                Time = (long)request.Time.TotalSeconds
            });
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotNetMetric>> GetDotNetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get dot net metrics call.");
            return Ok(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }

    }
}
