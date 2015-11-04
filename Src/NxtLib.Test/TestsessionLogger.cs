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

        private readonly ILogger _logger;
        private int _failCount;
        private int _warnCount;
        private int _successCount;
        private string _errorMessage = string.Empty;
        private string _warnMessage = string.Empty;

        public TestsessionLogger(ILogger logger, [CallerMemberName] string function = "", [CallerFilePath] string callerFile = "")
        {
            _logger = logger;
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
                _logger.LogWarning("{0} failed tests, {1} succeeded", _failCount, _successCount);
                _logger.LogError(_errorMessage);
            }
            if (_warnCount > 0)
            {
                _logger.LogWarning("{0} test warnings, {1} succeeded", _warnCount, _successCount);
                _logger.LogWarning(_warnMessage);
            }
            _failCount = 0;
            _successCount = 0;
        }

        private void Log(string action)
        {
            _logger.LogInformation("{0} {1} {2}", action, _class, _function);
        }

        public void Warn(string errorMessage)
        {
            if (_warnCount == 0 && !string.IsNullOrEmpty(errorMessage))
            {
                _warnMessage = errorMessage;
            }
            _warnCount++;
        }
    }
}
