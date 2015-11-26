using Microsoft.Extensions.Logging;
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
            TestGetAccountAssets();
            TestGetExpectedOrderCancellations();
            TestGetOrderTrades();
        }

        private void TestGetAccountAssets()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetAccountAssets(TestSettings.Account1.AccountRs, includeAssetInfo: false).Result;
                VerifyAccountAssets(result, 3, false);
                result = _service.GetAccountAssets(TestSettings.Account1.AccountRs, includeAssetInfo: true).Result;
                VerifyAccountAssets(result, 3, true);
                result = _service.GetAccountAssets(TestSettings.Account1.AccountRs, TestSettings.ExistingAssetId, true).Result;
                VerifyAccountAssets(result, 1, true);
            }
        }

        private static void VerifyAccountAssets(AccountAssetsReply result, int expectedCount, bool includeAssetInfo)
        {
            AssertEquals(expectedCount, result.AccountAssets.Count, $"Number of assets on {TestSettings.Account1.AccountRs}");
            result.AccountAssets.ForEach(a => AssertIsLargerThanZero(a.AssetId, nameof(a.AssetId)));
            result.AccountAssets.ForEach(a => AssertIsLargerThanZero(a.QuantityQnt, nameof(a.QuantityQnt)));
            result.AccountAssets.ForEach(a => AssertIsLargerThanZero(a.UnconfirmedQuantityQnt, nameof(a.UnconfirmedQuantityQnt)));

            if (includeAssetInfo)
            {
                result.AccountAssets.ForEach(a => AssertIsNotNullOrEmpty(a.Name, nameof(a.Name)));
            }
            else
            {
                result.AccountAssets.ForEach(a => AssertIsNull(a.Name, nameof(a.Name)));
                result.AccountAssets.ForEach(a => AssertEquals(0, a.Decimals, nameof(a.Decimals)));
            }
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
