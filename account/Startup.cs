using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using account.dataaccess.repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Prometheus;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace twelve_factor_aspnet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setting HttpFactory for call another services
            services
                .AddHttpClient("card", 
                    client => client.BaseAddress = new Uri(Configuration["cards:url"])).AddServiceDiscovery()
                // Zipkin for client call another services.
                .AddHttpMessageHandler(provider =>
                    TracingHandler.WithoutInnerHandler(Configuration["Eureka:Instance:AppName"]));
            
            services.AddScoped<IAccountRepository, MockAccountRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "twelve_factor_aspnet", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "twelve_factor_aspnet v1"));
            }

            #region Zipkin
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender(Configuration["zipkin:url"], "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer());
                TraceManager.RegisterTracer(tracer);
                TraceManager.Start(logger);
            });
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing(Configuration["Eureka:Instance:AppName"]);
            #endregion

            //app.UseHttpsRedirection();

            app.UseRouting();
            
            // Must to setup under app.UseRouting()
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Micrometer ASP.NET Core exporter middleware
                endpoints.MapMetrics();
            });
        }
    }
}
