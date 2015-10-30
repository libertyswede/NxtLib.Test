using System.Linq;
using Microsoft.Framework.Logging;
using NxtLib.Accounts;

namespace NxtLib.Test.Accounts
{
    public interface IAccountServiceTest : ITest
    {
    }

    public class AccountServiceTest : TestBase, IAccountServiceTest
    {
        private readonly IAccountService _service;
        private readonly ILogger _logger;
        private int _ledgerId;

        public AccountServiceTest(IAccountService accountService, ILogger logger)
        {
            _service = accountService;
            _logger = logger;
        }

        public void RunAllTests()
        {
            TestGetAccountLedger();
            TestGetAccountLedgerEntry();
        }

        private void TestGetAccountLedger()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetAccountLedger("NXT-DV37-DD8K-3Q74-8LE2W", includeTransactions: true).Result;
                _ledgerId = result.Entries.First().LedgerId;
            }
        }

        private void TestGetAccountLedgerEntry()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetAccountLedgerEntry(_ledgerId, true).Result;
            }
        }
    }
}
