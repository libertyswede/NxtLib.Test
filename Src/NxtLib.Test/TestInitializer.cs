using System.Collections.Generic;
using NLog;
using NxtLib.Accounts;
using NxtLib.VotingSystem;

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
            if (TestSettings.RunCostlyTests)
            {
                Logger.Info("Creating poll");
                CreatePoll();
            }
        }

        private static void CreatePoll()
        {
            var votingSystemServicer = TestSettings.ServiceFactory.CreateVotingSystemService();
            var createPollParameter = new CreatePollParameters("testpoll", "test poll", TestSettings.MaxHeight + 500,
                VotingModel.Asset, 1, 1, 0, 1, new List<string> {"how are you doing?"})
            {
                MinBalanceModel = MinBalanceModel.Asset,
                MinBalance = 1,
                HoldingId = TestSettings.ExistingAssetId
            };
            var createPoll = votingSystemServicer.CreatePoll(createPollParameter,
                CreateTransaction.CreateTransactionBySecretPhrase(true, Amount.CreateAmountFromNxt(10))).Result;
            if (createPoll.TransactionId != null) 
                TestSettings.PollId = createPoll.TransactionId.Value;
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
