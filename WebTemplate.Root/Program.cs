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
        //            // influxdb http 주소
        //            BaseUri = new Uri("http://localhost:8086"),
        //            // database 이름
        //            Database = "apiinfo", 
        //            // database 미존재시 자동 생성
        //            CreateDataBaseIfNotExists = true,
        //            // 인증사용시 계정 정보
        //            UserName = "admin",
        //            Password = "admin",
        //        },
        //        // 저장 주기. 1초마다 전송시도
        //        FlushInterval = TimeSpan.FromSeconds(1),
        //        HttpPolicy = {
        //            // FailuresBeforeBackoff 번 시도 후 실패시 BackoffPeriod 시간만큼 대기
        //            // 5번 실패시 10 초후 5번 재시도 반복. (0인 경우 Report 미동작)
        //            FailuresBeforeBackoff = 5,
        //            BackoffPeriod = TimeSpan.FromSeconds(10),
        //            // 저장시 timeout 값
        //            Timeout = TimeSpan.FromSeconds(10)
        //        }
        //    };
        //    // health 정보를 influxdb에 넣기 위해 별도 변수 정의
        //    IMetricsRoot metrics = AppMetrics.CreateDefaultBuilder()
        //        .Report.ToInfluxDb(influxdbOptions)
        //        .Build();

        //    return WebHost.CreateDefaultBuilder(args)
        //        .ConfigureMetrics(metrics)
        //        .ConfigureHealthWithDefaults(
        //            options =>
        //            {
        //                options.HealthChecks.AddCheck("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));
        //                // health 정보를 metrics 에 보고하도록 등록
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
