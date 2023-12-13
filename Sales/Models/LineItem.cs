using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[PrimaryKey("OrderKey", "LineNumber")]
public partial class LineItem
{
    [Key]
    public int OrderKey { get; set; }

    public int PartKey { get; set; }

    public int SuppKey { get; set; }

    [Key]
    public int LineNumber { get; set; }

    [Column(TypeName = "decimal(15, 2)")]

    [ConcurrencyCheck]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal ExtendedPrice { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Tax { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string ReturnFlag { get; set; } = null!;

    [StringLength(2)]
    [Unicode(false)]
    public string LineStatus { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ShipDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CommitDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ShipinStruct { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string ShipMode { get; set; } = null!;

    [StringLength(90)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [ForeignKey("PartKey, SuppKey")]
    [InverseProperty("LineItems")]
    public virtual PartsSupp PartsSupp { get; set; } = null!;

    [ForeignKey("OrderKey")]
    [InverseProperty("LineItems")]
    public virtual Order OrderKeyNavigation { get; set; } = null!;
}
