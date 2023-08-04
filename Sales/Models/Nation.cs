using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class Nation
{
    [Key]
    public int NationKey { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public int RegionKey { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Comment { get; set; }

    [ForeignKey("RegionKey")]
    [InverseProperty("Nations")]
    public virtual Region RegionKeyNavigation { get; set; } = null!;
}
