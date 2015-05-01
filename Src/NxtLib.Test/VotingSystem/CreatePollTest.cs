using System.Collections.Generic;
using NLog;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    internal class CreatePollTest
    {
        private readonly IVotingSystemService _votingSystemService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CreatePollTest(IVotingSystemService votingSystemService)
        {
            _votingSystemService = votingSystemService;
        }

        public void Test()
        {
            CreatePollByCurrency();
        }

        private void CreatePollByCurrency()
        {
            var name = Utils.GenerateRandomString(10);
            var description = Utils.GenerateRandomString(30);
            var finishHeight = TestSettings.MaxHeight + 1000;
            const VotingModel votingModel = VotingModel.Account;
            const int minNumberOfOptions = 1;
            const int maxNumberOfOptions = 1;
            const int minRangeValue = 0;
            const int maxRangeValue = 1;
            var options = new List<string> {"How are you doing?"};
            const int minBalance = 1;
            const MinBalanceModel balanceModel = MinBalanceModel.Currency;
            var currencyId = TestSettings.ExistingCurrencyId;

            var createPollParameters = new CreatePollParameters(name, description, finishHeight, votingModel, minNumberOfOptions,
                maxNumberOfOptions, minRangeValue, maxRangeValue, options, minBalance, balanceModel, currencyId);

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

        private static void Compare(string expected, string actual, string propertyName)
        {
            if (!string.Equals(expected, actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        private static void Compare(long? expected, long? actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        private static void Compare<T>(T expected, T actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        private static void Compare<T>(T? expected, T? actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        private static void Compare(List<string> expected, List<string> actual, string propertyName)
        {
            if (expected.Count != actual.Count)
            {
                Logger.Error("Unexpected length of {0}, expected: {1}, actual: {2}", propertyName, expected.Count, actual.Count);
            }
            for (var i = 0; i < expected.Count; i++)
            {
                if (string.Equals(expected[i], actual[i]))
                {
                    Logger.Error("Unexpected string value of {0}, index {1}, expected: {2}, actual{3}", propertyName, i, expected[i], actual[i]);
                }
            }
        }
    }
}