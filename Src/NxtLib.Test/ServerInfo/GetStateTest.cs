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
            AssertIsLargerThanZero(getStateReply.NumberOfAccounts, "NumberOfAccounts");
            AssertIsLargerThanZero(getStateReply.NumberOfActivePeers, "NumberOfActivePeers");
            AssertIsLargerThanZero(getStateReply.NumberOfAliases, "NumberOfAliases");
            AssertIsLargerThanZero(getStateReply.NumberOfAskOrders, "NumberOfAskOrders");
            AssertIsLargerThanZero(getStateReply.NumberOfAssets, "NumberOfAssets");
            AssertIsLargerThanZero(getStateReply.NumberOfBidOrders, "NumberOfBidOrders");
            AssertIsLargerThanZero(getStateReply.NumberOfCurrencies, "NumberOfCurrencies");
            AssertIsLargerThanZero(getStateReply.NumberOfCurrencyTransfers, "NumberOfCurrencyTransfers");
            AssertIsLargerThanZero(getStateReply.NumberOfDataTags, "NumberOfDataTags");
            AssertIsLargerThanZero(getStateReply.NumberOfExchanges, "NumberOfExchanges");
            AssertIsLargerThanZero(getStateReply.NumberOfGoods, "NumberOfGoods");
            AssertIsLargerThanZero(getStateReply.NumberOfOffers, "NumberOfOffers");
            AssertIsLargerThanZero(getStateReply.NumberOfOrders, "NumberOfOrders");
            AssertIsLargerThanZero(getStateReply.NumberOfPhasedTransactions, "NumberOfPhasedTransactions");
            AssertIsLargerThanZero(getStateReply.NumberOfPolls, "NumberOfPolls");
            AssertIsLargerThanZero(getStateReply.NumberOfPrunableMessages, "NumberOfPrunableMessages");
            AssertIsLargerThanZero(getStateReply.NumberOfPurchases, "NumberOfPurchases");
            AssertIsLargerThanZero(getStateReply.NumberOfTaggedData, "NumberOfTaggedData");
            AssertIsLargerThanZero(getStateReply.NumberOfTags, "NumberOfTags");
            AssertIsLargerThanZero(getStateReply.NumberOfTrades, "NumberOfTrades");
            AssertIsLargerThanZero(getStateReply.NumberOfTransactions, "NumberOfTransactions");
            AssertIsLargerThanZero(getStateReply.NumberOfTransfers, "NumberOfTransfers");
            AssertIsLargerThanZero(getStateReply.NumberOfVotes, "NumberOfVotes");
        }

        private void VerifyStateReply(GetStateReply getStateReply)
        {
            AssertIsLargerThanZero(getStateReply.AvailableProcessors, "AvailableProcessors");
            AssertIsLargerThanZero(getStateReply.FreeMemory, "FreeMemory");
            AssertIsFalse(getStateReply.IsOffline, "IsOffline");
            AssertIsLargerThanZero(getStateReply.MaxMemory, "MaxMemory");
            AssertIsFalse(getStateReply.NeedsAdminPassword, "NeedsAdminPassword");
            AssertIsLargerThanZero(getStateReply.NumberOfPeers, "NumberOfPeers");
            // getStateReply.NumberOfUnlockedAccounts
            AssertIsLargerThanZero(getStateReply.PeerPort, "PeerPort");
            AssertIsLargerThanZero(getStateReply.TotalMemory, "TotalMemory");
        }
    }
}