using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Validations.Enum;

namespace FifoApi.Extensions
{
    public static class ComparisonOperatorExtensions
    {
        public static string ToReadAbleString(this ComparisonOperatorType op)
        {
            return op switch
            {
                ComparisonOperatorType.LessThan => "less than",
                ComparisonOperatorType.LessThanEqual => "less than or equal to",
                ComparisonOperatorType.GreaterThan => "greater than",
                ComparisonOperatorType.GreaterThanEqual => "greater than or equal to",
                ComparisonOperatorType.Equal => "equal to",
                ComparisonOperatorType.NotEqual => "not equal to",
                _ => "invalid comparison"
            };
        }
    }
}