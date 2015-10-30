using Microsoft.Dnx.Runtime;
using NxtLib.Tokens;

namespace NxtLib.Test.Tokens
{
    public interface ITokenServiceTest : ITest
    {
    }

    public class TokenServiceTest : ITokenServiceTest
    {
        private readonly string _fileName;
        private const string FileToken = "41c8m87jfsjuqgq0ollfnpf4npmsq62ornvrtus41ssnfisbf9fcuk00jamj53o3fee6vdpit0ltdi9rkti7l8cq0jijam3ttjgc1ipucse0or694nsfrvci3hi2qo6q6id40qbsd0n4fr2ld786pfq43oai3c1t";
        private readonly ITokenService _service;

        public TokenServiceTest(ITokenService service, IApplicationEnvironment env)
        {
            _service = service;
            _fileName = env.ApplicationBasePath + @"\Files\test.txt";
        }

        public void RunAllTests()
        {
            TestDecodeFileToken();
            TestGenerateFileToken();
        }

        private void TestGenerateFileToken()
        {
            var result = _service.GenerateFileToken("abc123", _fileName).Result;
        }

        private void TestDecodeFileToken()
        {
            var result = _service.DecodeFileToken(_fileName, FileToken).Result;
        }
    }
}
