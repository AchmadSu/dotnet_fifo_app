using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Validations
{
    public class DecimalRangeAttribute : ValidationAttribute
    {
        private readonly decimal _min;
        private readonly decimal _max;

        public DecimalRangeAttribute(double min, double max)
        {
            _min = (decimal)min;
            _max = (decimal)max;
            ErrorMessage = $"Value must be between {_min} and {_max}";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            if (value is decimal d)
            {
                return d >= _min && d <= _max;
            }
            return false;
        }
    }
}