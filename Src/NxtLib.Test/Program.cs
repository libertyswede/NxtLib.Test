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

            var nxtServer = ConfigurationManager.AppSettings["NxtServerUrl"];
            var serviceFactory = new ServiceFactory(nxtServer);

            var serverInfoServiceTest = new ServerInfoServiceTest(serviceFactory.CreateServerInfoService());
            serverInfoServiceTest.RunAllTests();

            Logger.Info("Test run complete");
            Console.ReadLine();
        }
    }
}
