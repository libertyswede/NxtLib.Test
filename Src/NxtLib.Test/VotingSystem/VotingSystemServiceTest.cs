using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NxtLib.VotingSystem;

namespace NxtLib.Test.VotingSystem
{
    class VotingSystemServiceTest
    {
        private readonly IVotingSystemService _votingSystemService;

        public VotingSystemServiceTest(IVotingSystemService votingSystemService)
        {
            _votingSystemService = votingSystemService;
        }

        public void RunAllTests()
        {
            TestCreatePoll();
        }

        private void TestCreatePoll()
        {
            var createPollTest = new CreatePollTest(_votingSystemService);
        }
    }

    internal class CreatePollTest
    {
        private readonly IVotingSystemService _votingSystemService;

        public CreatePollTest(IVotingSystemService votingSystemService)
        {
            _votingSystemService = votingSystemService;
        }

        public void Test()
        {
            //_votingSystemService.CreatePoll("", "", )
        }
    }
}
