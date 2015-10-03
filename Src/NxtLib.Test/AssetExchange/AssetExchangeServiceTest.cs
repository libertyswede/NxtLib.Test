using NxtLib.AssetExchange;

namespace NxtLib.Test.AssetExchange
{
    class AssetExchangeServiceTest : TestBase
    {
        private IAssetExchangeService _service;

        internal AssetExchangeServiceTest()
        {
            _service = TestSettings.ServiceFactory.CreateAssetExchangeService();
        }

        public void RunAllTests()
        {
            TestDeleteAssetShares();
        }

        private void TestDeleteAssetShares()
        {
            using (Logger = new TestsessionLogger())
            {
                var deleteAssetSharesReply = _service.DeleteAssetShares(6926770479287491943, 1,
                    new CreateTransactionByPublicKey(1440, Amount.OneNxt,
                        new BinaryHexString("4e919871578a02cb2afc600c5c03414aa026d93a338ad0d098513ea0fe1b3056"))).Result;
            }
        }
    }
}
