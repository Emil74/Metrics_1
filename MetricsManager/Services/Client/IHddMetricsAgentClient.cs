using MetricsManager.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManager.Services.Client
{
    public interface IHddMetricsAgentClient
    {
        HddMetricsResponse GetHddMetrics(HddMetricsRequest request);
    }
}
