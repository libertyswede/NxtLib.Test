namespace NxtLib.Test.VotingSystem
{
    class VotingSystemServiceTest
    {
        public void RunAllTests()
        {
            TestCastVote();
            TestCreatePoll();
            TestGetPoll();
        }

        private void TestGetPoll()
        {
            var getPollTest = new GetPollTest();
            getPollTest.Test();
        }

        private void TestCastVote()
        {
            var castVoteTest = new CastVoteTest();
            castVoteTest.Test();
        }

        private void TestCreatePoll()
        {
            var createPollTest = new CreatePollTest();
            createPollTest.Test();
        }
    }
}
