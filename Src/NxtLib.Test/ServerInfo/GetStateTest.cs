using NLog;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    internal class GetStateTest
    {
        private readonly IServerInfoService _serverInfoService;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GetBlockchainStatusTest _getBlockchainStatusTest;

        public GetStateTest(IServerInfoService serverInfoService)
        {
            _serverInfoService = serverInfoService;
            _getBlockchainStatusTest = new GetBlockchainStatusTest(_serverInfoService);
        }

        public void RunAllTests()
        {
            var getStateReplyNoCounts = _serverInfoService.GetState(false).Result;
            _getBlockchainStatusTest.RunAllTests(getStateReplyNoCounts);
            VerifyStateReply(getStateReplyNoCounts);

            var getStateReplyWithImplicitCounts = _serverInfoService.GetState().Result;
            _getBlockchainStatusTest.RunAllTests(getStateReplyWithImplicitCounts);
            VerifyStateReply(getStateReplyWithImplicitCounts);
            VerifyStateCounts(getStateReplyWithImplicitCounts);

            var getStateReplyWithExplicitCounts = _serverInfoService.GetState(true).Result;
            _getBlockchainStatusTest.RunAllTests(getStateReplyWithExplicitCounts);
            VerifyStateReply(getStateReplyWithExplicitCounts);
            VerifyStateCounts(getStateReplyWithImplicitCounts);
        }

        private void VerifyStateCounts(GetStateReply getStateReply)
        {
            ExpectMoreThanZero(getStateReply.NumberOfAccounts, "NumberOfAccounts");
            ExpectMoreThanZero(getStateReply.NumberOfActivePeers, "NumberOfActivePeers");
            ExpectMoreThanZero(getStateReply.NumberOfAliases, "NumberOfAliases");
            ExpectMoreThanZero(getStateReply.NumberOfAskOrders, "NumberOfAskOrders");
            ExpectMoreThanZero(getStateReply.NumberOfAssets, "NumberOfAssets");
            ExpectMoreThanZero(getStateReply.NumberOfBidOrders, "NumberOfBidOrders");
            ExpectMoreThanZero(getStateReply.NumberOfCurrencies, "NumberOfCurrencies");
            ExpectMoreThanZero(getStateReply.NumberOfCurrencyTransfers, "NumberOfCurrencyTransfers");
            ExpectMoreThanZero(getStateReply.NumberOfDataTags, "NumberOfDataTags");
            ExpectMoreThanZero(getStateReply.NumberOfExchanges, "NumberOfExchanges");
            ExpectMoreThanZero(getStateReply.NumberOfGoods, "NumberOfGoods");
            ExpectMoreThanZero(getStateReply.NumberOfOffers, "NumberOfOffers");
            ExpectMoreThanZero(getStateReply.NumberOfOrders, "NumberOfOrders");
            ExpectMoreThanZero(getStateReply.NumberOfPhasedTransactions, "NumberOfPhasedTransactions");
            ExpectMoreThanZero(getStateReply.NumberOfPolls, "NumberOfPolls");
            ExpectMoreThanZero(getStateReply.NumberOfPrunableMessages, "NumberOfPrunableMessages");
            ExpectMoreThanZero(getStateReply.NumberOfPurchases, "NumberOfPurchases");
            ExpectMoreThanZero(getStateReply.NumberOfTaggedData, "NumberOfTaggedData");
            ExpectMoreThanZero(getStateReply.NumberOfTags, "NumberOfTags");
            ExpectMoreThanZero(getStateReply.NumberOfTrades, "NumberOfTrades");
            ExpectMoreThanZero(getStateReply.NumberOfTransactions, "NumberOfTransactions");
            ExpectMoreThanZero(getStateReply.NumberOfTransfers, "NumberOfTransfers");
            ExpectMoreThanZero(getStateReply.NumberOfVotes, "NumberOfVotes");
        }

        private static void ExpectMoreThanZero(long i, string propertyName)
        {
            if (i <= 0)
            {
                Logger.Error("Unexpected {0}, expected > 0, actual: {1}", propertyName, i);
            }
        }

        private void VerifyStateReply(GetStateReply getStateReply)
        {
            ExpectMoreThanZero(getStateReply.AvailableProcessors, "AvailableProcessors");
            if (getStateReply.FreeMemory == 0)
            {
                Logger.Error("Unexpected FreeMemory, expected > 0, actual: 0");
            }
            if (getStateReply.IsOffline)
            {
                Logger.Error("Unexpected IsOffline, expected false, actual: true");
            }
            ExpectMoreThanZero(getStateReply.MaxMemory, "MaxMemory");
            if (getStateReply.NeedsAdminPassword)
            {
                Logger.Error("Unexpected NeedsAdminPassword, expected false, actual: true");
            }
            ExpectMoreThanZero(getStateReply.NumberOfPeers, "NumberOfPeers");
            // getStateReply.NumberOfUnlockedAccounts
            ExpectMoreThanZero(getStateReply.PeerPort, "PeerPort");
            ExpectMoreThanZero(getStateReply.TotalMemory, "TotalMemory");
        }
    }
}