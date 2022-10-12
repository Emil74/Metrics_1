using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetricsManager.Models.Requests
{
    public class RamMetricsResponse
    {

        /// <summary>
        /// Идентификатор агента
        /// </summary>
        public int AgentId { get; set; }

        [JsonPropertyName("metrics")]
        public RamMetric[]? Metrics { get; set; }
    }
}
