using System;
using System.Text;

namespace NxtLib.Test
{
    public static class Utilities
    {
        public static  string GenerateRandomString(int length)
        {
            var random = new Random();
            var builder = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                builder.Append((char)random.Next('a', 'z'));
            }
            return builder.ToString();
        }
    }
}
