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
    [Route("api/[controller]")]
    [ApiController]
    public class RamMerticsController : ControllerBase
    {

        private readonly ILogger<RamMerticsController> _logger;
        private readonly IRamMetricsRepository _ramMetricsRepository;
        private readonly IMapper _mapper;
        public RamMerticsController(IRamMetricsRepository ramMetricsRepository,
            ILogger<RamMerticsController> logger,
            IMapper mapper)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _logger.LogInformation("Create ram metric.");

            _ramMetricsRepository.Create(
                _mapper.Map<RamMertic>(request));
            /*
            _ramMetricsRepository.Create(new Models.RamMertic
            {
                Value = request.Value,
                Time = (long)request.Time.TotalSeconds
            });*/
            return Ok();
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<RamMertic>> GetRamMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get ram metrics call.");
            return Ok(_mapper.Map<List<RamMetricDto>>(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime)));

           // return Ok(_ramMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }

        [HttpGet("all")]
        public ActionResult<IList<RamMetricDto>> GetRamMetricsAll() =>
           Ok(_mapper.Map<List<RamMetricDto>>(_ramMetricsRepository.GetAll()));
    }
}
