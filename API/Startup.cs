using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NodeBlock.Engine.API.Helpers;
using NodeBlock.Engine.API.Services;
using System;

namespace NodeBlock.Engine.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();


            services.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();

                // basic logs for engine cli (debug / dev mode)
           /*   config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddEventSourceLogger();
                config.AddConsole();*/
            });

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // configure DI for application services
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IGraphService, GraphService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            /*      if (env.IsDevelopment())
                  {
                      app.UseDeveloperExceptionPage();
                  }
                  else
                  {
                      app.UseExceptionHandler("/error");
                  }*/
            app.UseDeveloperExceptionPage();

            hostApplicationLifetime.ApplicationStarted.Register(() => {
                logger.Info("Internal GraphLinq Engine API successfully started");
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
