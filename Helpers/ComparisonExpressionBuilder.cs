using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FifoApi.Helpers
{
    public static class ComparisonExpressionBuilder
    {
        private static readonly HashSet<string> AllowedOperators = new()
        {
            ">", ">=", "<", "<=", "=", "=="
        };

        public static Expression<Func<TEntity, bool>> Build<TEntity, TProperty>(
            Expression<Func<TEntity, TProperty>> selector,
            string op,
            TProperty value
        )
            where TProperty : struct, IComparable<TProperty>
        {
            if (!AllowedOperators.Contains(op))
                throw new ArgumentException("Invalid comparison operator");

            var parameter = selector.Parameters[0];

            Expression left = selector.Body;

            if (left is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
            {
                left = unary.Operand;
            }

            var right = Expression.Constant(value, left.Type);

            BinaryExpression comparison = op switch
            {
                ">" => Expression.GreaterThan(left, right),
                ">=" => Expression.GreaterThanOrEqual(left, right),
                "<" => Expression.LessThan(left, right),
                "<=" => Expression.LessThanOrEqual(left, right),
                "=" or "==" => Expression.Equal(left, right),
                _ => throw new ArgumentException("Invalid comparison operator")
            };

            return Expression.Lambda<Func<TEntity, bool>>(comparison, parameter);
        }

        public static IQueryable<TEntity> ApplyComparison<TEntity, TProperty>(
            this IQueryable<TEntity> query,
            TProperty? value,
            string? op,
            Expression<Func<TEntity, TProperty>> selector
        )
            where TProperty : struct, IComparable<TProperty>
        {
            if (!value.HasValue || string.IsNullOrWhiteSpace(op))
                return query;

            return query.Where(
                Build(selector, op, value.Value)
            );
        }
    }
}