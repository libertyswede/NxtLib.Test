using Microsoft.Extensions.Logging;
using NxtLib.MonetarySystem;

namespace NxtLib.Test.MonetarySystem
{
    public interface IMonetarySystemServiceTest : ITest
    {
    }

    public class MonetarySystemServiceTest : TestBase, IMonetarySystemServiceTest
    {
        private readonly ILogger _logger;
        private readonly IMonetarySystemService _service;

        public MonetarySystemServiceTest(ILogger logger, IMonetarySystemService service)
        {
            _logger = logger;
            _service = service;
        }

        public void RunAllTests()
        {
            TestGetAvailableToBuy();
            TestGetAvailableToSell();
        }

        private void TestGetAvailableToBuy()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var reply = _service.GetAvailableToBuy(TestSettings.ExistingCurrencyId, 1).Result;
            }
        }

        private void TestGetAvailableToSell()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var reply = _service.GetAvailableToSell(TestSettings.ExistingCurrencyId, 1).Result;
            }
        }
    }
}
