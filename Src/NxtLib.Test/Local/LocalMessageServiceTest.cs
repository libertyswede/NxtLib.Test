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

            GetSharedSecretTest();
        }

        private void EncryptDataToTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var random = new Random();
                for (var i = 0; i < 100000; i++)
                {
                    Console.WriteLine($"{i}");
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
                for (var i = 0; i < 100000; i++)
                {
                    Console.WriteLine($"{i}");
                    var expected = BuildRandomString(random);
                    var nonce = _localMessageService.CreateNonce();
                    var compress = random.Next(0, 2) == 0;

                    var encrypted = _localMessageService.EncryptTextTo(TestSettings.Account1.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);

                    var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2.AccountRs, encrypted, nonce, compress, TestSettings.SecretPhrase1).Result;
                    AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
                }
            }
        }

        private void GetSharedSecretTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var nonce = new byte[]
                {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                    29, 30, 31, 32
                };
                const string secret = "123";
                var account = TestSettings.Account1.PublicKey;
                var sharedKey = _localMessageService.GetSharedKey(account, nonce, secret);

                AssertEquals("8d4632ddd2f5f75a09e0e64923921bb63489692528ee8224243402c2df93bb70", sharedKey.ToHexString(), nameof(sharedKey));
            }
        }
    }
}