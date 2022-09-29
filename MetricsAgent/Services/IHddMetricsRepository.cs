using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Services
{
    public interface IHddMetricsRepository:IRepository<HddMetric>
    {
        IList<HddMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo);
    }
}
