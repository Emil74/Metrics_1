using MetricsAgent.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob:IJob
    {

        private PerformanceCounter? _dotNetCounter;
        private IServiceScopeFactory? _serviceScopeFactory;

        public DotNetMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dotNetCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            /*
             
                         _dotNetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _dotNetCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var cpuMetricsRepository = serviceScope.ServiceProvider.GetService<IDotNetMetricsRepository>();
                try
                {
                    var cpuUsageInPercents = _dotNetCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {cpuUsageInPercents}");
                    cpuMetricsRepository.Create(new Models.DotNetMetric
                    {
                        Value = (int)cpuUsageInPercents,
                        Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    });
                }
                catch (Exception ex)
                {

                }
            }


            return Task.CompletedTask;
        }
    }
}
