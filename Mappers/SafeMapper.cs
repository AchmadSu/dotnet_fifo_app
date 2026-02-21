using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FifoApi.Mappers
{
    public static class SafeMapper
    {
        public static void Map<TDto, TModel>(TDto dto, TModel model)
        {
            if (dto == null || model == null) return;

            var dtoProperties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            foreach (var prop in dtoProperties)
            {
                if (modelProperties.TryGetValue(prop.Name, out var modelProp))
                {
                    var value = prop.GetValue(dto);
                    if (value == null) continue;
                    if (value is string str && string.IsNullOrWhiteSpace(str)) continue;
                    modelProp.SetValue(model, value);
                }
            }
        }
    }
}