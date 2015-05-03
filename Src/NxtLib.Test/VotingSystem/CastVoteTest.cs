using System.Collections.Generic;
using NLog;
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
            if (attachment.Votes.Count != 2)
            {
                Logger.Error("Unexpected vote count, expected: 2, actual: {0}", attachment.Votes.Count);
            }
            if (attachment.Votes[0] != 1)
            {
                Logger.Error("Unexpected vote value at index 0, expected: 1, actual: {0}", attachment.Votes[0]);
            }
            if (attachment.Votes[1] != -128)
            {
                Logger.Error("Unexpected vote value at index 1, expected: -128, actual: {0}", attachment.Votes[0]);
            }
        }
    }
}