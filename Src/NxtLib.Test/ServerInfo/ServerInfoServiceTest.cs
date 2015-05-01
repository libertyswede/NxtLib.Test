using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    class ServerInfoServiceTest
    {
        private readonly IServerInfoService _serverInfoService;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            try
            {
                var eventRegisterResponse = _serverInfoService.EventRegister(new List<string>(), "", "").Result;
                Logger.Error("Test code for EventRegister is not implemented yet");
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is NotImplementedException)
                    {
                        Logger.Error("EventRegister is not implemented yet");
                        return true;
                    }
                    return false;
                });
            }
        }

        private void EventWaitTest()
        {
            try
            {
                var eventRegisterResponse = _serverInfoService.EventWait(123).Result;
                Logger.Error("Test code for EventWait is not implemented yet");
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is NotImplementedException)
                    {
                        Logger.Error("EventWait is not implemented yet");
                        return true;
                    }
                    return false;
                });
            }
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
                Logger.Error("Unexpected Time, expected to be within 10 seconds ({0}), actual: {1}",
                    DateTime.UtcNow.ToLongTimeString(), getTimeReply.Time.ToLongTimeString());
            }
        }
    }
}
