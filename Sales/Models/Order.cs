using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[Index("CustKey", Name = "IX_Orders_CustKey")]
public partial class Order
{
    [Key]
    public int OrderKey { get; set; }

    public int CustKey { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string OrderStatus { get; set; } = null!;

    //[ConcurrencyCheck]
    [Column(TypeName = "decimal(15, 2)")]
    public decimal TotalPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string OrderPriority { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Clerk { get; set; } = null!;

    public int ShipPriority { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [ForeignKey("CustKey")]
    [InverseProperty("Orders")]
    public virtual Customer CustKeyNavigation { get; set; } = null!;

    [InverseProperty("OrderKeyNavigation")]
    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();
}
