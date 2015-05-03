using System.Collections.Generic;
using System.Linq;
using NLog;

namespace NxtLib.Test
{
    public abstract class TestBase
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected static void Start()
        {
            Logger.Info("Starting CastVote test");
        }

        protected static void Compare(string expected, string actual, string propertyName)
        {
            if (!string.Equals(expected, actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        protected static void Compare(long? expected, long? actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        protected static void Compare<T>(T expected, T actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        protected static void Compare<T>(T? expected, T? actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Error("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual);
            }
        }

        protected static void Compare(List<string> expected, List<string> actual, string propertyName)
        {
            if (expected.Count != actual.Count)
            {
                Logger.Error("Unexpected length of {0}, expected: {1}, actual: {2}", propertyName, expected.Count, actual.Count);
            }
            for (var i = 0; i < expected.Count; i++)
            {
                if (!string.Equals(expected[i], actual[i]))
                {
                    Logger.Error("Unexpected string value of {0}, index {1}, expected: {2}, actual: {3}", propertyName, i, expected[i], actual[i]);
                }
            }
        }

        protected static void CheckIsNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Error("Unexpected {0}, expected: Something, actual: {1}", propertyName, value);
            }
        }

        protected static void CheckLargerThanZero(int value, string propertyName)
        {
            if (value <= 0)
            {
                Logger.Error("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value);
            }
        }

        protected static void CheckListHasValues<T>(List<T> values, string propertyName)
        {
            if (!values.Any())
            {
                Logger.Error("Unexpected number of values in list {0}, expected: Any(), actual: 0", propertyName);
            }
        }
    }
}
