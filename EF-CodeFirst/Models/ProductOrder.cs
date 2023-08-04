using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.Models
{
    [Index(nameof(OrderID), nameof(ProductID), IsUnique = true, IsDescending = new[] {true, false})]
    public class ProductOrder
    {
        public Int64 ProductOrderID { get; set; }
        public int Quantity { get; set; }

        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
