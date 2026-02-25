using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FifoApi.Helpers
{
    public static class DynamicFilterBuilder
    {
        public static IQueryable<TEntity> ApplyFilters<TEntity>(
            IQueryable<TEntity> query,
            object queryObject
        )
        {
            var entityType = typeof(TEntity);
            var dtoType = queryObject.GetType();

            foreach (var prop in dtoType.GetProperties())
            {
                var value = prop.GetValue(queryObject);
                if (value == null) continue;

                var propType = prop.PropertyType;
                if (prop.Name.EndsWith("From"))
                {
                    var baseName = prop.Name.Replace("From", "");
                    var toProp = dtoType.GetProperty(baseName + "To");
                    if (toProp == null) continue;

                    var toValue = toProp.GetValue(queryObject);
                    if (toValue == null) continue;

                    if (DateHelper.IsDateType(propType))
                    {
                        query = ApplyBetweenDate<TEntity>(
                            query,
                            baseName,
                            (DateTime)value,
                            (DateTime)toValue
                        );
                    }
                    else
                    {
                        query = ApplyBetween<TEntity>(
                            query,
                            baseName,
                            value,
                            toValue
                        );
                    }

                }

                else if (!prop.Name.EndsWith("Op") && !prop.Name.EndsWith("To"))
                {
                    var opProp = dtoType.GetProperty(prop.Name + "Op");
                    if (opProp == null) continue;

                    var op = opProp.GetValue(queryObject)?.ToString();
                    if (string.IsNullOrWhiteSpace(op)) continue;

                    if (DateHelper.IsDateType(propType))
                    {
                        query = ApplyComparisonDate<TEntity>(
                            query,
                            prop.Name,
                            op,
                            (DateTime)value
                        );
                    }
                    else
                    {
                        query = ApplyComparison<TEntity>(
                            query,
                            prop.Name,
                            op,
                            value
                        );
                    }
                }
            }
            return query;
        }

        private static IQueryable<TEntity> ApplyComparison<TEntity>(
            IQueryable<TEntity> query,
            string propertyName,
            string op,
            object value
        )
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value);

            BinaryExpression body = op switch
            {
                ">" => Expression.GreaterThan(member, constant),
                ">=" => Expression.GreaterThanOrEqual(member, constant),
                "<" => Expression.LessThan(member, constant),
                "<=" => Expression.LessThanOrEqual(member, constant),
                "=" => Expression.Equal(member, constant),
                _ => throw new Exception($"Invalid operator {op}")
            };

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            return query.Where(lambda);
        }

        private static IQueryable<TEntity> ApplyBetween<TEntity>(
            IQueryable<TEntity> query,
            string propertyName,
            object from,
            object to
        )
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.Property(parameter, propertyName);

            var lower = Expression.GreaterThanOrEqual(
                member,
                Expression.Constant(from)
            );

            var upper = Expression.LessThanOrEqual(
                member,
                Expression.Constant(to)
            );

            var body = Expression.AndAlso(lower, upper);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

            return query.Where(lambda);
        }

        private static IQueryable<TEntity> ApplyComparisonDate<TEntity>(
            IQueryable<TEntity> query,
            string propertyName,
            string op,
            DateTime value
        )
        {
            var utcValue = EnsureUtc(value);
            var param = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(param, propertyName);
            var constant = Expression.Constant(utcValue, typeof(DateTime));

            BinaryExpression comparison = op switch
            {
                ">" => Expression.GreaterThan(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<" => Expression.LessThan(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                "==" => Expression.Equal(property, constant),
                _ => throw new Exception($"Invalid operator: {op}")
            };

            var lambda = Expression.Lambda<Func<TEntity, bool>>(comparison, param);
            return query.Where(lambda);
        }

        private static IQueryable<TEntity> ApplyBetweenDate<TEntity>(
            IQueryable<TEntity> query,
            string propertyName,
            DateTime from,
            DateTime to
        )
        {
            var utcFrom = EnsureUtc(from.Date);
            var utcTo = EnsureUtc(to.Date.AddDays(1).AddTicks(-1));
            var param = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(param, propertyName);

            var start = Expression.Constant(utcFrom, typeof(DateTime));
            var end = Expression.Constant(utcTo, typeof(DateTime));

            var greaterThanOrEqual = Expression.GreaterThanOrEqual(property, start);
            var lessThanOrEqual = Expression.LessThanOrEqual(property, end);

            var body = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            return query.Where(lambda);
        }

        private static DateTime EnsureUtc(DateTime dateTime)
        {
            return dateTime.Kind switch
            {
                DateTimeKind.Utc => dateTime,
                DateTimeKind.Local => dateTime.ToUniversalTime(),
                DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
                _ => dateTime
            };
        }
    }
}