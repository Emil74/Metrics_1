using MetricsAgent.Models.Requests;
using MetricsAgent.Models;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MetricsAgent.Services.Impl;
using MetricsAgent.Models.Dto;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {

        private readonly ILogger<HddMetricsController> _logger;
        private readonly IHddMetricsRepository _hddMetricsRepository;
        private readonly IMapper _mapper;
        public HddMetricsController(IHddMetricsRepository hddMetricsRepository,
            ILogger<HddMetricsController> logger,
            IMapper mapper)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] HddMetricCreateRequest request)
        //{
        //    _logger.LogInformation("Create hdd metric.");

        //    _hddMetricsRepository.Create(
        //        _mapper.Map<HddMetric>(request));

        //    /*
        //    _hddMetricsRepository.Create(new Models.HddMetric
        //    {
        //        Value = request.Value,
        //        Time = (long)request.Time.TotalSeconds
        //    });*/
        //    return Ok();
        //}

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<HddMetric>> GetHddMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get hdd metrics call.");
            return Ok(_mapper.Map<List<HddMetricDto>>(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime)));
            
            //return Ok(_hddMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }

        [HttpGet("all")]
        public ActionResult<IList<HddMetricDto>> GetHddMetricsAll() =>
           Ok(_mapper.Map<List<HddMetricDto>>(_hddMetricsRepository.GetAll()));
    }
}
