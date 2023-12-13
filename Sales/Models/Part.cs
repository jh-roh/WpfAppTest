using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class Part
{
    [Key]
    public int PartKey { get; set; }

    [StringLength(110)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Mfgr { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Brand { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Type { get; set; } = null!;

    public int Size { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Container { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal RetailPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [InverseProperty("PartKeyNavigation")]
    public virtual ICollection<PartsSupp> PartsSupps { get; } = new List<PartsSupp>();
}
