using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Helpers.SaleHelper
{
    public static class InvoiceNoGenerator
    {
        public static string Generate(
            int sequenceNumber,
            string prefix = "INV"
        )
        {
            return $"{prefix}-{DateTime.UtcNow:yyyyMM}-{sequenceNumber:D6}";
        }
    }
}