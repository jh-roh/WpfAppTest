using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
