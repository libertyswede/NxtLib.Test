using System;
using Microsoft.Extensions.Logging;
using NxtLib.Accounts;
using NxtLib.Local;
using NxtLib.Tokens;

namespace NxtLib.Test.Local
{
    public interface ILocalTokenServiceTest : ITest
    {
    }

    public class LocalTokenServiceTest : TestBase, ILocalTokenServiceTest
    {
        private readonly ILogger _logger;
        private readonly ILocalTokenService _localTokenService;
        private readonly ITokenService _tokenService;
        private readonly ILocalAccountService _localAccountService;

        public LocalTokenServiceTest(ILogger logger, ILocalTokenService localTokenService, ITokenService tokenService,
            ILocalAccountService localAccountService)
        {
            _logger = logger;
            _localTokenService = localTokenService;
            _tokenService = tokenService;
            _localAccountService = localAccountService;
        }

        public void RunAllTests()
        {
            TestDecodeToken();
            TestGenerateToken();
        }

        private void TestDecodeToken()
        {
            var random = new Random();
            using (Logger = new TestsessionLogger(_logger))
            {
                for (var i = 0; i < 100; i++)
                {
                    var expected = BuildRandomString(random);
                    var token = _localTokenService.GenerateToken(TestSettings.SecretPhrase1, expected);

                    var decodedToken = _localTokenService.DecodeToken(expected, token.Token);
                    var serviceDecodedToken = _tokenService.DecodeToken(expected, token.Token).Result;

                    var account = _localAccountService.GetAccount(AccountIdLocator.ByPublicKey(decodedToken.PublicKey));

                    AssertEquals(token.Timestamp, decodedToken.Timestamp, nameof(decodedToken.Timestamp));
                    AssertEquals(serviceDecodedToken.Account, account.AccountId, nameof(decodedToken.PublicKey));
                    AssertEquals(serviceDecodedToken.AccountRs, account.AccountRs, nameof(decodedToken.PublicKey));
                    AssertEquals(TestSettings.Account1.PublicKey.ToHexString(), decodedToken.PublicKey.ToHexString(), nameof(decodedToken.PublicKey));
                    AssertIsTrue(decodedToken.Valid, nameof(decodedToken.Valid));
                }
            }
        }

        private void TestGenerateToken()
        {
            var random = new Random();
            using (Logger = new TestsessionLogger(_logger))
            {
                for (var i = 0; i < 100; i++)
                {
                    var message = BuildRandomString(random);

                    var expected = _tokenService.GenerateToken(TestSettings.SecretPhrase1, message).Result;
                    var actual = _localTokenService.GenerateToken(TestSettings.SecretPhrase1, message, expected.Timestamp);

                    AssertEquals(expected.Token, actual.Token, nameof(actual.Token));
                    AssertEquals(expected.Timestamp, actual.Timestamp, nameof(actual.Timestamp));
                    AssertEquals(expected.Account, actual.Account.AccountId, nameof(actual.Account.AccountId));
                    AssertEquals(expected.AccountRs, actual.Account.AccountRs, nameof(actual.Account.AccountRs));
                    AssertEquals(expected.Valid, actual.Valid, nameof(actual.Valid));
                }
            }
        }
    }
}