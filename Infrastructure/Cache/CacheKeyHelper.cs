using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FifoApi.Attributes;

namespace FifoApi.Infrastructure.Cache
{
    public static class CacheKeyHelper
    {
        public static string GenerateListKey(string prefix, object queryObject)
        {
            if (queryObject == null)
                return $"{prefix}:list";

            var properties = queryObject
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    !Attribute.IsDefined(p, typeof(IgnoreCacheKeyAttribute))
                )
                .OrderBy(p => p.Name);

            var builder = new StringBuilder($"{prefix}:list");

            foreach (var prop in properties)
            {
                var value = prop.GetValue(queryObject);

                builder.Append($":{prop.Name}:{value ?? "null"}");
            }

            return builder.ToString();
        }
    }
}