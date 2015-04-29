using System;
using System.Configuration;
using NLog;
using NxtLib.Test.ServerInfo;

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

            var serverInfoServiceTest = new ServerInfoServiceTest(serviceFactory.CreateServerInfoService());
            serverInfoServiceTest.RunAllTests();

            Logger.Info("Test run complete");
            Console.ReadLine();
        }
    }
}
