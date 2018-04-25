using System.Text.RegularExpressions;

namespace Concordance
{
    public static class StringExtensions
    {
        private static readonly Regex NonAlpha = new Regex("[^a-zA-Z]");

        public static string JustAlpha(this string word)
        {
            return NonAlpha.Replace(word ?? string.Empty, string.Empty).ToLower();
        }
    }
}
