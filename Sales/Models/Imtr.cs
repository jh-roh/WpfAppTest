using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[Keyless]
[Table("imtr")]
public partial class Imtr
{
    [Column("code")]
    public int? Code { get; set; }
}
