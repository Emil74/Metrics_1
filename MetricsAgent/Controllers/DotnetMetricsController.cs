using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MetricsAgent.Services.Impl;
using MetricsAgent.Models.Dto;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotnetMetricsController : ControllerBase
    {

        private readonly ILogger<DotnetMetricsController> _logger;
        private readonly IDotNetMetricsRepository _dotNetMetricsRepository;
        private readonly IMapper _mapper;
        public DotnetMetricsController(IDotNetMetricsRepository dotNetMetricsRepository,
            ILogger<DotnetMetricsController> logger,
            IMapper mapper)
        {
            _dotNetMetricsRepository = dotNetMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        ////[HttpPost("create")]
        ////public IActionResult Create([FromBody] DotNetMetricCreateRequest request)
        ////{
        ////    _logger.LogInformation("Create dot net metric.");
        ////    _dotNetMetricsRepository.Create(
        ////        _mapper.Map<DotNetMetric>(request));

        ////    /*_dotNetMetricsRepository.Create(new Models.DotNetMetric
        ////    {
        ////        Value = request.Value,
        ////        Time = (long)request.Time.TotalSeconds
        ////    });*/
        ////    return Ok();
        ////}

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<DotNetMetric>> GetDotNetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get dot net metrics call.");

            return Ok(_mapper.Map<List<DotNetMetricDto>>(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime)));
            //  return Ok(_dotNetMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }

        [HttpGet("all")]
        public ActionResult<IList<DotNetMetricDto>> GetDotNetMetricsAll() =>
           Ok(_mapper.Map<List<DotNetMetricDto>>(_dotNetMetricsRepository.GetAll()));
    }
}
