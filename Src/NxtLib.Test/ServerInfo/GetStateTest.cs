using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    internal class GetStateTest : TestBase
    {
        private readonly IServerInfoService _serverInfoService;
        private readonly GetBlockchainStatusTest _getBlockchainStatusTest;

        public GetStateTest(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
            _getBlockchainStatusTest = new GetBlockchainStatusTest(_serverInfoService);
        }

        public void RunAllTests()
        {
            GetStateNoCounts();
            GetStateDefaultParameter();
            GetStateWithCounts();
        }

        private void GetStateWithCounts()
        {
            using (Logger = new TestsessionLogger())
            {
                var getStateReplyWithExplicitCounts = _serverInfoService.GetState(true).Result;
                _getBlockchainStatusTest.Test(getStateReplyWithExplicitCounts);
                VerifyStateReply(getStateReplyWithExplicitCounts);
                VerifyStateCounts(getStateReplyWithExplicitCounts);
            }
        }

        private void GetStateDefaultParameter()
        {
            using (Logger = new TestsessionLogger())
            {
                var getStateReplyWithImplicitCounts = _serverInfoService.GetState().Result;
                _getBlockchainStatusTest.Test(getStateReplyWithImplicitCounts);
                VerifyStateReply(getStateReplyWithImplicitCounts);
                VerifyStateCounts(getStateReplyWithImplicitCounts);
            }
        }

        private void GetStateNoCounts()
        {
            using (Logger = new TestsessionLogger())
            {
                var getStateReplyNoCounts = _serverInfoService.GetState(false).Result;
                _getBlockchainStatusTest.Test(getStateReplyNoCounts);
                VerifyStateReply(getStateReplyNoCounts);
            }
        }

        private void VerifyStateCounts(GetStateReply getStateReply)
        {
            CheckLargerThanZero(getStateReply.NumberOfAccounts, "NumberOfAccounts");
            CheckLargerThanZero(getStateReply.NumberOfActivePeers, "NumberOfActivePeers");
            CheckLargerThanZero(getStateReply.NumberOfAliases, "NumberOfAliases");
            CheckLargerThanZero(getStateReply.NumberOfAskOrders, "NumberOfAskOrders");
            CheckLargerThanZero(getStateReply.NumberOfAssets, "NumberOfAssets");
            CheckLargerThanZero(getStateReply.NumberOfBidOrders, "NumberOfBidOrders");
            CheckLargerThanZero(getStateReply.NumberOfCurrencies, "NumberOfCurrencies");
            CheckLargerThanZero(getStateReply.NumberOfCurrencyTransfers, "NumberOfCurrencyTransfers");
            CheckLargerThanZero(getStateReply.NumberOfDataTags, "NumberOfDataTags");
            CheckLargerThanZero(getStateReply.NumberOfExchanges, "NumberOfExchanges");
            CheckLargerThanZero(getStateReply.NumberOfGoods, "NumberOfGoods");
            CheckLargerThanZero(getStateReply.NumberOfOffers, "NumberOfOffers");
            CheckLargerThanZero(getStateReply.NumberOfOrders, "NumberOfOrders");
            CheckLargerThanZero(getStateReply.NumberOfPhasedTransactions, "NumberOfPhasedTransactions");
            CheckLargerThanZero(getStateReply.NumberOfPolls, "NumberOfPolls");
            CheckLargerThanZero(getStateReply.NumberOfPrunableMessages, "NumberOfPrunableMessages");
            CheckLargerThanZero(getStateReply.NumberOfPurchases, "NumberOfPurchases");
            CheckLargerThanZero(getStateReply.NumberOfTaggedData, "NumberOfTaggedData");
            CheckLargerThanZero(getStateReply.NumberOfTags, "NumberOfTags");
            CheckLargerThanZero(getStateReply.NumberOfTrades, "NumberOfTrades");
            CheckLargerThanZero(getStateReply.NumberOfTransactions, "NumberOfTransactions");
            CheckLargerThanZero(getStateReply.NumberOfTransfers, "NumberOfTransfers");
            CheckLargerThanZero(getStateReply.NumberOfVotes, "NumberOfVotes");
        }

        private void VerifyStateReply(GetStateReply getStateReply)
        {
            CheckLargerThanZero(getStateReply.AvailableProcessors, "AvailableProcessors");
            CheckLargerThanZero(getStateReply.FreeMemory, "FreeMemory");
            CheckIsFalse(getStateReply.IsOffline, "IsOffline");
            CheckLargerThanZero(getStateReply.MaxMemory, "MaxMemory");
            CheckIsFalse(getStateReply.NeedsAdminPassword, "NeedsAdminPassword");
            CheckLargerThanZero(getStateReply.NumberOfPeers, "NumberOfPeers");
            // getStateReply.NumberOfUnlockedAccounts
            CheckLargerThanZero(getStateReply.PeerPort, "PeerPort");
            CheckLargerThanZero(getStateReply.TotalMemory, "TotalMemory");
        }
    }
}