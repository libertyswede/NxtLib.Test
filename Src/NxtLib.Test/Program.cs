using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using NxtLib.Test.Accounts;
using NxtLib.Test.AssetExchange;
using NxtLib.Test.ServerInfo;
using NxtLib.Test.TaggedData;
using NxtLib.Test.Tokens;
using NxtLib.Test.Transactions;
using NxtLib.Test.Utils;
using NxtLib.Test.VotingSystem;
using Microsoft.Framework.Logging;
using Microsoft.Framework.DependencyInjection;
using NxtLib.Test.Local;
using NxtLib.Test.Messages;
using NxtLib.Local;

namespace NxtLib.Test
{
    public class Program
    {
        private static readonly Dictionary<string, string> InternalSettings = new Dictionary<string, string>();
        public static readonly ReadOnlyDictionary<string, string> AppSettings = new ReadOnlyDictionary<string, string>(InternalSettings);
        private readonly IServiceProvider _serviceProvider;
        private ServiceFactory _serviceFactory;

        public Program(IApplicationEnvironment env)
        {
            AddConfigurationSources(env);
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(provider => env);
            _serviceProvider = services.BuildServiceProvider();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void AddConfigurationSources(IApplicationEnvironment env)
        {
            var config = new ConfigurationBuilder();
            config.SetBasePath(env.ApplicationBasePath);
            config.AddJsonFile("config.json");
            config.AddJsonFile("config-Development.json", true);
            var configRoot = config.Build();
            foreach (var configurationSection in configRoot.GetChildren())
            {
                InternalSettings.Add(configurationSection.Key, configurationSection.Value);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Debug);
            _serviceFactory = new ServiceFactory(TestSettings.NxtServerUrl);

            // TODO: Fix this
            services.AddTransient(provider => loggerFactory.CreateLogger(""));

            services.AddTransient<ITestInitializer, TestInitializer>();
            services.AddTransient<ITransactionServiceTest, TransactionServiceTest>();
            services.AddTransient<IServerInfoServiceTest, ServerInfoServiceTest>();
            services.AddTransient<IUtilsServiceTest, UtilsServiceTest>();
            services.AddTransient<IAccountServiceTest, AccountServiceTest>();
            services.AddTransient<IAssetExchangeServiceTest, AssetExchangeServiceTest>();
            services.AddTransient<IVotingSystemServiceTest, VotingSystemServiceTest>();
            services.AddTransient<ITaggedDataServiceTest, TaggedDataServiceTest>();
            services.AddTransient<ITokenServiceTest, TokenServiceTest>();
            services.AddTransient<IGetBlockchainStatusTest, GetBlockchainStatusTest>();
            services.AddTransient<IGetConstantsTest, GetConstantsTest>();
            services.AddTransient<IGetStateTest, GetStateTest>();
            services.AddTransient<IGetPollResultTest, GetPollResultTest>();
            services.AddTransient<IGetPollTest, GetPollTest>();
            services.AddTransient<ICastVoteTest, CastVoteTest>();
            services.AddTransient<ICreatePollTest, CreatePollTest>();
            services.AddTransient<IMessageServiceTest, MessageServiceTest>();
            services.AddTransient<ILocalCryptoTest, LocalCryptoTest>();
            services.AddTransient<ILocalPasswordGenerator, LocalPasswordGenerator>();
            services.AddTransient<ILocalPasswordGeneratorTest, LocalPasswordGeneratorTest>();
            services.AddTransient<ILocalAccountService, LocalAccountService>();
            services.AddTransient<ILocalAccountServiceTest, LocalAccountServiceTest>();
            services.AddTransient<ILocalMessageService, LocalMessageService>();
            services.AddTransient<ILocalMessageServiceTest, LocalMessageServiceTest>();
            services.AddTransient<ILocalTokenService, LocalTokenService>();
            services.AddTransient<ILocalTokenServiceTest, LocalTokenServiceTest>();
            services.AddTransient<ILocalTransactionService, LocalTransactionService>();


            services.AddSingleton<IServiceFactory>(provider => _serviceFactory);
            services.AddInstance(_serviceFactory.CreateAccountService());
            services.AddInstance(_serviceFactory.CreateAliasService());
            services.AddInstance(_serviceFactory.CreateAssetExchangeService());
            services.AddInstance(_serviceFactory.CreateBlockService());
            services.AddInstance(_serviceFactory.CreateDebugService());
            services.AddInstance(_serviceFactory.CreateDigitalGoodsStoreService());
            services.AddInstance(_serviceFactory.CreateForgingService());
            services.AddInstance(_serviceFactory.CreateMessageService());
            services.AddInstance(_serviceFactory.CreateMonetarySystemService());
            services.AddInstance(_serviceFactory.CreateNetworkingService());
            services.AddInstance(_serviceFactory.CreatePhasingService());
            services.AddInstance(_serviceFactory.CreateServerInfoService());
            services.AddInstance(_serviceFactory.CreateTaggedDataService());
            services.AddInstance(_serviceFactory.CreateTokenService());
            services.AddInstance(_serviceFactory.CreateTransactionService());
            services.AddInstance(_serviceFactory.CreateUtilService());
            services.AddInstance(_serviceFactory.CreateVotingSystemService());
        }

        public void Main()
        {
            var logger = _serviceProvider.GetService<ILogger>();
            logger.LogInformation("Starting test run");
            logger.LogWarning("Have you rememberd to switch on MissingMemberHandling in NxtLib?");
            Console.ReadLine();

            var testInitializer = _serviceProvider.GetService<ITestInitializer>();
            testInitializer.InitializeTest();

            var localCryptoTest = _serviceProvider.GetService<ILocalCryptoTest>();
            localCryptoTest.RunAllTests();
            var tokenServiceTest = _serviceProvider.GetService<ITokenServiceTest>();
            tokenServiceTest.RunAllTests();
            var messageServiceTest = _serviceProvider.GetService<IMessageServiceTest>();
            messageServiceTest.RunAllTests();
            var transactionServiceTest = _serviceProvider.GetService<ITransactionServiceTest>();
            transactionServiceTest.RunAllTests();
            var serverInfoServiceTest = _serviceProvider.GetService<IServerInfoServiceTest>();
            serverInfoServiceTest.RunAllTests();
            var utilsServiceTest = _serviceProvider.GetService<IUtilsServiceTest>();
            utilsServiceTest.RunAllTests();
            var accountServiceTest = _serviceProvider.GetService<IAccountServiceTest>();
            accountServiceTest.RunAllTests();
            var assetExchangeServiceTest = _serviceProvider.GetService<IAssetExchangeServiceTest>();
            assetExchangeServiceTest.RunAllTests();
            var votingSystemServiceTest = _serviceProvider.GetService<IVotingSystemServiceTest>();
            votingSystemServiceTest.RunAllTests();
            var taggedDataServiceTest = _serviceProvider.GetService<ITaggedDataServiceTest>();
            taggedDataServiceTest.RunAllTests();

            // var debugService = new DebugServiceTest();
            // debugService.RunAllTests();

            Console.WriteLine("Test run complete");
            Console.ReadLine();
        }
    }
}
