using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    internal class GetPollTest : TestBase
    {
        private readonly IVotingSystemService _votingSystemService;
        private readonly GetPollsReply _getPollsReply;

        internal GetPollTest()
        {
            _votingSystemService = TestSettings.ServiceFactory.CreateVotingSystemService();
            _getPollsReply = _votingSystemService.GetPolls(includeFinished: true).Result;
        }

        public void Test()
        {
            using (Logger = new TestsessionLogger())
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