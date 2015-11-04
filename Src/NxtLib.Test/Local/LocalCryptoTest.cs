using Microsoft.Framework.Logging;
using NxtLib.Accounts;
using NxtLib.Local;
using NxtLib.Messages;

namespace NxtLib.Test.Local
{
    public interface ILocalCryptoTest : ITest
    {
    }

    public class LocalCryptoTest : TestBase, ILocalCryptoTest
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;

        public LocalCryptoTest(ILogger logger, IMessageService messageService, IAccountService accountService)
        {
            _logger = logger;
            _messageService = messageService;
            _accountService = accountService;
        }

        public void RunAllTests()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                const string expected = "Hello World!";
                var account = _accountService.GetAccount(TestSettings.AccountId).Result;
                var localCrypto = new LocalCrypto();
                var nonce = localCrypto.CreateNonce();

                var compress = true;

                var encrypted = localCrypto.EncryptTextTo(account.PublicKey, expected, nonce, compress, TestSettings.SecretPhrase2);

                var decrypted = _messageService.DecryptTextFrom(TestSettings.Account2Rs, encrypted, nonce, compress, TestSettings.SecretPhrase).Result;
                AssertEquals(expected, decrypted.DecryptedMessage, nameof(decrypted.DecryptedMessage));
            }
        }
    }
}
