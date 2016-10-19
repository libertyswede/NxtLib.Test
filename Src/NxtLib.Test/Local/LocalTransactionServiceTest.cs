using Microsoft.Extensions.Logging;
using NxtLib.Accounts;
using NxtLib.Local;
using NxtLib.Messages;
using static NxtLib.CreateTransactionParameters;
using NxtLib.VotingSystem;
using NxtLib.MonetarySystem;
using NxtLib.AssetExchange;

namespace NxtLib.Test.Local
{
    public interface ILocalTransactionServiceTest : ITest
    {
    }

    public class LocalTransactionServiceTest : TestBase, ILocalTransactionServiceTest
    {
        private readonly ILogger _logger;
        private readonly ILocalTransactionService _localTransactionService;
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private readonly IMonetarySystemService _monetarySystemService;
        private readonly IAssetExchangeService _assetExchangeService;

        public LocalTransactionServiceTest(ILogger logger, ILocalTransactionService localTransactionService, 
            IAccountService accountService, IMessageService messageService, IMonetarySystemService monetarySystemService,
            IAssetExchangeService assetExchangeService)
        {
            _logger = logger;
            _localTransactionService = localTransactionService;
            _accountService = accountService;
            _messageService = messageService;
            _monetarySystemService = monetarySystemService;
            _assetExchangeService = assetExchangeService;
        }

        public void RunAllTests()
        {
            TestTransferAsset();
            TestTransferCurrency();
            TestPhasing();
            TestSendEncryptedMessageToSelf();
            TestPublicKeyAnnouncement();
            TestSendEncryptedMessage();
            TestSendMessage();
            TestSendMoney();
        }

        private void TestPhasing()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.CreateAmountFromNxt(2), TestSettings.Account1.PublicKey);
                parameters.Phasing = new CreateTransactionPhasing(TestSettings.MaxHeight + 100, VotingModel.Account, 1);
                parameters.Phasing.WhiteListed.Add(TestSettings.Account2);
                var sendMoneyReply = _accountService.SendMoney(parameters, TestSettings.Account2, Amount.OneNxt).Result;
                _localTransactionService.VerifySendMoneyTransactionBytes(sendMoneyReply, parameters, TestSettings.Account2, Amount.OneNxt);

                parameters = new CreateTransactionByPublicKey(1440, Amount.CreateAmountFromNxt(21), TestSettings.Account1.PublicKey);
                parameters.Phasing = new CreateTransactionPhasing(TestSettings.MaxHeight + 100, VotingModel.Account, 100);
                parameters.Phasing.MinBalanceModel = MinBalanceModel.Nqt;
                parameters.Phasing.MinBalance = Amount.CreateAmountFromNxt(100).Nqt;
                sendMoneyReply = _accountService.SendMoney(parameters, TestSettings.Account2, Amount.OneNxt).Result;
                _localTransactionService.VerifySendMoneyTransactionBytes(sendMoneyReply, parameters, TestSettings.Account2, Amount.OneNxt);
            }
        }

        private void TestSendEncryptedMessageToSelf()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.CreateAmountFromNxt(3), TestSettings.Account1.PublicKey);
                parameters.EncryptedMessageToSelf = new AlreadyEncryptedMessageToSelf("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", true, true);
                var sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);

                parameters.EncryptedMessageToSelf = new AlreadyEncryptedMessageToSelf("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", false, false);
                sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);
            }
        }

        private void TestPublicKeyAnnouncement()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string account = "NXT-UJN3-RRHW-XPAC-2BTM8";
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                parameters.RecipientPublicKey = "9948dc7c342b685d41d8104c8fa66b9ae656078f84d8a08e317114165d71fc01";
                var sendMoneyReply = _accountService.SendMoney(parameters, account, Amount.OneNxt).Result;
                _localTransactionService.VerifySendMoneyTransactionBytes(sendMoneyReply, parameters, account, Amount.OneNxt);
            }
        }

        private void TestSendEncryptedMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.CreateAmountFromNxt(3), TestSettings.Account1.PublicKey);
                parameters.EncryptedMessage = new AlreadyEncryptedMessage("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", true, true);
                var sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);

                parameters.EncryptedMessage = new AlreadyEncryptedMessage("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", false, false);
                sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);

                parameters.EncryptedMessage = new AlreadyEncryptedMessage("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", true, true, true);
                sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);

                parameters.EncryptedMessage = new AlreadyEncryptedMessage("01020304050607080910", "0102030405060708091011121314151617181920212223242526272829303132", false, false, true);
                sendMessageReply = _messageService.SendMessage(parameters, TestSettings.Account2).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, TestSettings.Account2);
            }
        }

        private void TestSendMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage("hello world!");
                var sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);

                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(new byte[] { 1, 2, 3, 4, 5, 6, 7 });
                sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);
                
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage("hello world!", true);
                sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);

                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(new byte[] { 1, 2, 3, 4, 5, 6, 7 }, true);
                sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                _localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);
            }
        }

        private void TestSendMoney()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                var sendMoneyReply = _accountService.SendMoney(parameters, TestSettings.Account2, Amount.OneNxt).Result;
                _localTransactionService.VerifySendMoneyTransactionBytes(sendMoneyReply, parameters, TestSettings.Account2, Amount.OneNxt);
            }
        }

        private void TestTransferCurrency()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                var transferCurrencyReply = _monetarySystemService.TransferCurrency(TestSettings.Account2, TestSettings.ExistingCurrencyId, 10, parameters).Result;
                _localTransactionService.VerifyTransferCurrencyTransactionBytes(transferCurrencyReply, parameters, TestSettings.Account2, TestSettings.ExistingCurrencyId, 10);
            }
        }

        private void TestTransferAsset()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                var transferAssetReply = _assetExchangeService.TransferAsset(TestSettings.Account2, TestSettings.ExistingAssetId, 100, parameters).Result;
                _localTransactionService.VerifyTransferAssetTransactionBytes(transferAssetReply, parameters, TestSettings.Account2, TestSettings.ExistingAssetId, 100);
            }
        }
    }
}
