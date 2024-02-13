using MassTransit;
using MassTransit.Definition; // Add this using statement
using System;
using Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using GreenPipes;


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
                    
                    //the above section is for Retrying reading message from Message broker if the previous read failed ! 
                    configurator.UseMessageRetry(retryConfigurator => {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));// we repeat 3 times with 5 delai btw each repeat
                    });

                    //after this change you have to generat another nuget package "dotnet pack -p:PackageVersion=1.0.2 -o ..\package\"
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }
}
