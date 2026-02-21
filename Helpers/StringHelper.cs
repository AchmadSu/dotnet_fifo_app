using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FifoApi.Helpers
{
    public static class StringHelper
    {
        public static string CleanStringName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string result = input.Trim();

            result = Regex.Replace(result, @"[^a-zA-Z0-9\s\-]", "");
            result = Regex.Replace(result, @"\s+", " ");
            result = Regex.Replace(result, @"\-{2,}", "-");
            result = result.Trim('-').Trim();

            var words = result.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word.All(char.IsUpper))
                {
                    words[i] = word;
                }
                else if (word.Length <= 10 && word.All(char.IsLetter))
                {
                    words[i] = word.ToUpper();
                }
                else
                {
                    words[i] = textInfo.ToTitleCase(word.ToLower());
                }
            }
            return string.Join(" ", words);
        }

        public static string NormalizeSku(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var cleaned = Regex.Replace(input.Trim(), @"[^a-zA-Z0-9\-]", "");
            cleaned = Regex.Replace(cleaned, @"\-{2,}", "-").Trim('-');

            var segments = cleaned.Split('-', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Any(char.IsLetter))
                {
                    segments[i] = segments[i].ToUpper();
                }
            }

            return string.Join("-", segments);
        }
    }
}