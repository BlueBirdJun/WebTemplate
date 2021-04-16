using Autofac;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebTemplate.Common.Filters;
using WebTemplate.Common.Interfaces;

namespace WebTemplate.Root
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string MyPolicy = "policy";
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
           
            _environment = environment;
            Configuration = configuration;            
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidAudience = Configuration["Jwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
               };
           });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyPolicy,
                    builder =>
                    {
                        builder.WithOrigins("http://example.com",
                                            "http://www.contoso.com",
                                            "https://cors1.azurewebsites.net",
                                            "https://cors3.azurewebsites.net",
                                            "https://localhost:44398",
                                            "https://localhost:5001")
                               .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                               .WithMethods("PUT", "DELETE", "GET", "OPTIONS");
                    });
            });


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionLogFilter));
                options.Filters.Add(typeof(ValidateModelAttribute));
            }).AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebTemplate.Root", Version = "v1" });
            });
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddHealthChecks();
            services.AddMetrics();
            //services.AddMetricsEndpoints("metrics");
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            IBootStrap bu1 = new Business1.Api.BootStrap().SetContainerBuilder(services).RegMediator();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebTemplate.Root v1"));
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(MyPolicy);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
               
            });
        }
    }
}
