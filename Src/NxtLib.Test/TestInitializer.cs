﻿using System.Collections.Generic;
using Microsoft.Framework.Logging;
using NxtLib.VotingSystem;
using NxtLib.Local;

namespace NxtLib.Test
{
    public interface ITestInitializer
    {
        void InitializeTest();
    }

    public class TestInitializer : ITestInitializer
    {
        private readonly ILogger _logger;
        private readonly IServiceFactory _serviceFactory;

        public TestInitializer(IServiceFactory serviceFactory, ILogger logger)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
        }

        public void InitializeTest()
        {
            _logger.LogInformation("Fetching number of blocks");
            TestSettings.MaxHeight = GetCurrentHeight();
            _logger.LogInformation($"Current height: {TestSettings.MaxHeight}");
            _logger.LogInformation("Setting account properties");
            GetAccountProperties();
            if (TestSettings.RunCostlyTests)
            {
                _logger.LogInformation("Creating poll");
                CreatePoll();
            }
        }

        private void CreatePoll()
        {
            var votingSystemServicer = _serviceFactory.CreateVotingSystemService();
            var createPollParameter = new CreatePollParameters("testpoll", "test poll", TestSettings.MaxHeight + 500,
                VotingModel.Asset, 1, 1, 0, 1, new List<string> {"how are you doing?"})
            {
                MinBalanceModel = MinBalanceModel.Asset,
                MinBalance = 1,
                HoldingId = TestSettings.ExistingAssetId
            };
            var createPoll = votingSystemServicer.CreatePoll(createPollParameter,
                CreateTransaction.CreateTransactionBySecretPhrase(true, Amount.CreateAmountFromNxt(10))).Result;
            if (createPoll.TransactionId != null) 
                TestSettings.PollId = createPoll.TransactionId.Value;
        }

        private void GetAccountProperties()
        {
            var localCrypto = new LocalCrypto();
            TestSettings.PublicKey = localCrypto.GetPublicKey(TestSettings.SecretPhrase);
            TestSettings.AccountId = localCrypto.GetAccountIdFromPublicKey(TestSettings.PublicKey);
            TestSettings.AccountRs = localCrypto.GetReedSolomonFromAccountId(TestSettings.AccountId);
        }

        private int GetCurrentHeight()
        {
            var serverInfoService = _serviceFactory.CreateServerInfoService();
            var blockchainStatus = serverInfoService.GetBlockchainStatus().Result;
            return blockchainStatus.NumberOfBlocks;
        }
    }
}
