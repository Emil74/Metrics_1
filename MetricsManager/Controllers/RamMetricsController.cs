using MetricsManager.Models.Requests;
using MetricsManager.Models;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {

        #region Services

        private IHttpClientFactory _httpClientFactory;
        private AgentPool _agentPool;
        private IRamMetricsAgentClient _metricsAgentClient;

        #endregion


        public RamMetricsController(
            IRamMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory,
            AgentPool agentPool)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _agentPool = agentPool;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<RamMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricsAgentClient.GetRamMetrics(new RamMetricsRequest
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            }));
        }


        [HttpGet("agent-old/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<RamMetricsResponse> GetMetricsFromAgentOld(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == agentId)!;
            if (agentInfo == null)
                return BadRequest();

            string requestStr =
                $"{agentInfo.AgentAddress}api/metrics/ram/from/{fromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{toTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(3000); // 3 сек

            HttpResponseMessage response = httpClient.Send(httpRequestMessage, cancellationTokenSource.Token);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                RamMetricsResponse ramMetricsResponse =
                    (RamMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(RamMetricsResponse))!;
                ramMetricsResponse.AgentId = agentId;
                return Ok(ramMetricsResponse);
            }
            return BadRequest();
        }


        [HttpGet("all/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAll(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
