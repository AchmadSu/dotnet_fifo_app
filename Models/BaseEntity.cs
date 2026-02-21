using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}