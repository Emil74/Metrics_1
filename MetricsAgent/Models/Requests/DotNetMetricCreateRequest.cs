using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Models.Requests
{
    public class DotNetMetricCreateRequest
    {
        public int Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
