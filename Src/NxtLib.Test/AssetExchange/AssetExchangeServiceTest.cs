using Microsoft.Framework.Logging;
using NxtLib.AssetExchange;

namespace NxtLib.Test.AssetExchange
{
    public interface IAssetExchangeServiceTest : ITest
    {
    }

    public class AssetExchangeServiceTest : TestBase, IAssetExchangeServiceTest
    {
        private readonly IAssetExchangeService _service;
        private readonly ILogger _logger;

        public AssetExchangeServiceTest(IAssetExchangeService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public void RunAllTests()
        {
            TestDeleteAssetShares();
            TestGetExpectedOrderCancellations();
            TestGetOrderTrades();
        }

        private void TestGetOrderTrades()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetOrderTrades(OrderIdLocator.ByAskOrderId(3388049137599128377), includeAssetInfo:true).Result;
            }
        }

        private void TestGetExpectedOrderCancellations()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetExpectedOrderCancellations().Result;
            }
        }

        private void TestDeleteAssetShares()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var deleteAssetSharesReply = _service.DeleteAssetShares(TestSettings.ExistingAssetId, 1, 
                    CreateTransaction.CreateTransactionByPublicKey()).Result;
            }
        }
    }
}
