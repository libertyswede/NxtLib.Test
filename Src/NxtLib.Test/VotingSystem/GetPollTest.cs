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
            Logger.Info("");
            foreach (var expected in _getPollsReply.Polls)
            {
                var getPollReply = _votingSystemService.GetPoll(expected.PollId).Result;
                CheckIsNullOrEmpty(getPollReply.Name, "Name");
                CheckIsNullOrEmpty(getPollReply.Description, "Description");
                CheckLargerThanZero(getPollReply.FinishHeight, "FinishHeight");
                CheckLargerThanZero(getPollReply.MinNumberOfOptions, "MinNumberOfOptions");
                CheckLargerThanZero(getPollReply.MaxNumberOfOptions, "MaxNumberOfOptions");
                CheckListHasValues(getPollReply.Options, "Options");
            }
        }
    }
}