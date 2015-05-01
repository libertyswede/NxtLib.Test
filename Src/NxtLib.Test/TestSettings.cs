using System;
using System.Configuration;

namespace NxtLib.Test
{
    public static class TestSettings
    {
        public static ulong ExistingCurrencyId
        {
            get { return Convert.ToUInt64(ConfigurationManager.AppSettings["ExistingCurrencyId"]); }
        }

        public static int MaxHeight { get; set; }

        public static string NxtServerUrl
        {
            get { return ConfigurationManager.AppSettings["NxtServerUrl"]; }
        }

        public static IServiceFactory ServiceFactory { get; set; }
    }
}
