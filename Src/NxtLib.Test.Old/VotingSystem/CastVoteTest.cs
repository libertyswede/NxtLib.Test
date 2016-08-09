using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    public interface ICastVoteTest
    {
        void Test();
    }

    public class CastVoteTest : TestBase, ICastVoteTest
    {
        private readonly IVotingSystemService _votingSystemService;
        private readonly ILogger _logger;

        public CastVoteTest(IVotingSystemService votingSystemService, ILogger logger)
        {
            _votingSystemService = votingSystemService;
            _logger = logger;
        }

        public void Test()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                if (!TestSettings.RunCostlyTests)
                {
                    Logger.Warn("RunCostlyTests is set to false, skipping CastVote");
                    return;
                }

                var castVoteReply = _votingSystemService.CastVote(TestSettings.PollId, new Dictionary<int, int> { { 0, 1 } },
                CreateTransaction.CreateTransactionBySecretPhrase(true)).Result;

                var attachment = (MessagingVoteCastingAttachment)castVoteReply.Transaction.Attachment;
                AssertEquals(2, attachment.Votes.Count, "vote count");
                AssertEquals(1, attachment.Votes[0], "vote value at index 0");
                AssertEquals(-128, attachment.Votes[1], "vote value at index 1");
            }
        }
    }
}