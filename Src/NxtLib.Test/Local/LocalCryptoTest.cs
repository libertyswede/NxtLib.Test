using Microsoft.Framework.Logging;
using NxtLib.Accounts;
using NxtLib.Local;
using NxtLib.Messages;
using System;
using System.Linq;

namespace NxtLib.Test.Local
{
    public interface ILocalCryptoTest : ITest
    {
    }

    public class LocalCryptoTest : TestBase, ILocalCryptoTest
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        private readonly ILocalCrypto _localCrypto;

        public LocalCryptoTest(ILogger logger, IMessageService messageService, ILocalCrypto localCrypto)
        {
            _logger = logger;
            _messageService = messageService;
            _localCrypto = localCrypto;
        }

        public void RunAllTests()
        {
            EncryptTextToTest();
            EncryptDataToTest();
            TestGenerateToken();
        }

        private void TestGenerateToken()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var token = _localCrypto.GenerateToken(TestSettings.SecretPhrase, "nxt.org");
            }
        }

        private void EncryptDataToTest()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var random = new Random();
                for (var i = 0; i < 100; i++)
                {
                    var expected = BuildBytes(random);
                    var nonce = _localCrypto.CreateNonce();
                    var compress = random.Next(0, 2) == 0;

                    var encrypted = _localCrypto.EncryptDataTo(TestSettings.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);
    
                    var decrypted = _messageService.DecryptDataFrom(TestSettings.Account2Rs, encrypted, nonce, compress, TestSettings.SecretPhrase).Result;
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
                    var expected = BuildString(random);
                    var nonce = _localCrypto.CreateNonce();
                    var compress = random.Next(0, 2) == 0;

                    var encrypted = _localCrypto.EncryptTextTo(TestSettings.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);
    
                    var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2Rs, encrypted, nonce, compress, TestSettings.SecretPhrase).Result;
                    AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
                }
            }
        }
        
        private byte[] BuildBytes(Random random)
        {
            var length = random.Next(10, 1000);
            var bytes = new byte[length];
            random.NextBytes(bytes);
            return bytes;
        }
        
        private string BuildString(Random random)
        {
            var length = random.Next(10, 1000);
            const string chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö0123456789+-*/_.,;:!""#¤%&()=?`´\}][{$£@§½|^¨~";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
