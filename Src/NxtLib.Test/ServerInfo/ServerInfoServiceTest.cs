using System;
using System.Linq;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    class ServerInfoServiceTest : TestBase
    {
        private readonly IServerInfoService _serverInfoService;

        internal ServerInfoServiceTest()
        {
            _serverInfoService = TestSettings.ServiceFactory.CreateServerInfoService();
        }

        internal void RunAllTests()
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
            using (Logger = new TestsessionLogger())
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
            using (Logger = new TestsessionLogger())
            {
                //_serverInfoService.EventWait(2).Wait();
            }
        }

        private void GetBlockchainStatusTest()
        {
            var getBlockchainStatusTest = new GetBlockchainStatusTest(_serverInfoService);
            getBlockchainStatusTest.Test();
        }

        void GetConstantsTest()
        {
            var getConstantsTest = new GetConstantsTest(_serverInfoService);
            getConstantsTest.RunAllTests();
        }

        private void GetPluginsTest()
        {
            var getPluginsReply = _serverInfoService.GetPlugins().Result;
            if (getPluginsReply.Plugins.Count != 1)
            {
                Logger.Fail(string.Format("Unexpected number of plugins, expected: 1, actual: {0}", getPluginsReply.Plugins.Count));
            }
            if (!getPluginsReply.Plugins.Single().Equals("hello_world"))
            {
                Logger.Fail(string.Format("Unexpected name of plugin, expected: hello_world, actual: {0}", getPluginsReply.Plugins.Single()));
            }
        }

        private void GetStateTest()
        {
            var getStateTest = new GetStateTest(_serverInfoService);
            getStateTest.RunAllTests();
        }

        private void GetTimeTest()
        {
            var getTimeReply = _serverInfoService.GetTime().Result;
            if (Math.Abs(getTimeReply.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Fail(string.Format("Unexpected Time, expected to be within 10 seconds ({0}), actual: {1}",
                    DateTime.UtcNow.ToLongTimeString(), getTimeReply.Time.ToLongTimeString()));
            }
        }
    }
}
