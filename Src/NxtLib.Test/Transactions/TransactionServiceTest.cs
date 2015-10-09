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
            TestCalculateFullHash();
            TestGetExpectedTransactions();
            TestSignTransaction();
        }

        private void TestCalculateFullHash()
        {
            using (Logger = new TestsessionLogger())
            {
                var accountService = TestSettings.ServiceFactory.CreateAccountService();
                var unsignedSendMoney = accountService.SendMoney(CreateTransaction.CreateTransactionByPublicKey(), TestSettings.Account2Rs, Amount.OneNqt).Result;
                var signedTransaction = _service.SignTransaction(new TransactionParameter(unsignedSendMoney.UnsignedTransactionBytes), TestSettings.SecretPhrase).Result;
                var signatureHash = signedTransaction.Transaction.SignatureHash;

                var calculateFullHash = _service.CalculateFullHash(new BinaryHexString(signatureHash), unsignedSendMoney.UnsignedTransactionBytes).Result;
                AssertEquals(signedTransaction.FullHash.ToHexString(), calculateFullHash.FullHash.ToHexString(), "FullHash");
            }
        }

        private void TestSignTransaction()
        {
            using (Logger = new TestsessionLogger())
            {
                var amount = Amount.OneNqt;
                var accountService = TestSettings.ServiceFactory.CreateAccountService();
                var recipient = TestSettings.Account2Rs;
                var unsignedSendMoney = accountService.SendMoney(CreateTransaction.CreateTransactionByPublicKey(), recipient, amount).Result;

                var signedTransaction = _service.SignTransaction(new TransactionParameter(unsignedSendMoney.UnsignedTransactionBytes), TestSettings.SecretPhrase).Result;

                var transaction = signedTransaction.Transaction;
                AssertEquals(transaction.Amount.Nqt, Amount.OneNqt.Nqt, "Amount");
                AssertEquals(transaction.RecipientRs, recipient, "RecipientRs");
            }
        }

        private void TestGetExpectedTransactions()
        {
            using (Logger = new TestsessionLogger())
            {
                var result = _service.GetExpectedTransactions().Result;
            }
        }
    }
}
