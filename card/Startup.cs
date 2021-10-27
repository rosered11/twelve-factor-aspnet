using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card.dataaccess.repositories;
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
using Prometheus.HttpMetrics;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Metrics;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace card
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
            services.AddScoped<ICardRepository, MockCardRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "card", Version = "v1" });
            });

            // Prometheus
            // services.AddAllActuators(Configuration);
            // services.AddMetricsActuator(Configuration);
            // services.AddPrometheusActuator(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "card v1"));
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

            #region  Micrometer ASP.NET Core HTTP request metrics

            // Custom Metrics to count requests for each endpoint and the method
            var counter = Metrics.CreateCounter("peopleapi_path_counter", "Counts requests to the People API endpoints", new CounterConfiguration
            {
            LabelNames = new[] { "method", "endpoint" }
            });

            app.Use((context, next) =>
                {
                    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                    return next();
                }
            );

            
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Micrometer ASP.NET Core exporter middleware
                endpoints.MapAllActuators();
                // endpoints.Map<MetricsEndpoint>();
                // endpoints.Map<PrometheusEndpoint>();
                endpoints.MapMetrics();
            });
        }
    }
}
