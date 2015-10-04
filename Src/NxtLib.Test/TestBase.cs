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
                Logger.Fail($"Unexpected {propertyName}, expected: {expected}, actual: {actual}");
            }
        }

        protected static void AssertEquals(ulong expected, ulong actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: {expected}, actual: {actual}");
            }
        }

        protected static void AssertEquals(long? expected, long? actual, string propertyName)
        {
            if (expected != actual)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: {expected}, actual: {actual}");
            }
        }

        protected static void AssertEquals<T>(T expected, T actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail($"Unexpected {propertyName}, expected: {expected}, actual: {actual}");
            }
        }

        protected static void AssertEquals<T>(T? expected, T? actual, string propertyName) where T : struct
        {
            if (!expected.Equals(actual))
            {
                Logger.Fail($"Unexpected {propertyName}, expected: {expected}, actual: {actual}");
            }
        }

        protected static void AssertIsNull(object actual, string propertyName)
        {
            if (actual != null)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: null, actual: {actual}");
            }
        }

        protected static void AssertEquals(List<string> expected, List<string> actual, string propertyName)
        {
            if (expected.Count != actual.Count)
            {
                Logger.Fail($"Unexpected length of {propertyName}, expected: {expected.Count}, actual: {actual.Count}");
            }
            for (var i = 0; i < expected.Count; i++)
            {
                if (!string.Equals(expected[i], actual[i]))
                {
                    Logger.Fail(
                        $"Unexpected string value of {propertyName}, index {i}, expected: {expected[i]}, actual: {actual[i]}");
                }
            }
        }

        protected static void AssertIsNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Fail($"Unexpected {propertyName}, expected: Something, actual: {value}");
            }
        }

        protected static void AssertIsLargerThanZero(ulong? value, string propertyName)
        {
            if (!value.HasValue)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: > 0, actual: null");
            }
            else if (value == 0)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: > 0, actual: {value}");
            }
        }

        protected static void AssertIsLargerThanZero(ulong value, string propertyName)
        {
            if (value == 0)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: > 0, actual: {value}");
            }
        }

        protected static void AssertIsLargerThanZero(long value, string propertyName)
        {
            if (value <= 0)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: > 0, actual: {value}");
            }
        }

        protected static void AssertIsFalse(bool value, string propertyName)
        {
            if (value)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: false, actual: true");
            }
        }

        protected static void AssertIsTrue(bool value, string propertyName)
        {
            if (!value)
            {
                Logger.Fail($"Unexpected {propertyName}, expected: true, actual: false");
            }
        }

        protected static void AssertListHasValues<T>(List<T> values, string propertyName)
        {
            if (!values.Any())
            {
                Logger.Fail($"Unexpected number of values in list {propertyName}, expected: Any(), actual: 0");
            }
        }

        protected void AssertIsLargerThanOrEquals(long largerValue, long smallerValue, string largerPropertyName, string smallerPropertyName)
        {
            if (largerValue < smallerValue)
            {
                Logger.Fail(
                    $"Unexpected relative values {largerPropertyName} and {smallerPropertyName}. Expected: {largerValue} >= {smallerValue}");
            }
        }
    }
}
