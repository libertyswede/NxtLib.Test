using System.Collections.Generic;
using NLog;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    internal class CreatePollTest : TestBase
    {
        private readonly IVotingSystemService _votingSystemService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CreatePollTest(IVotingSystemService votingSystemService)
        {
            _votingSystemService = votingSystemService;
        }

        public void Test()
        {
            Logger.Info("Starting CreatePoll test");
            CreatePollByCurrency();
            CreatePollByNqt();
            CreatePollByAsset();
        }

        private void CreatePollByCurrency()
        {
            const VotingModel votingModel = VotingModel.Account;
            const MinBalanceModel balanceModel = MinBalanceModel.Currency;
            var currencyId = TestSettings.ExistingCurrencyId;

            CreatePoll(votingModel, balanceModel, currencyId);
        }

        private void CreatePollByNqt()
        {
            const VotingModel votingModel = VotingModel.Nqt;
            const MinBalanceModel balanceModel = MinBalanceModel.Nqt;

            CreatePoll(votingModel, balanceModel, null);
        }

        private void CreatePollByAsset()
        {
            const VotingModel votingModel = VotingModel.Asset;
            const MinBalanceModel balanceModel = MinBalanceModel.Asset;
            var assetId = TestSettings.ExistingAssetId;

            CreatePoll(votingModel, balanceModel, assetId);
        }

        private void CreatePoll(VotingModel votingModel, MinBalanceModel balanceModel, ulong? holdingId)
        {
            var name = Utils.GenerateRandomString(10);
            var description = Utils.GenerateRandomString(30);
            var finishHeight = TestSettings.MaxHeight + 1000;
            const int minNumberOfOptions = 1;
            const int maxNumberOfOptions = 1;
            const int minRangeValue = 0;
            const int maxRangeValue = 1;
            var options = new List<string> { "How are you doing?" };
            const int minBalance = 1;

            var createPollParameters = new CreatePollParameters(name, description, finishHeight, votingModel,
                minNumberOfOptions, maxNumberOfOptions, minRangeValue, maxRangeValue, options)
            {
                MinBalance = minBalance,
                MinBalanceModel = balanceModel,
                HoldingId = holdingId
            };

            var createPollReply = _votingSystemService.CreatePoll(createPollParameters,
                CreateTransaction.CreateTransactionBySecretPhrase(fee: Amount.CreateAmountFromNxt(10))).Result;

            VerifyCreatePollParameters(createPollParameters, createPollReply.Transaction.Attachment as MessagingPollCreationAttachment);
        }

        private static void VerifyCreatePollParameters(CreatePollParameters createPollParameters, MessagingPollCreationAttachment pollCreation)
        {
            Compare(createPollParameters.Name, pollCreation.Name, "poll Name");
            Compare(createPollParameters.Description, pollCreation.Description, "poll Description");
            Compare(createPollParameters.FinishHeight, pollCreation.FinishHeight, "poll FinishHeight");
            Compare(createPollParameters.VotingModel, pollCreation.VotingModel, "poll VotingModel");
            Compare(createPollParameters.MinNumberOfOptions, pollCreation.MinNumberOfOptions, "poll MinNumberOfOptions");
            Compare(createPollParameters.MaxNumberOfOptions, pollCreation.MaxNumberOfOptions, "poll MaxNumberOfOptions");
            Compare(createPollParameters.MinRangeValue, pollCreation.MinRangeValue, "poll MinRangeValue");
            Compare(createPollParameters.MaxRangeValue, pollCreation.MaxRangeValue, "poll MaxRangeValue");
            Compare(createPollParameters.Options, pollCreation.Options, "poll Options");
            Compare(createPollParameters.MinBalance, pollCreation.MinBalance, "poll MinRangeValue");
            Compare(createPollParameters.MinBalanceModel, pollCreation.MinBalanceModel, "poll MinBalanceModel");
            Compare(createPollParameters.HoldingId, pollCreation.HoldingId, "poll HoldingId");
        }
    }
}