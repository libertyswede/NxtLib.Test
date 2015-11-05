﻿using Microsoft.Framework.Logging;
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
        private readonly ILocalCrypto _localCrypto;

        public MessageServiceTest(ILogger logger, IMessageService messageService, ITransactionService transactionService, ILocalCrypto localCrypto)
        {
            _logger = logger;
            _messageService = messageService;
            _transactionService = transactionService;
            _localCrypto = localCrypto;
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
        }

        private void SendEncryptedPrunableMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase();
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
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
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase();
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
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
                var nonce = _localCrypto.CreateNonce();
                var recipientPublicKey = _localCrypto.GetPublicKey(TestSettings.SecretPhrase2);
                var encrypted = _localCrypto.EncryptTextTo(recipientPublicKey, "Hello World!", nonce, true, TestSettings.SecretPhrase);

                var parameters = CreateTransaction.CreateTransactionByPublicKey();
                parameters.EncryptedMessage = new CreateTransactionParameters.AlreadyEncryptedMessage(encrypted, nonce, true, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase();
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(bytes, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionBySecretPhrase();
                parameters.EncryptedMessage = new CreateTransactionParameters.MessageToBeEncrypted(expected, true);
                var sendMesageResult = _messageService.SendMessage(parameters, TestSettings.Account2Rs).Result;
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
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
                var parameters = CreateTransaction.CreateTransactionByPublicKey();
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
