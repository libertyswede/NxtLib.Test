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
            var password = _passwordGenerator.GeneratePassword(1);
            AssertIsFalse(password.Contains(""), "PasswordLength");
        }
    }
}
