using Microsoft.Framework.Logging;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    public interface IGetPollTest
    {
        void Test();
    }

    public class GetPollTest : TestBase, IGetPollTest
    {
        private readonly IVotingSystemService _votingSystemService;
        private readonly ILogger _logger;
        private readonly GetPollsReply _getPollsReply;

        public GetPollTest(IVotingSystemService votingSystemService, ILogger logger)
        {
            _votingSystemService = votingSystemService;
            _logger = logger;
            _getPollsReply = _votingSystemService.GetPolls(includeFinished: true).Result;
        }

        public void Test()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                foreach (var expected in _getPollsReply.Polls)
                {
                    var getPollReply = _votingSystemService.GetPoll(expected.PollId).Result;
                    AssertIsNullOrEmpty(getPollReply.Name, "Name");
                    AssertIsNullOrEmpty(getPollReply.Description, "Description");
                    AssertIsLargerThanZero(getPollReply.FinishHeight, "FinishHeight");
                    AssertIsLargerThanZero(getPollReply.MinNumberOfOptions, "MinNumberOfOptions");
                    AssertIsLargerThanZero(getPollReply.MaxNumberOfOptions, "MaxNumberOfOptions");
                    AssertListHasValues(getPollReply.Options, "Options");
                }
            }
        }
    }
}