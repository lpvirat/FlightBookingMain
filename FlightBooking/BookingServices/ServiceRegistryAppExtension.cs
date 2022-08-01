using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace ServiceDiscovery.Config
{
    public static class ServiceRegistryAppExtension
    {
        public static IServiceCollection AddConsulConfiguration(this IServiceCollection services)
        {
            string ConsulAddress = "http://localhost:8500/";
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(ConsulAddress);
            }));
            return services;
        }
        public static IApplicationBuilder UseConsulconfig(this IApplicationBuilder app)
        {
            var consulclient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var registration = new AgentServiceRegistration()
            {
                ID = "BookingID1",
                Name= "BookingServices",
                Address="localhost",
                Port= 8013
            };
            logger.LogInformation("Registering with Consul");
            consulclient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulclient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering");
                consulclient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;

        }
    }
}
