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
    public class CpuMetricJob : IJob
    {
        private PerformanceCounter? _cpuCounter;
        private IServiceScopeFactory? _serviceScopeFactory;

        public CpuMetricJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            /*
             
                         _cpuCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
            _cpuCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
             
             */
        }


        public Task Execute(IJobExecutionContext context)
        {

            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                var cpuMetricsRepository = serviceScope.ServiceProvider.GetService<ICpuMetricsRepository>();
                try
                {
                    var cpuUsageInPercents = _cpuCounter.NextValue();
                    var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    Debug.WriteLine($"{time} > {cpuUsageInPercents}");
                    cpuMetricsRepository.Create(new Models.CpuMetric
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
