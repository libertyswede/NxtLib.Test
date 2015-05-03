using System.Collections.Generic;
using System.Linq;

namespace NxtLib.Test
{
    public abstract class TestBase
    {
        protected static TestsessionLogger Logger;

        protected static void AssertEquals(string expected, string actual, string propertyName)
        {
            if (!string.Equals(expected, actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void AssertEquals(ulong expected, ulong actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void AssertEquals(long? expected, long? actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void AssertEquals<T>(T expected, T actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void AssertEquals<T>(T? expected, T? actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: {1}, actual: {2}", propertyName, expected, actual));
            }
        }

        protected static void AssertIsNull(object actual, string propertyName)
        {
            if (actual != null)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: null, actual: {1}", propertyName, actual));
            }
        }

        protected static void AssertEquals(List<string> expected, List<string> actual, string propertyName)
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

        protected static void AssertIsNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: Something, actual: {1}", propertyName, value));
            }
        }

        protected static void AssertIsLargerThanZero(ulong? value, string propertyName)
        {
            if (!value.HasValue)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: null", propertyName));
            }
            else if (value == 0)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value));
            }
        }

        protected static void AssertIsLargerThanZero(ulong value, string propertyName)
        {
            if (value == 0)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value));
            }
        }

        protected static void AssertIsLargerThanZero(long value, string propertyName)
        {
            if (value <= 0)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: > 0, actual: {1}", propertyName, value));
            }
        }

        protected static void AssertIsFalse(bool value, string propertyName)
        {
            if (value)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: false, actual: true", propertyName));
            }
        }

        protected static void AssertIsTrue(bool value, string propertyName)
        {
            if (!value)
            {
                Logger.Fail(string.Format("Unexpected {0}, expected: true, actual: false", propertyName));
            }
        }

        protected static void AssertListHasValues<T>(List<T> values, string propertyName)
        {
            if (!values.Any())
            {
                Logger.Fail(string.Format("Unexpected number of values in list {0}, expected: Any(), actual: 0", propertyName));
            }
        }

        protected void AssertIsLargerThanOrEquals(long largerValue, long smallerValue, string largerPropertyName, string smallerPropertyName)
        {
            if (largerValue < smallerValue)
            {
                Logger.Fail(string.Format("Unexpected relative values {0} and {1}. Expected: {2} >= {3}", 
                    largerPropertyName, smallerPropertyName, largerValue, smallerValue));
            }
        }
    }
}
