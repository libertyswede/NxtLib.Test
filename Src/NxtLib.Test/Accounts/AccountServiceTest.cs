using System.Linq;
using Microsoft.Extensions.Logging;
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
            TestSetAccountProperty();
            TestGetAccountProperties();
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

        private void TestSetAccountProperty()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var property = "key1";
                var value = "supersecret";

                var accountProperty = _service.SetAccountProperty(CreateTransaction.CreateTransactionByPublicKey(), property, value).Result;
                var attachment = (MessagingAccountPropertyAttachment) accountProperty.Transaction.Attachment;

                // ReSharper disable once PossibleInvalidOperationException
                AssertEquals(accountProperty.Transaction.Recipient.Value, TestSettings.Account1.AccountId, "recipient");
                AssertEquals(property, attachment.Property, nameof(attachment.Property));
                AssertEquals(value, attachment.Value, nameof(attachment.Value));
            }
        }

        private void TestGetAccountProperties()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var reply = _service.GetAccountProperties(TestSettings.Account1.AccountId, null).Result;

                AssertEquals(1, reply.Properties.Count(), "Properties count");
                var property = reply.Properties.Single();
                AssertEquals("testkey1", property.Property, nameof(property.Property));
                AssertEquals("testvalue1", property.Value, nameof(property.Value));
            }
        }
    }
}
