using System.Collections.Generic;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    internal class CreatePollTest : TestBase
    {
        private readonly IVotingSystemService _votingSystemService;

        public CreatePollTest()
        {
            _votingSystemService = TestSettings.ServiceFactory.CreateVotingSystemService();
        }

        public void Test()
        {
            CreatePollByCurrency();
            CreatePollByNqt();
            CreatePollByAsset();
        }

        private void CreatePollByCurrency()
        {
            using (Logger = new TestsessionLogger())
            {
                const VotingModel votingModel = VotingModel.Account;
                const MinBalanceModel balanceModel = MinBalanceModel.Currency;
                var currencyId = TestSettings.ExistingCurrencyId;

                CreatePoll(votingModel, balanceModel, currencyId);
            }
        }

        private void CreatePollByNqt()
        {
            using (Logger = new TestsessionLogger())
            {
                const VotingModel votingModel = VotingModel.Nqt;
                const MinBalanceModel balanceModel = MinBalanceModel.Nqt;

                CreatePoll(votingModel, balanceModel, null);
            }
        }

        private void CreatePollByAsset()
        {
            using (Logger = new TestsessionLogger())
            {
                const VotingModel votingModel = VotingModel.Asset;
                const MinBalanceModel balanceModel = MinBalanceModel.Asset;
                var assetId = TestSettings.ExistingAssetId;

                CreatePoll(votingModel, balanceModel, assetId);
            }
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
            AssertEquals(createPollParameters.Name, pollCreation.Name, "poll Name");
            AssertEquals(createPollParameters.Description, pollCreation.Description, "poll Description");
            AssertEquals(createPollParameters.FinishHeight, pollCreation.FinishHeight, "poll FinishHeight");
            AssertEquals(createPollParameters.VotingModel, pollCreation.VotingModel, "poll VotingModel");
            AssertEquals(createPollParameters.MinNumberOfOptions, pollCreation.MinNumberOfOptions, "poll MinNumberOfOptions");
            AssertEquals(createPollParameters.MaxNumberOfOptions, pollCreation.MaxNumberOfOptions, "poll MaxNumberOfOptions");
            AssertEquals(createPollParameters.MinRangeValue, pollCreation.MinRangeValue, "poll MinRangeValue");
            AssertEquals(createPollParameters.MaxRangeValue, pollCreation.MaxRangeValue, "poll MaxRangeValue");
            AssertEquals(createPollParameters.Options, pollCreation.Options, "poll Options");
            AssertEquals(createPollParameters.MinBalance, pollCreation.MinBalance, "poll MinRangeValue");
            AssertEquals(createPollParameters.MinBalanceModel, pollCreation.MinBalanceModel, "poll MinBalanceModel");
            AssertEquals(createPollParameters.HoldingId ?? 0, pollCreation.HoldingId, "poll HoldingId");
        }
    }
}