using EF_CodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EFCodeFirst;Integrated Security=True;Encrypt=False;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderFulfilled)
                .IncludeProperties(o => new { o.OrderID, o.CustomerID });
        }
    }
}


/*
 *
 Add-Migration InitialCreate
 Update-Database
 * */


