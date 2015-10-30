using System;
using Microsoft.Framework.Logging;
using NxtLib.Messages;

namespace NxtLib.Test.Messages
{
    public interface IMessageServiceTest : ITest
    {
    }

    public class MessageServiceTest : TestBase, IMessageServiceTest
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;

        public MessageServiceTest(ILogger logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public void RunAllTests()
        {
            const string data = "32627f503c339c643a0dc6ef211b3291127f0767afc306afd3f634feab517c9f";
            const string nonce = "d000d04c219828caee765c5420c2de1cf0827c3b5299425da6798bc18da7adfa";
            const string expected = "Hello World!";

            using (Logger = new TestsessionLogger(_logger))
            {
                var decrypted = _messageService.DecryptFrom(TestSettings.Account2Rs, data, true, nonce, false, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.Data, nameof(decrypted.Data); // TODO: Fix!
            }
        }
    }
}
