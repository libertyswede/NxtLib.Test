using NxtLib.AssetExchange;

namespace NxtLib.Test.AssetExchange
{
    class AssetExchangeServiceTest : TestBase
    {
        private readonly IAssetExchangeService _service;

        internal AssetExchangeServiceTest()
        {
            _service = TestSettings.ServiceFactory.CreateAssetExchangeService();
        }

        public void RunAllTests()
        {
            TestDeleteAssetShares();
            TestGetExpectedOrderCancellations();
            TestGetOrderTrades();
        }

        private void TestGetOrderTrades()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetOrderTrades(OrderIdLocator.ByAskOrderId(3388049137599128377), includeAssetInfo:true).Result;
            }
        }

        private void TestGetExpectedOrderCancellations()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetExpectedOrderCancellations().Result;
            }
        }

        private void TestDeleteAssetShares()
        {
            using (Logger = new TestsessionLogger())
            {
                var deleteAssetSharesReply = _service.DeleteAssetShares(TestSettings.ExistingAssetId, 1, 
                    CreateTransaction.CreateTransactionByPublicKey()).Result;
            }
        }
    }
}
