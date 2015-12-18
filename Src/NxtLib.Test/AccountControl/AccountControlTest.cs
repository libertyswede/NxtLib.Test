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
        private readonly IAccountControlService _accountControlService;

        public AccountControlTest(ILogger logger, IAccountControlService accountControlService)
        {
            _logger = logger;
            _accountControlService = accountControlService;
        }

        public void RunAllTests()
        {
            TestSetPhasingOnlyControl();
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

                var phasingOnlyControl = _accountControlService.SetPhasingOnlyControl(votingModel, controlQuorum,
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
