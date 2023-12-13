using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

[Index("NationKey", "AcctBal", Name = "IX_C_A")]
public partial class Customer
{
    [Key]
    public int CustKey { get; set; }

    [StringLength(50)]
    //실제 Name 열은 varchar() 형, 테스트를 위해 UNICODE로 설정
    //[Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(80)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    public int NationKey { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal AcctBal { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string MktSegment { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string Comment { get; set; } = null!;

    [InverseProperty("CustKeyNavigation")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
