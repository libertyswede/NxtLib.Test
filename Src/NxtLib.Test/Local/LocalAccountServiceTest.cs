using System;
using Microsoft.Framework.Logging;
using NxtLib.Local;

namespace NxtLib.Test.Local
{
    public interface ILocalAccountServiceTest : ITest
    {
    }

    public class LocalAccountServiceTest : TestBase, ILocalAccountServiceTest
    {
        private readonly ILogger _logger;
        private readonly ILocalAccountService _localAccountService;

        public LocalAccountServiceTest(ILogger logger, ILocalAccountService localAccountService)
        {
            _logger = logger;
            _localAccountService = localAccountService;
        }

        public void RunAllTests()
        {
            
        }
    }
}