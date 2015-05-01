using NLog;

namespace NxtLib.Test
{
    public static class TestInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void InitializeTest()
        {
            Logger.Info("Fetching number of blocks");
            TestSettings.MaxHeight = GetCurrentHeight();
            
        }

        private static int GetCurrentHeight()
        {
            var blockchainStatus = TestSettings.ServiceFactory.CreateServerInfoService().GetBlockchainStatus().Result;
            return blockchainStatus.NumberOfBlocks;
        }
    }
}
