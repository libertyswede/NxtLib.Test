using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace NxtLib.Test
{
    public class Program
    {
        private static readonly Dictionary<string, string> InternalSettings = new Dictionary<string, string>();
        public static readonly ReadOnlyDictionary<string, string> AppSettings = new ReadOnlyDictionary<string, string>(InternalSettings);
        private static IServiceProvider _serviceProvider;
        private static ServiceFactory _serviceFactory;

        public static void Main(string[] args)
        {
            var applicationEnvironment = PlatformServices.Default.Application;
            AddConfigurationSources(applicationEnvironment);
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(provider => applicationEnvironment);
            _serviceProvider = services.BuildServiceProvider();
            ConfigureServices(services);
            //_serviceProvider = services.BuildServiceProvider(); //Needed?
        }

        private static void AddConfigurationSources(ApplicationEnvironment applicationEnvironment)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(applicationEnvironment.ApplicationBasePath);
            configBuilder.AddJsonFile("config.json");
            configBuilder.AddJsonFile("config-Development.json", true);
            var configRoot = configBuilder.Build();
            foreach (var configurationSection in configRoot.GetChildren())
            {
                InternalSettings.Add(configurationSection.Key, configurationSection.Value);
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Debug);
            _serviceFactory = new ServiceFactory(TestSettings.NxtServerUrl);

            // TODO: Fix this
            //services.AddTransient(provider => loggerFactory.CreateLogger(""));


        }
    }
}
