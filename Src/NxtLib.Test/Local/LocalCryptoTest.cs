namespace NxtLib.Test.Local
{

    public interface ILocalCryptoTest : ITest
    {
    }

    public class LocalCryptoTest : TestBase, ILocalCryptoTest
    {
        private readonly ILocalAccountServiceTest _localAccountServiceTest;
        private readonly ILocalMessageServiceTest _localMessageServiceTest;
        private readonly ILocalPasswordGeneratorTest _localPasswordGeneratorTest;
        private readonly ILocalTokenServiceTest _localTokenServiceTest;

        public LocalCryptoTest(ILocalAccountServiceTest localAccountServiceTest,
            ILocalMessageServiceTest localMessageServiceTest, ILocalPasswordGeneratorTest localPasswordGeneratorTest,
            ILocalTokenServiceTest localTokenServiceTest)
        {
            _localAccountServiceTest = localAccountServiceTest;
            _localMessageServiceTest = localMessageServiceTest;
            _localPasswordGeneratorTest = localPasswordGeneratorTest;
            _localTokenServiceTest = localTokenServiceTest;
        }

        public void RunAllTests()
        {
            _localAccountServiceTest.RunAllTests();
            _localMessageServiceTest.RunAllTests();
            _localPasswordGeneratorTest.RunAllTests();
            _localTokenServiceTest.RunAllTests();
        }
    }
}
