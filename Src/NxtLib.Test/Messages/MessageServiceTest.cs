using Microsoft.Framework.Logging;
using NxtLib.Messages;
using NxtLib.Transactions;
using System.Linq;

namespace NxtLib.Test.Messages
{
    public interface IMessageServiceTest : ITest
    {
    }

    public class MessageServiceTest : TestBase, IMessageServiceTest
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        private readonly ITransactionService _transactionService;

        public MessageServiceTest(ILogger logger, IMessageService messageService, ITransactionService transactionService)
        {
            _logger = logger;
            _messageService = messageService;
            _transactionService = transactionService;
        }

        public void RunAllTests()
        {
            DecryptTextFrom();
            DecryptDataFrom();
            EncryptTextTo();
            EncryptDataTo();
        }

        private void EncryptDataTo()
        {
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            using (Logger = new TestsessionLogger(_logger))
            {
                // TODO: Decrypt locally
                var encrypted = _messageService.EncryptDataTo(TestSettings.Account2Rs, expected, true, TestSettings.SecretPhrase).Result;
                var decrypted = _messageService.DecryptDataFrom(TestSettings.AccountId, encrypted.Data, encrypted.Nonce, true, TestSettings.SecretPhrase2).Result;
                AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));
            }
        }

        private void EncryptTextTo()
        {
            const string expected = "Hello World!";

            using (Logger = new TestsessionLogger(_logger))
            {
                // TODO: Decrypt locally
                var encrypted = _messageService.EncryptTextTo(TestSettings.Account2Rs, expected, true, TestSettings.SecretPhrase).Result;
                var decrypted = _messageService.DecryptTextFrom(TestSettings.AccountId, encrypted.Data, encrypted.Nonce, true, TestSettings.SecretPhrase2).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
            }
        }

        private void DecryptDataFrom()
        {
            const string nonce = "b436d7be3750d4a9e41534b33d08e5260957bbffb2d1bb52c14847e45decdd1d";
            const string data = "f4340208430969b4460bf9d6e7cf9f577e8fc5c0d2558f6dd5e7d16b9a4bc8af";
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            using (Logger = new TestsessionLogger(_logger))
            {
                //TODO: Encrypt locally first
                var decrypted = _messageService.DecryptDataFrom(TestSettings.Account2Rs, data, nonce, false, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));

                var transaction = _transactionService.GetTransaction(TestSettings.SentDataMessageTransactionId).Result;
                decrypted = _messageService.DecryptDataFrom(TestSettings.Account2Rs, transaction.EncryptedMessage.Data,
                    transaction.EncryptedMessage.Nonce, transaction.EncryptedMessage.IsCompressed, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));
            }
        }

        private void DecryptTextFrom()
        {
            const string data = "32627f503c339c643a0dc6ef211b3291127f0767afc306afd3f634feab517c9f";
            const string nonce = "d000d04c219828caee765c5420c2de1cf0827c3b5299425da6798bc18da7adfa";
            const string expected = "Hello World!";

            using (Logger = new TestsessionLogger(_logger))
            {
                //TODO: Encrypt locally first
                var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2Rs, data, nonce, false, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));

                var transaction = _transactionService.GetTransaction(TestSettings.SentTextMessageTransactionId).Result;
                decrypted = _messageService.DecryptTextFrom(TestSettings.Account2Rs, transaction.EncryptedMessage.Data,
                    transaction.EncryptedMessage.Nonce, transaction.EncryptedMessage.IsCompressed, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
            }
        }
    }
}
