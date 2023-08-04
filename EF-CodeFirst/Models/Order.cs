using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.Models
{
    public class Order
    {
        //[Index("PK_Orders", IsUnique = true)]
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderPlaced { get; set; }
        [Column(TypeName = "date")]
        public DateTime? OrderFulfilled { get; set; }
        public int CustomerID { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<ProductOrder>? ProductOrders { get; set; }
    }
}
