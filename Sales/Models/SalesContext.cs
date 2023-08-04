using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sales.Models;

public partial class SalesContext : DbContext
{
    public SalesContext()
    {
    }

    public SalesContext(DbContextOptions<SalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blob> Blobs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DmlMaster> DmlMasters { get; set; }

    public virtual DbSet<DmlMaster2> DmlMaster2s { get; set; }

    public virtual DbSet<Imtr> Imtrs { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartsSupp> PartsSupps { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=SalesSimple;Integrated Security=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustKey).ValueGeneratedNever();
            entity.Property(e => e.MktSegment).IsFixedLength();
            entity.Property(e => e.Phone).IsFixedLength();
        });

        modelBuilder.Entity<DmlMaster>(entity =>
        {
            entity.Property(e => e.CustomerKey).IsFixedLength();
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<DmlMaster2>(entity =>
        {
            entity.Property(e => e.OrderKey).ValueGeneratedNever();
            entity.Property(e => e.CustomerKey).IsFixedLength();
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.Property(e => e.LineStatus).IsFixedLength();
            entity.Property(e => e.ReturnFlag).IsFixedLength();
            entity.Property(e => e.ShipMode).IsFixedLength();
            entity.Property(e => e.ShipinStruct).IsFixedLength();

            entity.HasOne(d => d.OrderKeyNavigation).WithMany(p => p.LineItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItems_Orders");

            entity.HasOne(d => d.PartsSupp).WithMany(p => p.LineItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItems_PartsSupps");
        });

        modelBuilder.Entity<Nation>(entity =>
        {
            entity.Property(e => e.NationKey).ValueGeneratedNever();
            entity.Property(e => e.Name).IsFixedLength();

            entity.HasOne(d => d.RegionKeyNavigation).WithMany(p => p.Nations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Nations_Regions");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderKey).IsClustered(false);

            entity.HasIndex(e => e.OrderDate, "CL_Orders").IsClustered();

            entity.Property(e => e.OrderKey).ValueGeneratedNever();
            entity.Property(e => e.Clerk).IsFixedLength();
            entity.Property(e => e.OrderPriority).IsFixedLength();
            entity.Property(e => e.OrderStatus).IsFixedLength();

            entity.HasOne(d => d.CustKeyNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.Property(e => e.PartKey).ValueGeneratedNever();
            entity.Property(e => e.Brand).IsFixedLength();
            entity.Property(e => e.Container).IsFixedLength();
            entity.Property(e => e.Mfgr).IsFixedLength();
        });

        modelBuilder.Entity<PartsSupp>(entity =>
        {
            entity.HasKey(e => new { e.PartKey, e.SuppKey }).HasName("PK_PartSupps");

            entity.HasOne(d => d.PartKeyNavigation).WithMany(p => p.PartsSupps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartsSupps_Parts");

            entity.HasOne(d => d.SuppKeyNavigation).WithMany(p => p.PartsSupps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartsSupps_Suppliers");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.Property(e => e.RegionKey).ValueGeneratedNever();
            entity.Property(e => e.Name).IsFixedLength();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.Suppkey).ValueGeneratedNever();
            entity.Property(e => e.Name).IsFixedLength();
            entity.Property(e => e.Phone).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
