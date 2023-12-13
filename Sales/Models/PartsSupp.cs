using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[PrimaryKey("PartKey", "SuppKey")]
public partial class PartsSupp
{
    [Key]
    public int PartKey { get; set; }

    [Key]
    public int SuppKey { get; set; }

    public int AvailQty { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal SupplyCost { get; set; }

    [StringLength(400)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [InverseProperty("PartsSupp")]
    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();

    [ForeignKey("PartKey")]
    [InverseProperty("PartsSupps")]
    public virtual Part PartKeyNavigation { get; set; } = null!;

    [ForeignKey("SuppKey")]
    [InverseProperty("PartsSupps")]
    public virtual Supplier SuppKeyNavigation { get; set; } = null!;
}
