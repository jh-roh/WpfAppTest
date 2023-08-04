using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class Blob
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    public string Lob { get; set; } = null!;
}
