using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Services
{
    public interface INetworkMetricsRepository:IRepository<NetworkMetric>
    {
        IList<NetworkMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo);
    }
}
