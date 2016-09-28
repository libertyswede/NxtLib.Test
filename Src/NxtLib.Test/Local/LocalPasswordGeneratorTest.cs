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
            TestGenerateDeterenisticPassword();
        }

        private void TestGenerateDeterenisticPassword()
        {
            using (Logger = new TestsessionLogger(_logger))
            {
                var seed = _localPasswordGenerator.GeneratePassword();
                var password1 = _localPasswordGenerator.GenerateDetermenisticPassword(seed, 42);
                var password2 = _localPasswordGenerator.GenerateDetermenisticPassword(seed, 42);
                var password3 = _localPasswordGenerator.GenerateDetermenisticPassword(seed, 43);
                AssertEquals(password1, password2, "Should be deterministic!");
                AssertIsFalse(password1.Equals(password3), "Should not be deterministic!");
            }
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
