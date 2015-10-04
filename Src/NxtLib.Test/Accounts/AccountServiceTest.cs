using NxtLib.Accounts;

namespace NxtLib.Test.Accounts
{
    internal class AccountServiceTest : TestBase
    {
        private readonly IAccountService _service;

        internal AccountServiceTest()
        {
            _service = TestSettings.ServiceFactory.CreateAccountService();
        }

        internal void RunAllTests()
        {
            TestGetAccountLedger();
            TestGetAccountLedgerEntry();
        }

        private void TestGetAccountLedger()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetAccountLedger("NXT-DV37-DD8K-3Q74-8LE2W", includeTransactions: true).Result;
            }
        }

        private void TestGetAccountLedgerEntry()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetAccountLedgerEntry(21417, true).Result;
            }
        }
    }
}
