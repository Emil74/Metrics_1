using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {

        private readonly ILogger<CpuMetricsController> _logger;
        private readonly ICpuMetricsRepository _cpuMetricsRepository;
        private readonly IMapper _mapper;

        public CpuMetricsController(ICpuMetricsRepository cpuMetricsRepository,
            ILogger<CpuMetricsController> logger,
            IMapper mapper)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            _logger.LogInformation("Create cpu metric.");
            _cpuMetricsRepository.Create(
                _mapper.Map<CpuMetric>(request));

            //    new Models.CpuMetric
            //{
            //    Value = request.Value,
            //    Time = (long)request.Time.TotalSeconds
            //});
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<CpuMetricDto>> GetCpuMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get cpu metrics call.");

            return Ok(_mapper.Map<List<CpuMetricDto>>(_cpuMetricsRepository.GetByTimePeriod(fromTime, toTime)));
            
            

            //return _cpuMetricsRepository.GetByTimePeriod(fromTime, toTime).Select(element => new CpuMetricDto
            //{
            //    Value = element.Value,
            //    Time = element.Time
            //}).ToList();

        }
        [HttpGet("all")]
        public ActionResult<IList<CpuMetricDto>> GetCpuMetricsAll() =>
            Ok(_mapper.Map<List<CpuMetricDto>>(_cpuMetricsRepository.GetAll()));

       

    }
}
