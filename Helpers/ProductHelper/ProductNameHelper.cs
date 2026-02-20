using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FifoApi.Helpers.ProductHelper
{
    public static class ProductNameHelper
    {
        public static string CleanProductName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string result = input.Trim();

            result = Regex.Replace(result, @"[^a-zA-Z0-9\s\-]", "");

            result = Regex.Replace(result, @"\s+", " ");

            result = Regex.Replace(result, @"\-{2,}", "-");

            result = result.Trim('-').Trim();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            return textInfo.ToTitleCase(result.ToLower());
        }
    }
}