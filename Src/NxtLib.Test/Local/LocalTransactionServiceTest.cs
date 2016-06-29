using System;
using Microsoft.Extensions.Logging;
using NxtLib.Accounts;
using NxtLib.Local;
using NxtLib.Messages;

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

        public LocalTransactionServiceTest(ILogger logger, ILocalTransactionService localTransactionService, 
            IAccountService accountService, IMessageService messageService)
        {
            _logger = logger;
            _localTransactionService = localTransactionService;
            _accountService = accountService;
            _messageService = messageService;
        }

        public void RunAllTests()
        {
            TestSendMessage();
            TestSendMoney();
        }

        private void TestSendMessage()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var localTransactionService = new LocalTransactionService();
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                parameters.Message = new CreateTransactionParameters.UnencryptedMessage("hello world!");
                var sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);

                parameters.Message = new CreateTransactionParameters.UnencryptedMessage(new byte[] { 1, 2, 3, 4, 5, 6, 7 });
                sendMessageReply = _messageService.SendMessage(parameters, null).Result;
                localTransactionService.VerifySendMessageTransactionBytes(sendMessageReply, parameters, null);
                //var signed = _localTransactionService.SignTransaction(sendMessageReply, TestSettings.SecretPhrase1);
            }
        }

        private void TestSendMoney()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var localTransactionService = new LocalTransactionService();
                var parameters = new CreateTransactionByPublicKey(1440, Amount.OneNxt, TestSettings.Account1.PublicKey);
                var sendMoneyReply = _accountService.SendMoney(parameters, TestSettings.Account2, Amount.OneNxt).Result;
                localTransactionService.VerifySendMoneyTransactionBytes(sendMoneyReply, parameters, TestSettings.Account2, Amount.OneNxt);
                //var signed = _localTransactionService.SignTransaction(sendMoneyReply, TestSettings.SecretPhrase1);
            }
        }
    }
}
