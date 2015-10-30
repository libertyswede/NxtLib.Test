using System.Linq;
using Microsoft.Framework.Logging;
using NxtLib.AssetExchange;
using NxtLib.MonetarySystem;
using NxtLib.Transactions;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    public interface IGetPollResultTest
    {
        void Test();
    }

    public class GetPollResultTest : TestBase, IGetPollResultTest
    {
        private readonly IVotingSystemService _votingSystemService;
        private readonly GetPollsReply _getPolls;
        private readonly ITransactionService _transactionService;
        private readonly IAssetExchangeService _assetExchangeService;
        private readonly ILogger _logger;
        private readonly IMonetarySystemService _monetarySystemService;

        public GetPollResultTest(IServiceFactory serviceFactory, ILogger logger)
        {
            _votingSystemService = serviceFactory.CreateVotingSystemService();
            _transactionService = serviceFactory.CreateTransactionService();
            _assetExchangeService = serviceFactory.CreateAssetExchangeService();
            _monetarySystemService = serviceFactory.CreateMonetarySystemService();

            _logger = logger;
            _getPolls = _votingSystemService.GetPolls(includeFinished: true).Result;
        }

        public void Test()
        {
            TestAllForExceptions();
        }

        private void TestAllForExceptions()
        {
            using (Logger = new TestsessionLogger(_logger))
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
                var assetReply = _assetExchangeService.GetAsset(attachment.HoldingId).Result;
                AssertEquals(assetReply.Decimals, getPollResult.Decimals, "Decimals");
            }
            else if (attachment.MinBalanceModel == MinBalanceModel.Currency)
            {
                var currency = _monetarySystemService.GetCurrency(CurrencyLocator.ByCurrencyId(attachment.HoldingId)).Result;
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