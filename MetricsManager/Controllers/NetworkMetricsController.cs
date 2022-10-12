using MetricsManager.Models.Requests;
using MetricsManager.Models;
using MetricsManager.Services.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MetricsManager.Controllers
{
    [Route("api/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {

        #region Services

        private IHttpClientFactory _httpClientFactory;
        private AgentPool _agentPool;
        private INetworkMetricsAgentClient _metricsAgentClient;

        #endregion


        public NetworkMetricsController(
            INetworkMetricsAgentClient metricsAgentClient,
            IHttpClientFactory httpClientFactory,
            AgentPool agentPool)
        {
            _httpClientFactory = httpClientFactory;
            _metricsAgentClient = metricsAgentClient;
            _agentPool = agentPool;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<NetworkMetricsResponse> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_metricsAgentClient.GetNetworkMetrics(new NetworkMetricsRequest
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            }));
        }


        [HttpGet("agent-old/{agentId}/from/{fromTime}/to/{toTime}")]
        public ActionResult<NetworkMetricsResponse> GetMetricsFromAgentOld(
            [FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == agentId)!;
            if (agentInfo == null)
                return BadRequest();

            string requestStr =
                $"{agentInfo.AgentAddress}api/metrics/network/from/{fromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{toTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpClient httpClient = _httpClientFactory.CreateClient();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(3000); // 3 сек

            HttpResponseMessage response = httpClient.Send(httpRequestMessage, cancellationTokenSource.Token);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                NetworkMetricsResponse networkMetricsResponse =
                    (NetworkMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(NetworkMetricsResponse))!;
                networkMetricsResponse.AgentId = agentId;
                return Ok(networkMetricsResponse);
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
