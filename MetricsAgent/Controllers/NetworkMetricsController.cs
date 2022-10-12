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
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {

        private readonly ILogger<NetworkMetricsController> _logger;
        private readonly INetworkMetricsRepository _networkMetricsRepository;
        private readonly IMapper _mapper;
        public NetworkMetricsController(INetworkMetricsRepository networkMetricsRepository,
            ILogger<NetworkMetricsController> logger,
            IMapper mapper)
        {
            _networkMetricsRepository = networkMetricsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        //{
        //    _logger.LogInformation("Create network metric.");
        //    _networkMetricsRepository.Create(
        //        _mapper.Map<NetworkMetric>(request));
        //    /*
        //    _networkMetricsRepository.Create(new Models.NetworkMetric
        //    {
        //        Value = request.Value,
        //        Time = (long)request.Time.TotalSeconds
        //    });*/
        //    return Ok();
        //}

        [HttpGet("from/{fromTime}/to/{toTime}")]
        //[Route("from/{fromTime}/to/{toTime}")]
        public ActionResult<IList<NetworkMetric>> GetNetworkMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Get network metrics call.");
            return Ok(_mapper.Map<List<NetworkMetricDto>>(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime)));

            //return Ok(_networkMetricsRepository.GetByTimePeriod(fromTime, toTime));
        }

        [HttpGet("all")]
        public ActionResult<IList<NetworkMetricDto>> GetNetworkMetricsAll() =>
           Ok(_mapper.Map<List<NetworkMetricDto>>(_networkMetricsRepository.GetAll()));
    }
}
