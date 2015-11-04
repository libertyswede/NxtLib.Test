using System;

namespace NxtLib.Test
{
    public static class TestSettings
    {
        public static ulong AccountId { get; set; }
        public static string AccountRs { get; set; }
        public static ulong ExistingAssetId => Convert.ToUInt64(Program.AppSettings["ExistingAssetId"]);
        public static string Account2Rs => Program.AppSettings["Account2Rs"];
        public static ulong ExistingCurrencyId => Convert.ToUInt64(Program.AppSettings["ExistingCurrencyId"]);
        public static int MaxHeight { get; set; }
        public static string NxtServerUrl => Program.AppSettings["NxtServerUrl"];
        public static ulong PollId { get; set; }
        public static BinaryHexString PublicKey { get; set; }
        public static bool RunCostlyTests => Convert.ToBoolean(Program.AppSettings["RunCostlyTests"]);
        public static string SecretPhrase => Program.AppSettings["SecretPhrase"];
        public static string SecretPhrase2 => Program.AppSettings["SecretPhrase2"];
        public static ulong TaggedDataTransactionId => Convert.ToUInt64(Program.AppSettings["TaggedDataTransactionId"]);
        public static ulong SentTextMessageTransactionId => Convert.ToUInt64(Program.AppSettings["SentTextMessageTransactionId"]);
        public static ulong SentDataMessageTransactionId => Convert.ToUInt64(Program.AppSettings["SentDataMessageTransactionId"]);
    }
}
