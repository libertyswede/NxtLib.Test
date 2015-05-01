using System.Configuration;
using NxtLib.Local;

namespace NxtLib.Test
{
    public static class CreateTransaction
    {
        private static readonly string SecretPhrase;
        private static readonly BinaryHexString PublicKey;
        private static readonly ulong AccountId;

        static CreateTransaction()
        {
            var localCrypto = new LocalCrypto();
            SecretPhrase = ConfigurationManager.AppSettings["SecretPhrase"];
            PublicKey = localCrypto.GetPublicKey(SecretPhrase);
            AccountId = localCrypto.GetAccountIdFromPublicKey(PublicKey);
        }

        public static CreateTransactionByPublicKey CreateTransactionByPublicKey()
        {
            return new CreateTransactionByPublicKey(1440, Amount.OneNxt, PublicKey);
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
