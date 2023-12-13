using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class Region
{
    [Key]
    public int RegionKey { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Comment { get; set; }

    [InverseProperty("RegionKeyNavigation")]
    public virtual ICollection<Nation> Nations { get; } = new List<Nation>();
}
