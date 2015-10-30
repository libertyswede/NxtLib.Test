using Microsoft.Framework.Logging;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace NxtLib.Test
{
    public class TestsessionLogger : IDisposable
    {
        private readonly string _function;
        private readonly string _class;

        private readonly ILogger Logger;
        private int _failCount;
        private int _successCount;
        private string _errorMessage = string.Empty;

        public TestsessionLogger(ILogger logger, [CallerMemberName] string function = "", [CallerFilePath] string callerFile = "")
        {
            Logger = logger;
            _function = function;
            _class = Regex.Match(callerFile, @".*\\([^\\]+)\.cs").Groups[1].Value;
            Log("Started");
        }

        public void Fail(string errorMessage = null)
        {
            if (_failCount == 0 && !string.IsNullOrEmpty(errorMessage))
            {
                _errorMessage = errorMessage;
            }
            _failCount++;
        }

        public void Success()
        {
            _successCount++;
        }

        public void Dispose()
        {
            Log("Ended");
            if (_failCount > 0)
            {
                Logger.LogWarning("{0} failed tests, {1} succeeded", _failCount, _successCount);
                Logger.LogError(_errorMessage);
            }
            _failCount = 0;
            _successCount = 0;
        }

        private void Log(string action)
        {
            Logger.LogInformation("{0} {1} {2}", action, _class, _function);
        }
    }
}
