using System.Collections.Generic;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    public class CastVoteTest : TestBase
    {
        private readonly IVotingSystemService _votingSystemService;

        public CastVoteTest()
        {
            _votingSystemService = TestSettings.ServiceFactory.CreateVotingSystemService();
        }

        public void Test()
        {
            if (!TestSettings.RunCostlyTests)
            {
                Logger.Warn("RunCostlyTests is set to false, skipping CastVote");
                return;
            }
            var castVoteReply = _votingSystemService.CastVote(TestSettings.PollId, new Dictionary<int, int> {{0, 1}},
                CreateTransaction.CreateTransactionBySecretPhrase(true)).Result;

            var attachment = (MessagingVoteCastingAttachment)castVoteReply.Transaction.Attachment;
            Compare(2, attachment.Votes.Count, "vote count");
            Compare(1, attachment.Votes[0], "vote value at index 0");
            Compare(-128, attachment.Votes[1], "vote value at index 1");
        }
    }
}