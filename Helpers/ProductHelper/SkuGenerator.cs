using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FifoApi.Helpers.ProductHelper
{
    public static class SkuGenerator
    {
        public static string Generate(
            string productName,
            int sequenceNumber,
            string prefix = "PRD"
        )
        {
            var cleanName = Regex.Replace(productName.ToUpper(), @"[^A-Z0-9]", "");

            cleanName = cleanName.Length > 8 ? cleanName.Substring(0, 8) : cleanName;

            return $"{prefix}-{cleanName}-{DateTime.UtcNow:yyyyMM}-{sequenceNumber:D6}";
        }
    }
}