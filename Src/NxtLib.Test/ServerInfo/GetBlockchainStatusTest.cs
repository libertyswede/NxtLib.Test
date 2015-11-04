using System;
using System.Text.RegularExpressions;
using Microsoft.Framework.Logging;
using NxtLib.ServerInfo;

namespace NxtLib.Test.ServerInfo
{
    public interface IGetBlockchainStatusTest
    {
        void Test();
        void Test(BlockchainStatus blockchainStatus);
    }

    internal class GetBlockchainStatusTest : TestBase, IGetBlockchainStatusTest
    {
        private readonly ILogger _logger;
        private readonly GetBlockchainStatusReply _getBlockchainStatusReply;

        public GetBlockchainStatusTest(IServerInfoService serverInfoService, ILogger logger)
        {
            _logger = logger;
            _getBlockchainStatusReply = serverInfoService.GetBlockchainStatus().Result;
        }

        public void Test()
        {
           Test(_getBlockchainStatusReply);
        }

        public void Test(BlockchainStatus blockchainStatus)
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                AssertEquals("NRS", blockchainStatus.Application, "Application");
                AssertIsLargerThanZero(blockchainStatus.CumulativeDifficulty, "CumulativeDifficulty");
                AssertIsLargerThanZero(blockchainStatus.CurrentMinRollbackHeight, "CurrentMinRollbackHeight");
                // blockchainStatus.IncludeExpiredPrunable
                // blockchainStatus.IsDownloading
                // blockchainStatus.IsScanning
                // blockchainStatus.IsTestnet
                AssertIsLargerThanZero(blockchainStatus.LastBlockId, "LastBlockId");
                //AssertIsNullOrEmpty(blockchainStatus.LastBlockchainFeeder, "LastBlockchainFeeder");
                AssertIsLargerThanZero(blockchainStatus.LastBlockchainFeederHeight, "LastBlockchainFeederHeight");
                //AssertEquals(86400, blockchainStatus.MaxPrunableLifetime, "MaxPrunableLifetime", true);
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
                Logger.Fail($"Unexpected version format, expected: x.x.x(.x)(e), actual: {blockchainStatus.Version}");
            }
        }

        private static void CheckTimeShouldBeAroundNow(BlockchainStatus blockchainStatus)
        {
            if (Math.Abs(blockchainStatus.Time.Subtract(DateTime.UtcNow).TotalSeconds) > 10)
            {
                Logger.Fail($"Unexpected Time, expected to be within 10 seconds ({DateTime.UtcNow.ToString("T")}), actual: {blockchainStatus.Time.ToString("T")}");
            }
        }
    }
}