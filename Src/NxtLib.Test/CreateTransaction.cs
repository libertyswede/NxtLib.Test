using NxtLib.Accounts;
using NxtLib.Local;

namespace NxtLib.Test
{
    public static class CreateTransaction
    {
        private static readonly string SecretPhrase;
        private static readonly AccountWithPublicKey Account;

        static CreateTransaction()
        {
            var localCrypto = new LocalAccountService();
            SecretPhrase = TestSettings.SecretPhrase1;
            Account = localCrypto.GetAccount(AccountIdLocator.BySecretPhrase(SecretPhrase));
        }

        public static CreateTransactionByPublicKey CreateTransactionByPublicKey(Amount fee = null)
        {
            if (fee == null)
            {
                fee = Amount.OneNxt;
            }
            return new CreateTransactionByPublicKey(1440, fee, Account.PublicKey);
        }

        public static CreateTransactionBySecretPhrase CreateTransactionBySecretPhrase(bool broadcast = false, Amount fee = null)
        {
            if (fee == null)
            {
                fee = Amount.OneNxt;
            }
            return new CreateTransactionBySecretPhrase(broadcast, 1440, fee, SecretPhrase);
        }
    }
}
