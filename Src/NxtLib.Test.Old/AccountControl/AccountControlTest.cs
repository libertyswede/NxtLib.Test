using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using NxtLib.AccountControl;
using NxtLib.VotingSystem;

namespace NxtLib.Test.AccountControl
{
    public interface IAccountControlTest : ITest
    {
    }

    public class AccountControlTest : TestBase, IAccountControlTest
    {
        private readonly ILogger _logger;
        private readonly IAccountControlService _service;

        public AccountControlTest(ILogger logger, IAccountControlService service)
        {
            _logger = logger;
            _service = service;
        }

        public void RunAllTests()
        {
            TestGetAllPhasingOnlyControls();
            TestGetPhasingOnlyControl();
            TestSetPhasingOnlyControl();
        }

        private void TestGetAllPhasingOnlyControls()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var phasingOnlyControls = _service.GetAllPhasingOnlyControls().Result;
            }
        }

        private void TestGetPhasingOnlyControl()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var result = _service.GetPhasingOnlyControl(TestSettings.Account1).Result;
            }
        }

        private void TestSetPhasingOnlyControl()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var votingModel = VotingModel.Account;
                var controlQuorum = 1;
                var byPublicKey = CreateTransaction.CreateTransactionByPublicKey();
                var controlMinBalance = 1000;
                var controlMinBalanceModel = VotingModel.Nqt;
                var controlMaxFees = Amount.CreateAmountFromNxt(1000);

                var phasingOnlyControl = _service.SetPhasingOnlyControl(votingModel, controlQuorum,
                    byPublicKey, controlMinBalance, controlMinBalanceModel, null,
                    new[] {TestSettings.Account2.AccountRs}, controlMaxFees).Result;

                var attachment = (AccountControlSetPhasingOnlyAttachment)phasingOnlyControl.Transaction.Attachment;

                AssertEquals((int)votingModel, (int)attachment.PhasingVotingModel, nameof(attachment.PhasingVotingModel));
                AssertEquals(controlQuorum, attachment.PhasingQuorum, nameof(attachment.PhasingQuorum));
                AssertEquals(controlMinBalance, attachment.PhasingMinBalance, nameof(attachment.PhasingMinBalance));
                AssertEquals((int)controlMinBalanceModel, (int)attachment.PhasingMinBalanceModel, nameof(attachment.PhasingMinBalanceModel));
                AssertEquals(controlMaxFees.Nqt, attachment.ControlMaxFees.Nqt, nameof(attachment.ControlMaxFees));
                AssertEquals(1, attachment.PhasingWhitelist.Count(), "attachment.PhasingWhitelist.Count()");
                AssertEquals(attachment.PhasingWhitelist.Single(), TestSettings.Account2.AccountId, nameof(attachment.PhasingWhitelist));
            }
        }
    }
}
