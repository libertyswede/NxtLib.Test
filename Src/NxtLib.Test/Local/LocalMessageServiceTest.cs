using System;
using NxtLib.Local;
using NxtLib.Messages;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace NxtLib.Test.Local
{
    public interface ILocalMessageServiceTest : ITest
    {
    }

    public class LocalMessageServiceTest : TestBase, ILocalMessageServiceTest
    {
        private readonly ILogger _logger;
        private readonly ILocalMessageService _localMessageService;
        private readonly IMessageService _messageService;

        public LocalMessageServiceTest(ILogger logger, ILocalMessageService localMessageService, IMessageService messageService)
        {
            _logger = logger;
            _localMessageService = localMessageService;
            _messageService = messageService;
        }

        public void RunAllTests()
        {
            EncryptDataToTest();
            EncryptTextToTest();
        }

        private void EncryptDataToTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var random = new Random();
                for (var i = 0; i < 100; i++)
                {
                    var expected = BuildRandomBytes(random);
                    var nonce = _localMessageService.CreateNonce();
                    var compress = random.Next(0, 2) == 0;

                    var encrypted = _localMessageService.EncryptDataTo(TestSettings.Account1.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);

                    var decrypted = _messageService.DecryptDataFrom(TestSettings.Account2.AccountRs, encrypted, nonce, compress, TestSettings.SecretPhrase1).Result;
                    AssertEquals(expected, decrypted.Data.ToBytes().ToArray(), nameof(decrypted.Data));
                }
            }
        }

        private void EncryptTextToTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var random = new Random();
                for (var i = 0; i < 100; i++)
                {
                    var expected = BuildRandomString(random);
                    var nonce = _localMessageService.CreateNonce();
                    var compress = random.Next(0, 2) == 0;

                    var encrypted = _localMessageService.EncryptTextTo(TestSettings.Account1.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);

                    var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2.AccountRs, encrypted, nonce, compress, TestSettings.SecretPhrase1).Result;
                    AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
                }
            }
        }
    }
}