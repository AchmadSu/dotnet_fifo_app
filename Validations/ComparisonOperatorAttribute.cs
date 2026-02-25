using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Validations
{
    public class ComparisonOperatorAttribute : ValidationAttribute
    {
        private static readonly HashSet<string> AllowedOperators = new()
        {
            ">", ">=", "<", "<=", "="
        };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var op = value.ToString()?.Trim();
            if (string.IsNullOrEmpty(op)) return ValidationResult.Success;

            if (!AllowedOperators.Contains(op))
            {
                return new ValidationResult(
                    $"Invalid operator '{op}'. Allowed operators are: {string.Join(", ", AllowedOperators)}"
                );
            }

            return ValidationResult.Success;
        }
    }
}