using System;

namespace NxtLib.Test
{
    public static class TestSettings
    {
        public static string NxtServerUrl => Program.AppSettings["NxtServerUrl"];
        public static bool RunCostlyTests => Convert.ToBoolean(Program.AppSettings["RunCostlyTests"]);
        public static AccountWithPublicKey Account1 { get; set; }
        public static AccountWithPublicKey Account2 { get; set; }
        public static string SecretPhrase1 => Program.AppSettings["SecretPhrase1"];
        public static string SecretPhrase2 => Program.AppSettings["SecretPhrase2"];
        public static ulong ExistingAssetId => Convert.ToUInt64(Program.AppSettings["ExistingAssetId"]);
        public static ulong ExistingCurrencyId => Convert.ToUInt64(Program.AppSettings["ExistingCurrencyId"]);
        public static int MaxHeight { get; set; }
        public static ulong PollId { get; set; }
        public static ulong TaggedDataTransactionId => Convert.ToUInt64(Program.AppSettings["TaggedDataTransactionId"]);
        public static ulong SentTextMessageTransactionId => Convert.ToUInt64(Program.AppSettings["SentTextMessageTransactionId"]);
        public static ulong SentDataMessageTransactionId => Convert.ToUInt64(Program.AppSettings["SentDataMessageTransactionId"]);
    }
}
