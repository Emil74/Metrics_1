using MetricsManager.Models.Requests;
using MetricsManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManager.Services.Client.Impl
{
    public class DotNetMetricsAgentClient : IDotNetMetricsAgentClient
    {

        #region Services

        private readonly AgentPool _agentPool;
        private readonly HttpClient _httpClient;

        #endregion

        public DotNetMetricsAgentClient(HttpClient httpClient,
            AgentPool agentPool)
        {
            _httpClient = httpClient;
            _agentPool = agentPool;
        }


        public DotNetMetricsResponse GetDotNetMetrics(DotNetMetricsRequest request)
        {
            AgentInfo agentInfo = _agentPool.Get().FirstOrDefault(agent => agent.AgentId == request.AgentId)!;
            if (agentInfo == null)
                return null!;

            string requestStr =
                $"{agentInfo.AgentAddress}api/metrics/dotnet/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestStr);
            httpRequestMessage.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = _httpClient.Send(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;
                DotNetMetricsResponse dotnetMetricsResponse =
                    (DotNetMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(DotNetMetricsResponse))!;
                dotnetMetricsResponse.AgentId = request.AgentId;
                return dotnetMetricsResponse;
            }

            return null!;
        }
    }
}
