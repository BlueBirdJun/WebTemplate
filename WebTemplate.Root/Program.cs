using App.Metrics;
using App.Metrics.Reporting.InfluxDB;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTemplate.Root
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string appjson = "appsettings.json";
            string conjson = "values.json";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower() == "development")
            {
                appjson = $"appsettings.{environmentName}.json";
                conjson = $"values.{environmentName}.json";
            }  

            var configuration = new ConfigurationBuilder()
                    .AddJsonFile(appjson)
                    .AddJsonFile(conjson)
                    .Build();
             Serilog.ILogger logger = Log.Logger = new LoggerConfiguration()            
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            try
            {
                Log.Warning("Start API");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception.Message);
                throw;
            }
            finally
            {    
                Log.CloseAndFlush();
            }
        }
       
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    MetricsReportingInfluxDbOptions influxdbOptions = new MetricsReportingInfluxDbOptions
        //    {
        //        InfluxDb = {
        //            // influxdb http �ּ�
        //            BaseUri = new Uri("http://localhost:8086"),
        //            // database �̸�
        //            Database = "apiinfo", 
        //            // database ������� �ڵ� ����
        //            CreateDataBaseIfNotExists = true,
        //            // �������� ���� ����
        //            UserName = "admin",
        //            Password = "admin",
        //        },
        //        // ���� �ֱ�. 1�ʸ��� ���۽õ�
        //        FlushInterval = TimeSpan.FromSeconds(1),
        //        HttpPolicy = {
        //            // FailuresBeforeBackoff �� �õ� �� ���н� BackoffPeriod �ð���ŭ ���
        //            // 5�� ���н� 10 ���� 5�� ��õ� �ݺ�. (0�� ��� Report �̵���)
        //            FailuresBeforeBackoff = 5,
        //            BackoffPeriod = TimeSpan.FromSeconds(10),
        //            // ����� timeout ��
        //            Timeout = TimeSpan.FromSeconds(10)
        //        }
        //    };
        //    // health ������ influxdb�� �ֱ� ���� ���� ���� ����
        //    IMetricsRoot metrics = AppMetrics.CreateDefaultBuilder()
        //        .Report.ToInfluxDb(influxdbOptions)
        //        .Build();

        //    return WebHost.CreateDefaultBuilder(args)
        //        .ConfigureMetrics(metrics)
        //        .ConfigureHealthWithDefaults(
        //            options =>
        //            {
        //                options.HealthChecks.AddCheck("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));
        //                // health ������ metrics �� �����ϵ��� ���
        //                object p = options.Report.ToMetrics(metrics);
        //            }
        //        ).ConfigureMetricsWithDefaults(
        //            builder =>
        //            {
        //                builder.Configuration.Configure(
        //                    options =>
        //                    {
        //                        options.Enabled = true;
        //                        options.ReportingEnabled = true;
        //                    }
        //                );
        //            }
        //        )
        //        .UseMetricsWebTracking(
        //            options =>
        //            {
        //                options.ApdexTrackingEnabled = true;
        //                options.ApdexTSeconds = 0.5;
        //                options.IgnoredHttpStatusCodes = new List<int> { 404 };
        //                options.OAuth2TrackingEnabled = true;
        //            }
        //        )
        //        .UseHealth()    // Enable Report Health
        //        .UseMetricsEndpoints()  // Enable Endpoint : /metrics, /metrics-text, /env
        //        .UseHealthEndpoints()   // Enable Health Check Endpoint : /health, /ping
        //        .UseMetrics()   // Enable Metrics
        //        .UseStartup<Startup>()
        //        .ConfigureServices(services => services.AddAutofac())
        //        //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //        .UseSerilog();
        //}

        public static Microsoft.Extensions.Hosting.IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            /*.ConfigureMetricsWithDefaults(builder =>
               {
                   builder.Report.ToTextFile("c:\\logs", TimeSpan.FromSeconds(2));
               })
               .ConfigureAppMetricsHostingConfiguration(options =>
               {
                   // options.AllEndpointsPort = 3333;
                   options.EnvironmentInfoEndpoint = "/my-env";
                   options.EnvironmentInfoEndpointPort = 1111;
                   options.MetricsEndpoint = "/my-metrics";
                   options.MetricsEndpointPort = 2222;
                   options.MetricsTextEndpoint = "/my-metrics-text";
                   options.MetricsTextEndpointPort = 3333;
               })*/            
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               }).UseSerilog();//.UseMetrics();
              //.UseHealth()    // Enable Report Health
              //.UseMetricsEndpoints()  // Enable Endpoint : /metrics, /metrics-text, /env
              //.UseHealthEndpoints()   // Enable Health Check Endpoint : /health, /ping
              //.UseMetrics();   // Enable Metrics


    }
}
