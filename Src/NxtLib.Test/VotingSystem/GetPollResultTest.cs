using System.Linq;
using NxtLib.MonetarySystem;
using NxtLib.Transactions;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    internal class GetPollResultTest : TestBase
    {
        private readonly IVotingSystemService _votingSystemService;
        private readonly GetPollsReply _getPolls;
        private readonly ITransactionService _transactionService;

        public GetPollResultTest()
        {
            _votingSystemService = TestSettings.ServiceFactory.CreateVotingSystemService();
            _transactionService = TestSettings.ServiceFactory.CreateTransactionService();
            _getPolls = _votingSystemService.GetPolls(includeFinished: true).Result;
        }

        public void Test()
        {
            TestAllForExceptions();
        }

        private void TestAllForExceptions()
        {
            using (Logger = new TestsessionLogger())
            {
                foreach (var pollId in _getPolls.Polls.Where(p => p.PollId != 9315232938245213980).Select(p => p.PollId))
                {
                    var getPollResult = _votingSystemService.GetPollResult(pollId).Result;
                    var transaction = _transactionService.GetTransaction(GetTransactionLocator.ByTransactionId(pollId)).Result;
                    var attachment = (MessagingPollCreationAttachment)transaction.Attachment;
                    AssetDecimals(attachment, getPollResult);
                    AssertEquals(attachment.FinishHeight <= TestSettings.MaxHeight, getPollResult.Finished, "Finished");
                    AssertHoldingId(attachment, getPollResult);
                    AssertEquals(attachment.MinBalance, getPollResult.MinBalance, "MinBalance");
                    AssertEquals(attachment.MinBalanceModel, getPollResult.MinBalanceModel, "MinBalanceModel");
                    AssertEquals(attachment.Options, getPollResult.Options, "Options");
                    AssertEquals(pollId, getPollResult.PollId, "PollId");
                    AssertResults(attachment, getPollResult);
                    AssertEquals(attachment.VotingModel, getPollResult.VotingModel, "VotingModel");
                }
            }
        }

        private void AssertResults(MessagingPollCreationAttachment attachment, GetPollResultReply getPollResult)
        {
            var results = getPollResult.Results;
            AssertEquals(attachment.Options.Count, results.Count, "Results count");
            results.Where(r => !r.Result.HasValue).ToList().ForEach(r => AssertEquals(0, r.Weight, "Weight"));
        }

        private void AssetDecimals(MessagingPollCreationAttachment attachment, GetPollResultReply getPollResult)
        {
            if (attachment.MinBalanceModel == MinBalanceModel.Asset)
            {
                var assetReply = TestSettings.ServiceFactory.CreateAssetExchangeService().GetAsset(attachment.HoldingId).Result;
                AssertEquals(assetReply.Decimals, getPollResult.Decimals, "Decimals");
            }
            else if (attachment.MinBalanceModel == MinBalanceModel.Currency)
            {
                var currency = TestSettings.ServiceFactory.CreateMonetarySystemService()
                    .GetCurrency(CurrencyLocator.ByCurrencyId(attachment.HoldingId)).Result;
                AssertEquals(currency.Decimals, getPollResult.Decimals, "Decimals");
            }
        }

        private static void AssertHoldingId(MessagingPollCreationAttachment attachment, GetPollResultReply getPollResult)
        {
            if (attachment.MinBalanceModel != MinBalanceModel.Nqt && attachment.MinBalanceModel != MinBalanceModel.None)
            {
                AssertIsLargerThanZero(getPollResult.HoldingId, "HoldingId");
            }
            else
            {
                AssertEquals(0, getPollResult.HoldingId, "HoldingId");
            }
        }
    }
}