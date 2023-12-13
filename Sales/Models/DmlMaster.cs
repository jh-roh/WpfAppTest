using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[Table("DML_Master")]
public partial class DmlMaster
{
    [Key]
    public int OrderKey { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(5)]
    [Unicode(false)]
    public string? CustomerKey { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }

    [Unicode(false)]
    public string? Note { get; set; }
}
