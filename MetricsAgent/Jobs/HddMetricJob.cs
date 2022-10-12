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
    public class HddMetricJob
    {

        private PerformanceCounter? _hddCounter;
        private IServiceScopeFactory? _serviceScopeFactory;

        public HddMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hddCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            /*
             
                         _hddCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _hddCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var cpuMetricsRepository = serviceScope.ServiceProvider.GetService<IHddMetricsRepository>();
                try
                {
                    var cpuUsageInPercents = _hddCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {cpuUsageInPercents}");
                    cpuMetricsRepository.Create(new Models.HddMetric
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
