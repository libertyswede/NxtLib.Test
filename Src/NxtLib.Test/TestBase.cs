using System.Collections.Generic;
using System.Linq;

namespace NxtLib.Test
{
    public abstract class TestBase
    {
        protected static TestsessionLogger Logger;

        protected static void Compare(string expected, string actual, string propertyName)
        {
            if (!string.Equals(expected, actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void Compare(long? expected, long? actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void Compare<T>(T expected, T actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void Compare<T>(T? expected, T? actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void Compare(List<string> expected, List<string> actual, string propertyName)
        {
            if (expected.Count != actual.Count)
            {
                Logger.Fail(string.Format("Unexpected length of {0}, expected: {1}, actual: {2}", propertyName, expected.Count, actual.Count));
            }
            for (var i = 0; i < expected.Count; i++)
            {
                if (!string.Equals(expected[i], actual[i]))
                {
                    Logger.Fail(string.Format("Unexpected string value of {0}, index {1}, expected: {2}, actual: {3}", propertyName, i, expected[i], actual[i]));
                }
            }
        }

        protected static void CheckIsNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: Something, actual: {1}", propertyName, value));
            }
        }

        protected static void CheckLargerThanZero(ulong value, string propertyName)
        {
            if (value == 0)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value));
            }
        }

        protected static void CheckIsFalse(bool value, string propertyName)
        {
            if (value)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: false, actual: true", propertyName));
            }
        }

        protected static void CheckLargerThanZero(long value, string propertyName)
        {
            if (value <= 0)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value));
            }
        }

        protected static void CheckListHasValues<T>(List<T> values, string propertyName)
        {
            if (!values.Any())
            {
                Logger.Fail(string.Format("Unexpected number of values in list {0}, expected: Any(), actual: 0", propertyName));
            }
        }
    }
}
