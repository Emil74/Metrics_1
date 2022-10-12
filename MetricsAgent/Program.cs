using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.Job;
using MetricsAgent.Jobs;
using MetricsAgent.Mappings;
using MetricsAgent.Models;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Configure Options
            builder.Services.Configure<DatabaseOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });


            #endregion

            #region Configure Mapping

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new
                MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            builder.Services.AddSingleton(mapper);

            #endregion

            #region Configure logging

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            #endregion

            #region Configure Repositories

            builder.Services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            builder.Services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            builder.Services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            builder.Services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            builder.Services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();





            //ConfigureSqlLiteConnection();
            #endregion

            #region Configure Connection

            builder.Services.AddFluentMigratorCore()
               .ConfigureRunner(rb =>
               rb.AddSQLite()
               .WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString())
               .ScanIn(typeof(Program).Assembly).For.Migrations()
               ).AddLogging(lb => lb.AddFluentMigratorConsole());

            #endregion

            #region Configure Jobs

            // Регистрация сервиса фабрики
            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
            // Регистрация базового сервиса Quartz
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // Регистрация сервиса самой задачи
            builder.Services.AddSingleton<CpuMetricJob>();
            builder.Services.AddSingleton<DotNetMetricJob>();
            builder.Services.AddSingleton<HddMetricJob>();
            builder.Services.AddSingleton<NetworkMetricJob>();
            builder.Services.AddSingleton<RamMetricJob>();

            // https://www.freeformatter.com/cron-expression-generator-quartz.html
            builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * ? * * *"));

            builder.Services.AddHostedService<QuartzHostedService>();

            #endregion

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();



            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });

            });

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }


            app.UseAuthorization();
            app.UseHttpLogging();

            app.MapControllers();

            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                var migrationRunner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                migrationRunner.MigrateUp();
            }

            app.Run();

        }
        #region Connection Старый


        //private static void ConfigureSqlLiteConnection()
        //{
        //    const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
        //    var connection = new SQLiteConnection(connectionString);
        //    connection.Open();
        //    PrepareSchema(connection);
        //}

        //private static void PrepareSchema(SQLiteConnection connection)
        //{
        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        //Задаём новый текст команды для выполнения
        //        // Удаляем таблицу с метриками, если она есть в базе данных
        //        command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
        //        // Отправляем запрос в базу данных
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE cpumetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }
        //}
        #endregion
    }
}