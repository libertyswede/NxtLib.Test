using NLog;
using NxtLib.Accounts;

namespace NxtLib.Test
{
    public static class TestInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void InitializeTest()
        {
            Logger.Info("Fetching number of blocks");
            TestSettings.MaxHeight = GetCurrentHeight();
            Logger.Info("Setting account properties");
            GetAccountProperties();
        }

        private static void GetAccountProperties()
        {
            var accountIdLocator = AccountIdLocator.BySecretPhrase(TestSettings.SecretPhrase);
            var accountService = TestSettings.ServiceFactory.CreateAccountService();
            var accountIdReply = accountService.GetAccountId(accountIdLocator).Result;
            TestSettings.AccountId = accountIdReply.AccountId;
            TestSettings.AccountRs = accountIdReply.AccountRs;
            TestSettings.PublicKey = accountIdReply.PublicKey;
        }

        private static int GetCurrentHeight()
        {
            var serverInfoService = TestSettings.ServiceFactory.CreateServerInfoService();
            var blockchainStatus = serverInfoService.GetBlockchainStatus().Result;
            return blockchainStatus.NumberOfBlocks;
        }
    }
}
