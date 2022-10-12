using MetricsManager.Models.Requests;

namespace MetricsManager.Services.Client
{
    public interface ICpuMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest request);
    }
}
