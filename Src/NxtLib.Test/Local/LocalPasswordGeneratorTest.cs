using System;
using Microsoft.Extensions.Logging;
using NxtLib.Local;

namespace NxtLib.Test.Local
{
    public interface ILocalPasswordGeneratorTest : ITest
    {
    }

    public class LocalPasswordGeneratorTest : TestBase, ILocalPasswordGeneratorTest
    {
        private readonly ILogger _logger;
        private readonly ILocalPasswordGenerator _localPasswordGenerator;

        public LocalPasswordGeneratorTest(ILogger logger, ILocalPasswordGenerator localPasswordGenerator)
        {
            _logger = logger;
            _localPasswordGenerator = localPasswordGenerator;
        }

        public void RunAllTests()
        {
            TestGeneratePassword();
        }

        private void TestGeneratePassword()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                try
                {
                    _localPasswordGenerator.GeneratePassword(1);
                    Logger.Fail("No ArgumentException was thrown!");
                }
                catch (ArgumentException)
                {
                }
                var password = _localPasswordGenerator.GeneratePassword();
                var words = password.Split(' ');
                AssertEquals(12, words.Length, "Number of words");
            }
        }
    }
}
