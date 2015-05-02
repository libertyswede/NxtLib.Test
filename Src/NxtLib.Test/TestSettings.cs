using System;
using System.Configuration;

namespace NxtLib.Test
{
    public static class TestSettings
    {
        public static ulong AccountId { get; set; }
        public static string AccountRs { get; set; }

        public static ulong ExistingAssetId
        {
            get { return Convert.ToUInt64(ConfigurationManager.AppSettings["ExistingAssetId"]); }
        }
        

        public static string Account2Rs
        {
            get { return ConfigurationManager.AppSettings["Account2Rs"]; }
        }

        public static ulong ExistingCurrencyId
        {
            get { return Convert.ToUInt64(ConfigurationManager.AppSettings["ExistingCurrencyId"]); }
        }

        public static int MaxHeight { get; set; }

        public static string NxtServerUrl
        {
            get { return ConfigurationManager.AppSettings["NxtServerUrl"]; }
        }

        public static string PublicKey { get; set; }

        public static string SecretPhrase
        {
            get { return ConfigurationManager.AppSettings["SecretPhrase"]; }
        }

        public static IServiceFactory ServiceFactory { get; set; }
    }
}
