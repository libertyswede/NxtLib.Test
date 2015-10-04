using NxtLib.Transactions;

namespace NxtLib.Test.Transactions
{
    internal class TransactionServiceTest : TestBase
    {
        private readonly ITransactionService _service;

        internal TransactionServiceTest()
        {
            _service = TestSettings.ServiceFactory.CreateTransactionService();
        }

        internal void RunAllTests()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetExpectedTransactions().Result;
            }
        }
    }
}
