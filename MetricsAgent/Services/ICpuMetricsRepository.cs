using MetricsAgent.Models;

namespace MetricsAgent.Services
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
        IList<CpuMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo);
    }
}
