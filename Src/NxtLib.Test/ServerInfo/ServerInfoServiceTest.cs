using System;
using System.Linq;
using NLog;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    class ServerInfoServiceTest
    {
        private readonly IServerInfoService _serverInfoService;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal ServerInfoServiceTest(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
        }

        internal void RunAllTests()
        {
            GetConstantsTest();
            GetBlockchainStatusTest();
            GetPluginsTest();
            GetStateTest();
            GetTimeTest();
        }

        private void GetPluginsTest()
        {
            var getPluginsReply = _serverInfoService.GetPlugins().Result;
            if (getPluginsReply.Plugins.Count != 1)
            {
                Logger.Error("Unexpected number of plugins, expected: 1, actual: {0}", getPluginsReply.Plugins.Count);
            }
            if (!getPluginsReply.Plugins.Single().Equals("hello_world"))
            {
                Logger.Error("Unexpected name of plugin, expected: hello_world, actual: {0}", getPluginsReply.Plugins.Single());
            }
        }

        private void GetTimeTest()
        {
            var getTimeReply = _serverInfoService.GetTime().Result;
            if (Math.Abs(getTimeReply.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Error("Unexpected Time, expected to be within 10 seconds ({0}), actual: {1}",
                    DateTime.UtcNow.ToLongTimeString(), getTimeReply.Time.ToLongTimeString());
            }
        }

        private void GetStateTest()
        {
            var getStateTest = new GetStateTest(_serverInfoService);
            getStateTest.RunAllTests();
        }

        private void GetBlockchainStatusTest()
        {
            var getBlockchainStatusTest = new GetBlockchainStatusTest(_serverInfoService);
            getBlockchainStatusTest.RunAllTests();
        }

        void GetConstantsTest()
        {
            var getConstantsTest = new GetConstantsTest(_serverInfoService);
            getConstantsTest.RunAllTests();
        }
    }
}
