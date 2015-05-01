using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    class VotingSystemServiceTest
    {
        private readonly IVotingSystemService _votingSystemService;

        public VotingSystemServiceTest()
        {
            _votingSystemService = TestSettings.ServiceFactory.CreateVotingSystemService();
        }

        public void RunAllTests()
        {
            TestCreatePoll();
        }

        private void TestCreatePoll()
        {
            var createPollTest = new CreatePollTest(_votingSystemService);
            createPollTest.Test();
        }
    }
}
