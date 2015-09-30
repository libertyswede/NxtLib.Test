namespace NxtLib.Test.Tokens
{
    class TokenServiceTest
    {
        private readonly string _fileName;
        private const string FileToken = "41c8m87jfsjuqgq0ollfnpf4npmsq62ornvrtus41ssnfisbf9fcuk00jamj53o3fee6vdpit0ltdi9rkti7l8cq0jijam3ttjgc1ipucse0or694nsfrvci3hi2qo6q6id40qbsd0n4fr2ld786pfq43oai3c1t";

        public TokenServiceTest()
        {
            _fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "Files\\test.txt";
        }

        public void RunAllTests()
        {
            TestDecodeFileToken();
            TestGenerateFileToken();
        }

        private void TestGenerateFileToken()
        {
            var service = TestSettings.ServiceFactory.CreateTokenService();
            var result = service.GenerateFileToken("abc123", _fileName).Result;
        }

        private void TestDecodeFileToken()
        {
            var service = TestSettings.ServiceFactory.CreateTokenService();
            var result = service.DecodeFileToken(_fileName, FileToken).Result;
        }
    }
}
