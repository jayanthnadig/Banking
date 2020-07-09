using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using ASNRTech.CoreService.Core;

namespace ASNRTech.CoreService.Config {
    public class Startup {
        private const string API_VERSION = "v1.0";

        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
               .AddMvc(o => o.MaxModelValidationErrors = 20)
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
               .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c => c.SwaggerDoc(API_VERSION, new Info { Title = $"ASNRTech CORE API ({Utilities.Utility.Environment}) {API_VERSION}", Version = API_VERSION }));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Modified due to widget edit on 1st july 2020

            //services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors();

            services.AddSignalR();

            //services.AddDistributedRedisCache(option => option.Configuration = Utilities.Utility.GetConfigValue("redis"));

            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IServiceProvider container) {
            if (!Utilities.Utility.IsProduction) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            //logging
            app.UseMiddleware<CorrelationMiddleWare>();
            app.UseMiddleware<ApiLogMiddleWare>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();

            //api doc
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{API_VERSION}/swagger.json", $"ASNRTech CORE API {API_VERSION}"));

            app.UseMvc();

            app.UseFileServer();

            SchedulerService scheduler = new SchedulerService(container);
            lifetime.ApplicationStarted.Register(() => scheduler.StartAsync());
            lifetime.ApplicationStopping.Register(scheduler.Stop);

            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug, false, true);
        }
    }
}
