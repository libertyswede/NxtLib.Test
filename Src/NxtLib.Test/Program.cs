using System;
using System.Configuration;
using NLog;
using NxtLib.Test.ServerInfo;
using NxtLib.Test.VotingSystem;

namespace NxtLib.Test
{
    class Program
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            Logger.Info("Starting test run");
            Logger.Warn("Have you rememberd to switch on MissingMemberHandling in NxtLib?");
            Console.ReadLine();

            var nxtServer = ConfigurationManager.AppSettings["NxtServerUrl"];
            var serviceFactory = new ServiceFactory(nxtServer);

            var searchAccountsResult = serviceFactory.CreateAccountService().SearchAccounts("test").Result;

            var serverInfoServiceTest = new ServerInfoServiceTest(serviceFactory.CreateServerInfoService());
            serverInfoServiceTest.RunAllTests();
            var votingSystemServiceTest = new VotingSystemServiceTest(serviceFactory.CreateVotingSystemService());
            votingSystemServiceTest.RunAllTests();

            Logger.Info("Test run complete");
            Console.ReadLine();
        }
    }
}
