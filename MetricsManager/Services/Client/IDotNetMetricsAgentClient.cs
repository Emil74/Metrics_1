﻿using MetricsManager.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManager.Services.Client
{
    public interface IDotNetMetricsAgentClient
    {
        DotNetMetricsResponse GetDotNetMetrics(DotNetMetricsRequest request);
    }
}
