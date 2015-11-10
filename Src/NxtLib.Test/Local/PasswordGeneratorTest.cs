using System;
using Microsoft.Framework.Logging;
using NxtLib.Local;

namespace NxtLib.Test.Local
{
    public interface IPasswordGeneratorTest : ITest
    {
    }

    public class PasswordGeneratorTest : TestBase, IPasswordGeneratorTest
    {
        private readonly ILogger _logger;
        private readonly IPasswordGenerator _passwordGenerator;

        public PasswordGeneratorTest(ILogger logger, IPasswordGenerator passwordGenerator)
        {
            _logger = logger;
            _passwordGenerator = passwordGenerator;
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
                    _passwordGenerator.GeneratePassword(1);
                    Logger.Fail("No ArgumentException was thrown!");
                }
                catch (ArgumentException)
                {
                }
                var password = _passwordGenerator.GeneratePassword();
                var words = password.Split(' ');
                AssertEquals(12, words.Length, "Number of words");
            }
        }
    }
}
