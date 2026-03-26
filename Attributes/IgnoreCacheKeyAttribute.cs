using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreCacheKeyAttribute : Attribute
    {
    }
}