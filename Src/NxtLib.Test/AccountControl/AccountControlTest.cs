using System;
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
                var phasingOnlyControl = _accountControlService.SetPhasingOnlyControl(VotingModel.Account, 1,
                    CreateTransaction.CreateTransactionByPublicKey(), 1000, VotingModel.Account, null,
                    new[] {TestSettings.Account2.AccountRs}, Amount.CreateAmountFromNxt(1000)).Result;
            }
        }
    }
}
