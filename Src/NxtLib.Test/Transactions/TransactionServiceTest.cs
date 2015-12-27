using Microsoft.Extensions.Logging;
using NxtLib.Accounts;
using NxtLib.Transactions;

namespace NxtLib.Test.Transactions
{
    public interface ITransactionServiceTest : ITest
    {
    }

    public class TransactionServiceTest : TestBase, ITransactionServiceTest
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public TransactionServiceTest(ITransactionService transactionService, IAccountService accountService, ILogger logger)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _logger = logger;
        }

        public void RunAllTests()
        {
            TestCalculateFullHash();
            TestGetExpectedTransactions();
            TestRetrievePrunedTransaction();
            TestSignTransaction();
        }

        private void TestRetrievePrunedTransaction()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var reply = _transactionService.RetrievePrunedTransaction(18060986091603027950).Result;
            }
        }

        private void TestCalculateFullHash()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var unsignedSendMoney = _accountService.SendMoney(CreateTransaction.CreateTransactionByPublicKey(), TestSettings.Account2.AccountRs, Amount.OneNqt).Result;
                var signedTransaction = _transactionService.SignTransaction(new TransactionParameter(unsignedSendMoney.UnsignedTransactionBytes), TestSettings.SecretPhrase1).Result;
                var signatureHash = signedTransaction.Transaction.SignatureHash;

                var calculateFullHash = _transactionService.CalculateFullHash(new BinaryHexString(signatureHash), unsignedSendMoney.UnsignedTransactionBytes).Result;
                AssertEquals(signedTransaction.FullHash.ToHexString(), calculateFullHash.FullHash.ToHexString(), "FullHash");
            }
        }

        private void TestSignTransaction()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var amount = Amount.OneNqt;
                var recipient = TestSettings.Account2.AccountRs;
                var unsignedSendMoney = _accountService.SendMoney(CreateTransaction.CreateTransactionByPublicKey(), recipient, amount).Result;

                var signedTransaction = _transactionService.SignTransaction(new TransactionParameter(unsignedSendMoney.UnsignedTransactionBytes), TestSettings.SecretPhrase1).Result;

                var transaction = signedTransaction.Transaction;
                AssertEquals(transaction.Amount.Nqt, Amount.OneNqt.Nqt, "Amount");
                AssertEquals(transaction.RecipientRs, recipient, "RecipientRs");
            }
        }

        private void TestGetExpectedTransactions()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _transactionService.GetExpectedTransactions().Result;
            }
        }
    }
}
