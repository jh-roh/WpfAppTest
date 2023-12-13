using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[Keyless]
public partial class Blob
{
    public int Id { get; set; }

    [Unicode(false)]
    public string Lob { get; set; } = null!;
}
