using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class Supplier
{
    [Key]
    public int Suppkey { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(80)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    public int Nationkey { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal AcctBal { get; set; }

    [StringLength(202)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [InverseProperty("SuppKeyNavigation")]
    public virtual ICollection<PartsSupp> PartsSupps { get; } = new List<PartsSupp>();
}
