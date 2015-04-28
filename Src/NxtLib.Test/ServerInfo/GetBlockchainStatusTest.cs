using System;
using System.Text.RegularExpressions;
using NLog;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    internal class GetBlockchainStatusTest
    {
        private readonly GetBlockchainStatusReply _getBlockchainStatusReply;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GetBlockchainStatusTest(IServerInfoService serverInfoService)
        {
            _getBlockchainStatusReply = serverInfoService.GetBlockchainStatus().Result;
        }

        public void RunAllTests()
        {
            RunAllTests(_getBlockchainStatusReply);
        }

        public void RunAllTests(BlockchainStatus blockchainStatus)
        {
            if (!blockchainStatus.Application.Equals("NRS"))
            {
                Logger.Error("Unexpected Application, expected: NRS, actual: {0}", blockchainStatus.Application);
            }
            if (blockchainStatus.CumulativeDifficulty == 0)
            {
                Logger.Error("Unexpected CumulativeDifficulty, expected > 0, actual: 0");
            }
            if (blockchainStatus.CurrentMinRollbackHeight <= 0)
            {
                Logger.Error("Unexpected CurrentMinRollbackHeight, expected > 0, actual: {0}", blockchainStatus.CurrentMinRollbackHeight);
            }
            // blockchainStatus.IncludeExpiredPrunable
            // blockchainStatus.IsScanning
            // blockchainStatus.IsTestnet
            if (blockchainStatus.LastBlockId == 0)
            {
                Logger.Error("Unexpected LastBlockId, expected > 0, actual: 0");
            }
            if (string.IsNullOrEmpty(blockchainStatus.LastBlockchainFeeder))
            {
                Logger.Error("Unexpected LastBlockchainFeeded, expected: something, actual: nothing");
            }
            if (blockchainStatus.LastBlockchainFeederHeight <= 0)
            {
                Logger.Error("Unexpected LastBlockchainFeederHeight, expected > 0, actual: {0}", blockchainStatus.LastBlockchainFeederHeight);
            }
            if (blockchainStatus.MaxPrunableLifetime != 86400)
            {
                Logger.Error("Unexpected MaxPrunableLifetime, expected: 86400, actual: {0}", blockchainStatus.MaxPrunableLifetime);
            }
            if (blockchainStatus.MaxRollback != 800)
            {
                Logger.Error("Unexpected MaxRollback, expected: 800, actual: {0}", blockchainStatus.MaxRollback);
            }
            if (blockchainStatus.NumberOfBlocks <= 0)
            {
                Logger.Error("Unexpected NumberOfBlocks, expected > 0, actual: {0}");
            }
            if (Math.Abs(blockchainStatus.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Error("Unexpected Time, expected to be within 10 seconds ({0}), actual: {1}",
                    DateTime.UtcNow.ToLongTimeString(), blockchainStatus.Time.ToLongTimeString());
            }
            if (!Regex.Match(blockchainStatus.Version, "([0-9]+)\\.([0-9]+)\\.([0-9]+)(\\.([0-9]+))?(e?)").Success)
            {
                Logger.Error("Unexpected version format, expected: x.x.x(.x)(e), actual: {0}", blockchainStatus.Version);
            }
        }
    }
}