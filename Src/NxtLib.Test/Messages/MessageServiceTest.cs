using Microsoft.Extensions.Logging;
using NxtLib.Messages;
using NxtLib.Transactions;
using System.Linq;
using NxtLib.Local;

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
        private readonly ILocalMessageService _localMessageService;

        public MessageServiceTest(ILogger logger, IMessageService messageService, ITransactionService transactionService,
            ILocalMessageService localMessageService)
        {
            _logger = logger;
            _messageService = messageService;
            _transactionService = transactionService;
            _localMessageService = localMessageService;
        }

        public void RunAllTests()
        {
            SendUnencryptedMessage();
            SendUnencryptedData();
            SendEncryptedMessageByPublicKey();
            SendEncryptedMessageBySecretPhrase();
            SendEncryptedDataByPublicKey();
            SendEncryptedDataBySecretPhrase();
            SendAlreadyEncryptedMessage();
            SendEncryptedMessageToSelfByPublicKey();
            SendEncryptedMessageToSelfBySecretPhrase();
            SendPrunableMessage();
            SendEncryptedPrunableMessage();

            DecryptTextFrom();
            DecryptDataFrom();
            EncryptTextTo();
            EncryptDataTo();

            GetSharedKeyTest();
        }

        private void SendEncryptedPrunableMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase(false, Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsTrue(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsNull(actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertIsNotNull(actual.Data, nameof(actual.Data));
                AssertIsNotNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendPrunableMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(expected, true);
                var sendMessageResult = _messageService.SendMessage(parameters).Result;
                var actual = sendMessageResult.Transaction.Message;

                AssertEquals(expected, actual.MessageText, nameof(actual.MessageText));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsTrue(actual.IsPrunable, nameof(actual.IsPrunable));
            }
        }

        private void SendEncryptedMessageToSelfBySecretPhrase()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase(false, Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessageToSelf = new CreateTransactionParameters.MessageToBeEncryptedToSelf(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters).Result;
                var actual = sendMesageResult.Transaction.EncryptToSelfMessage;

                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsNull(actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNotNull(actual.Data, nameof(actual.Data));
                AssertIsNotNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendEncryptedMessageToSelfByPublicKey()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessageToSelf = new CreateTransactionParameters.MessageToBeEncryptedToSelf(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters).Result;
                var actual = sendMesageResult.Transaction.EncryptToSelfMessage;
                
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertEquals(expected, actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.Data, nameof(actual.Data));
                AssertIsNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendAlreadyEncryptedMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var nonce = _localMessageService.CreateNonce();
                var encrypted = _localMessageService.EncryptTextTo(TestSettings.Account2.PublicKey, "Hello World!", nonce, true, TestSettings.SecretPhrase1);

                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.AlreadyEncryptedMessage(encrypted, nonce, true, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsNull(actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertEquals(encrypted.ToHexString(), actual.Data.ToHexString(), nameof(actual.Data));
                AssertEquals(nonce.ToHexString(), actual.Nonce.ToHexString(), nameof(actual.Nonce));
            }
        }

        private void SendEncryptedMessageByPublicKey()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertEquals(expected, actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.Data, nameof(actual.Data));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertIsNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendEncryptedMessageBySecretPhrase()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase(false, Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsNull(actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertIsNotNull(actual.Data, nameof(actual.Data));
                AssertIsNotNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendEncryptedDataByPublicKey()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                byte[] bytes = {4, 7, 1, 64, 23, 91, 1, 45, 23};
                var expected = new BinaryHexString(bytes);
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(bytes, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsFalse(actual.IsText, nameof(actual.IsText));
                AssertEquals(expected.ToHexString(), actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.Data, nameof(actual.Data));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertIsNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendEncryptedDataBySecretPhrase()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                byte[] expected = { 4, 7, 1, 64, 23, 91, 1, 45, 23 };
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase(false, Amount.CreateAmountFromNxt(3));
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2.AccountRs).Result;
                var actual = sendMesageResult.Transaction.EncryptedMessage;

                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
                AssertIsTrue(actual.IsCompressed, nameof(actual.IsCompressed));
                AssertIsFalse(actual.IsText, nameof(actual.IsText));
                AssertIsNull(actual.MessageToEncrypt, nameof(actual.MessageToEncrypt));
                AssertIsNull(actual.EncryptedMessageHash, nameof(actual.EncryptedMessageHash));
                AssertIsNotNull(actual.Data, nameof(actual.Data));
                AssertIsNotNull(actual.Nonce, nameof(actual.Nonce));
            }
        }

        private void SendUnencryptedMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(expected);
                var sendMessageResult = _messageService.SendMessage(parameters).Result;
                var actual = sendMessageResult.Transaction.Message;

                AssertEquals(expected, actual.MessageText, nameof(actual.MessageText));
                AssertIsTrue(actual.IsText, nameof(actual.IsText));
                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
            }
        }

        private void SendUnencryptedData()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                byte[] expected = {4, 7, 1, 64, 23, 91, 1, 45, 23};
                var parameters = CreateTransaction.CreateTransactionByPublicKey(Amount.CreateAmountFromNxt(3));
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(expected);
                var sendMessageResult = _messageService.SendMessage(parameters).Result;
                var actual = sendMessageResult.Transaction.Message;

                AssertEquals(expected, actual.Data.ToBytes().ToArray(), nameof(actual.Data));
                AssertIsFalse(actual.IsText, nameof(actual.IsText));
                AssertIsFalse(actual.IsPrunable, nameof(actual.IsPrunable));
            }
        }

        private void EncryptDataTo()
        {
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            using (Logger = new TestsessionLogger(_logger))
            {
                var encrypted = _messageService.EncryptDataTo(TestSettings.Account2.AccountRs, expected, true, TestSettings.SecretPhrase1).Result;
                var decrypted = _messageService.DecryptDataFrom(TestSettings.Account1.AccountId, encrypted.Data, encrypted.Nonce, true, TestSettings.SecretPhrase2).Result;
                AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));
            }
        }

        private void EncryptTextTo()
        {
            const string expected = "Hello World!";

            using (Logger = new TestsessionLogger(_logger))
            {
                var encrypted = _messageService.EncryptTextTo(TestSettings.Account2.AccountRs, expected, true, TestSettings.SecretPhrase1).Result;
                var decrypted = _messageService.DecryptTextFrom(TestSettings.Account1.AccountId, encrypted.Data, encrypted.Nonce, true, TestSettings.SecretPhrase2).Result;
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
                var decrypted = _messageService.DecryptDataFrom(TestSettings.Account2.AccountRs, data, nonce, false, TestSettings.SecretPhrase1).Result;
                AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));

                var transaction = _transactionService.GetTransaction(TestSettings.SentDataMessageTransactionId).Result;
                decrypted = _messageService.DecryptDataFrom(TestSettings.Account2.AccountRs, transaction.EncryptedMessage.Data,
                    transaction.EncryptedMessage.Nonce, transaction.EncryptedMessage.IsCompressed, TestSettings.SecretPhrase1).Result;
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
                var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2.AccountRs, data, nonce, false, TestSettings.SecretPhrase1).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));

                var transaction = _transactionService.GetTransaction(TestSettings.SentTextMessageTransactionId).Result;
                decrypted = _messageService.DecryptTextFrom(TestSettings.Account2.AccountRs, transaction.EncryptedMessage.Data,
                    transaction.EncryptedMessage.Nonce, transaction.EncryptedMessage.IsCompressed, TestSettings.SecretPhrase1).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
            }
        }

        private void GetSharedKeyTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var nonce = new byte[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                    29, 30, 31, 32
                };
                const string secret = "123";
                var account = TestSettings.Account1;
                var sharedKeyResult = _messageService.GetSharedKey(account, secret, nonce).Result;

                AssertEquals("8d4632ddd2f5f75a09e0e64923921bb63489692528ee8224243402c2df93bb70", sharedKeyResult.SharedKey.ToHexString(), nameof(sharedKeyResult.SharedKey));
            }
        }
    }
}
