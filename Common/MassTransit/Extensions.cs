using MassTransit;
using MassTransit.Definition; // Add this using statement
using System;
using Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {

            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumers(Assembly.GetEntryAssembly());

                configurator.UsingRabbitMq((context, configurator) =>
                {
                    var Configuration = context.GetService<IConfiguration>();
                    var serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMQSettings = Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }
}
