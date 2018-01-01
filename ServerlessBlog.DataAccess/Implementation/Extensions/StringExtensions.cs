using System;
using System.Text.RegularExpressions;

namespace ServerlessBlog.DataAccess.Implementation.Extensions
{
    internal static class StringExtensions
    {
        public static string ToUrlString(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            Regex rgx = new Regex("[^a-zA-Z0-9-]");
            value = rgx.Replace(value, "");
            value = Char.ToLowerInvariant(value[0]) + value.Substring(1);
            return value;
        }
    }
}
