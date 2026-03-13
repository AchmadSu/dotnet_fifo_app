using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Validations;

namespace FifoApi.Helpers.SaleHelper
{
    public class SaleQueryObject : QueryObject
    {
        public DateTime? SaleDate { get; set; }

        [ComparisonOperator]
        public string? SaleDateOp { get; set; }

        public decimal? TotalAmount { get; set; }

        [ComparisonOperator]
        public string? TotalAmountOp { get; set; }

        public decimal? TotalAmountFrom { get; set; }
        public decimal? TotalAmountTo { get; set; }
        public DateTime? SaleDateFrom { get; set; }
        public DateTime? SaleDateTo { get; set; }

    }
}