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
        }

        void GetConstantsTest()
        {
            var getConstantsTest = new GetConstantsTest(_serverInfoService);
            getConstantsTest.RunAllTests();
        }
    }
}
