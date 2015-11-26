using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    public interface IServerInfoServiceTest : ITest
    {
    }

    class ServerInfoServiceTest : TestBase, IServerInfoServiceTest
    {
        private readonly IServerInfoService _serverInfoService;
        private readonly ILogger _logger;
        private readonly IGetBlockchainStatusTest _getBlockchainStatusTest;
        private readonly IGetConstantsTest _getConstantsTest;
        private readonly IGetStateTest _getStateTest;

        public ServerInfoServiceTest(IServerInfoService serverInfoService, ILogger logger,
            IGetBlockchainStatusTest getBlockchainStatusTest, IGetConstantsTest getConstantsTest,
            IGetStateTest getStateTest)
        {
            _serverInfoService = serverInfoService;
            _logger = logger;
            _getBlockchainStatusTest = getBlockchainStatusTest;
            _getConstantsTest = getConstantsTest;
            _getStateTest = getStateTest;
        }

        public void RunAllTests()
        {
            EventRegisterTest();
            EventWaitTest();
            GetConstantsTest();
            GetBlockchainStatusTest();
            GetPluginsTest();
            GetStateTest();
            GetTimeTest();
        }

        private void EventRegisterTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var eventRegisterResponse = _serverInfoService.EventRegister().Result;
                if (!eventRegisterResponse.Registered)
                {
                    Logger.Fail("EventRegister.Registered is false");
                }
            }
        }

        private void EventWaitTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                //_serverInfoService.EventWait(2).Wait();
            }
        }

        private void GetBlockchainStatusTest()
        {
            _getBlockchainStatusTest.Test();
        }

        void GetConstantsTest()
        {
            _getConstantsTest.RunAllTests();
        }

        private void GetPluginsTest()
        {
            var getPluginsReply = _serverInfoService.GetPlugins().Result;
            if (getPluginsReply.Plugins.Count != 1)
            {
                Logger.Fail($"Unexpected number of plugins, expected: 1, actual: {getPluginsReply.Plugins.Count}");
            }
            if (!getPluginsReply.Plugins.Single().Equals("hello_world"))
            {
                Logger.Fail($"Unexpected name of plugin, expected: hello_world, actual: {getPluginsReply.Plugins.Single()}");
            }
        }

        private void GetStateTest()
        {
            _getStateTest.RunAllTests();
        }

        private void GetTimeTest()
        {
            var getTimeReply = _serverInfoService.GetTime().Result;
            if (Math.Abs(getTimeReply.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Fail($"Unexpected Time, expected to be within 10 seconds ({DateTime.UtcNow.ToString("T")}), actual: {getTimeReply.Time.ToString("T")}");
            }
        }
    }
}
