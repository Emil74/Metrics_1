﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetricsManager.Models
{
    public class DotNetMetric
    {

        [JsonPropertyName("time")]
        public int Time { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}