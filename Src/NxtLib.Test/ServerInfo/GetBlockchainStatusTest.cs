using System;
using System.Text.RegularExpressions;
using NLog;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    internal class GetBlockchainStatusTest : TestBase
    {
        private readonly GetBlockchainStatusReply _getBlockchainStatusReply;

        public GetBlockchainStatusTest(IServerInfoService serverInfoService)
        {
            _getBlockchainStatusReply = serverInfoService.GetBlockchainStatus().Result;
        }

        public void Test()
        {
           Test(_getBlockchainStatusReply);
        }

        public void Test(BlockchainStatus blockchainStatus)
        {
            using (Logger = new TestsessionLogger())
            {
                AssertEquals("NRS", blockchainStatus.Application, "Application");
                AssertIsLargerThanZero(blockchainStatus.CumulativeDifficulty, "CumulativeDifficulty");
                AssertIsLargerThanZero(blockchainStatus.CurrentMinRollbackHeight, "CurrentMinRollbackHeight");
                // blockchainStatus.IncludeExpiredPrunable
                // blockchainStatus.IsScanning
                // blockchainStatus.IsTestnet
                AssertIsLargerThanZero(blockchainStatus.LastBlockId, "LastBlockId");
                AssertIsNullOrEmpty(blockchainStatus.LastBlockchainFeeder, "LastBlockchainFeeder");
                AssertIsLargerThanZero(blockchainStatus.LastBlockchainFeederHeight, "LastBlockchainFeederHeight");
                AssertEquals(86400, blockchainStatus.MaxPrunableLifetime, "MaxPrunableLifetime");
                AssertEquals(800, blockchainStatus.MaxRollback, "MaxRollback");
                AssertIsLargerThanZero(blockchainStatus.NumberOfBlocks, "NumberOfBlocks");
                CheckTimeShouldBeAroundNow(blockchainStatus);
                CheckVersionFormat(blockchainStatus);
            }
        }

        private static void CheckVersionFormat(BlockchainStatus blockchainStatus)
        {
            if (!Regex.Match(blockchainStatus.Version, "([0-9]+)\\.([0-9]+)\\.([0-9]+)(\\.([0-9]+))?(e?)").Success)
            {
                Logger.Fail(string.Format("Unexpected version format, expected: x.x.x(.x)(e), actual: {0}",
                    blockchainStatus.Version));
            }
        }

        private static void CheckTimeShouldBeAroundNow(BlockchainStatus blockchainStatus)
        {
            if (Math.Abs(blockchainStatus.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Fail(string.Format("Unexpected Time, expected to be within 10 seconds ({0}), actual: {1}",
                    DateTime.UtcNow.ToLongTimeString(), blockchainStatus.Time.ToLongTimeString()));
            }
        }
    }
}