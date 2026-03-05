using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FifoApi.Extensions;
using FifoApi.Validations.Enum;

namespace FifoApi.Validations
{
    public class CompareToPropertyAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        private readonly ComparisonOperatorType _operator;

        public CompareToPropertyAttribute(
            string comparisonProperty,
            ComparisonOperatorType op
        )
        {
            _comparisonProperty = comparisonProperty;
            _operator = op;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            PropertyInfo? property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == null)
                return ValidationResult.Success;

            if (value is not IComparable current || comparisonValue is not IComparable compareTo)
                return new ValidationResult("Properties must implement IComparable");

            int result = current.CompareTo(compareTo);

            bool isValid = _operator switch
            {
                ComparisonOperatorType.LessThan => result < 0,
                ComparisonOperatorType.LessThanEqual => result <= 0,
                ComparisonOperatorType.GreaterThan => result > 0,
                ComparisonOperatorType.GreaterThanEqual => result >= 0,
                ComparisonOperatorType.Equal => result == 0,
                ComparisonOperatorType.NotEqual => result != 0,
                _ => false
            };

            if (!isValid)
            {
                return new ValidationResult(
                    ErrorMessage ??
                    $"{validationContext.MemberName} must be {_operator.ToReadAbleString()} {_comparisonProperty}"
                );
            }

            return ValidationResult.Success;
        }
    }
}