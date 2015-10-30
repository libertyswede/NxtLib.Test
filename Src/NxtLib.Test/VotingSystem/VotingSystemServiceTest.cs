namespace NxtLib.Test.VotingSystem
{
    public interface IVotingSystemServiceTest : ITest
    {
    }

    public class VotingSystemServiceTest : IVotingSystemServiceTest
    {
        private readonly IGetPollResultTest _getPollResultTest;
        private readonly IGetPollTest _getPollTest;
        private readonly ICastVoteTest _castVoteTest;
        private readonly ICreatePollTest _createPollTest;

        public VotingSystemServiceTest(IGetPollResultTest getPollResultTest, IGetPollTest getPollTest,
            ICastVoteTest castVoteTest, ICreatePollTest createPollTest)
        {
            _getPollResultTest = getPollResultTest;
            _getPollTest = getPollTest;
            _castVoteTest = castVoteTest;
            _createPollTest = createPollTest;
        }

        public void RunAllTests()
        {
            TestCastVote();
            TestCreatePoll();
            TestGetPoll();
            TestGetPollResult();
        }

        private void TestGetPollResult()
        {
            _getPollResultTest.Test();
        }

        private void TestGetPoll()
        {
            _getPollTest.Test();
        }

        private void TestCastVote()
        {
            _castVoteTest.Test();
        }

        private void TestCreatePoll()
        {
            _createPollTest.Test();
        }
    }
}
