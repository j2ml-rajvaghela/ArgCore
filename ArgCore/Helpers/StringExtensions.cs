using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace ArgCore.Helpers
{
    public static class StringExtensions
    {
        public static string ToSlug(this string text)
        {
            string value = text.Normalize(NormalizationForm.FormD).Trim();
            StringBuilder builder = new StringBuilder();

            foreach (char c in text.ToCharArray())
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);

            value = builder.ToString();

            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);

            value = Regex.Replace(Regex.Replace(Encoding.ASCII.GetString(bytes), @"\s{2,}|[^\w]", " ", RegexOptions.ECMAScript).Trim(), @"\s+", "_");

            return value;
        }

        public static IHtmlContent FormattedValue(decimal value)
        {
            var formattedValue = string.Format("{0:n}", value);
            return new HtmlString(formattedValue);
        }
    }
}
