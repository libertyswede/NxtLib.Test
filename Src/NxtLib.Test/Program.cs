﻿using System;
using NLog;
using NxtLib.Test.ServerInfo;
using NxtLib.Test.TaggedData;
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

            TestSettings.ServiceFactory = new ServiceFactory(TestSettings.NxtServerUrl);
            TestInitializer.InitializeTest();

            var serverInfoServiceTest = new ServerInfoServiceTest();
            serverInfoServiceTest.RunAllTests();
            var votingSystemServiceTest = new VotingSystemServiceTest();
            votingSystemServiceTest.RunAllTests();
            var taggedDataServiceTest = new TaggedDataServiceTest();
            taggedDataServiceTest.RunAllTests();

            Logger.Info("Test run complete");
            Console.ReadLine();
        }
    }
}
