using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Reflection;
using System.Data.Common;   //DbCommandInterceptor용으로 추가
using System.Text.RegularExpressions;

namespace Sales.Models;

public partial class SalesContext : DbContext
{
    public SalesContext()
    {
        //LazyLoadingEnabled
        //this.ChangeTracker.LazyLoadingEnabled = false;

        //CommandInterceptorForHint, Static 변수 초기화(예제로 1개 변수만)
        CommandInterceptorForHint.UseHintNOLOCK = false;
    }

    public SalesContext(DbContextOptions<SalesContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Blob> Blobs { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DmlMaster> DmlMasters { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartsSupp> PartsSupps { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    /// <summary>
    /// 명시적으로 컴파일된 쿼리 생성 (Delegate)
    /// getCustomerById() 
    /// </summary>
    public static Func<SalesContext, int, IAsyncEnumerable<Customer>> getCustByKey =
        EF.CompileAsyncQuery((SalesContext db, int custkey)
            => db
            .Customers
            .Include(c => c.Orders)
            .Where(c => c.CustKey == custkey));


    // 자동 생성되지 않아서 추가한 코드
    // A) 사용자 정의 테이블 값 함수
    public IQueryable<Order> GetOrdersList(int OrderKey)
        => FromExpression(() => GetOrdersList(OrderKey));

    // B) 사용자 정의 스칼라 함수
    public int PartsCountforSupplier(int SuppKey)
        => throw new NotSupportedException();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 개별 context 인스턴스에서 Lambda/Linq 사용 시 호출
        if (!optionsBuilder.IsConfigured)
        {
            //Demo 편의상 직접 지정
            string connectionString = "Data Source=localhost;Initial Catalog=SalesSimple;Integrated Security=True;Pooling=True;MultipleActiveResultSets=True;Application Name=Sales;Encrypt=False;";

            optionsBuilder
                .UseLazyLoadingProxies(true)  //테스트를 위해 디폴트로 True 적용
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseSqlServer(connectionString, sqloptions =>
                {
                    sqloptions.CommandTimeout(40);
                    //sqloptions.MaxBatchSize(42);
                    //sqloptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    //sqloptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                })
                .AddInterceptors(new CommandInterceptorForHint())   //테스트용
#if DEBUG
                //Logging
                .LogTo(
                      Console.WriteLine
                    , LogLevel.Information	//Information < Debug(connection, transaction 등 정보까지) < Trace
                    , DbContextLoggerOptions.DefaultWithLocalTime
                    )
                
                //쿼리 매개변수 정보도 포함하고 싶은 경우
                //  주의) 아래 옵션은 운영에서 사용하지 말 것(개발 또는 테스팅 환경)
                .EnableSensitiveDataLogging(true)
                
                // 필드 수준의 상세한 오류 정보
                //.EnableDetailedErrors();  
#endif
                ;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 자동 생성되지 않아서 추가한 코드
        modelBuilder
            .HasDbFunction(typeof(SalesContext).GetMethod(nameof(GetOrdersList), new[] { typeof(int) }))
            //.HasName("GetOrdersList") //참고. 여기에는 실제 함수명 (다른 경우)
            ;

        modelBuilder
            .HasDbFunction(typeof(SalesContext).GetMethod(nameof(PartsCountforSupplier), new[] { typeof(int) }))
            ;
        
        modelBuilder.Entity<Blob>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

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
            entity.Property(e => e.OrderKey).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.Property(e => e.LineStatus).IsFixedLength();
            entity.Property(e => e.ReturnFlag).IsFixedLength();
            entity.Property(e => e.ShipMode).IsFixedLength();
            entity.Property(e => e.ShipinStruct).IsFixedLength();

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

}//class, SalesContext




/*
 * 주의. 아래 예제 코드는 단순 학습 및 테스트용임, 운영에서 사용 금지
 */
public partial class CommandInterceptorForHint : DbCommandInterceptor
{
    /// <summary>
    /// 구글로 찾아본 DbCommandInterceptor를 이용한 쿼리 실행 전 변경 방법
    /// (ex, NOLOCK 힌트, 쿼리 힌트 추가 등)
    /// </summary>
    /// <remarks>
    /// - 아래 Regex 식에 대해서는 아직 완전히 보장 못함 @ 2023.01.03
    /// - 아래 코드는 단순 학습 및 테스트용임, 운영에서 사용 금지
    /// </remarks>    
    private static readonly Regex _tableAliasRegex =
        new Regex(@"(?<tableAlias>((FROM)|(JOIN))\s\[([^\s]+)\]\sAS\s\[([^\s]+)\](?!\sWITH\s \(NOLOCK\)))",
            RegexOptions.Multiline | RegexOptions.IgnoreCase);

    //[ThreadStatic]
    public static bool UseHintNOLOCK;
    //[ThreadStatic]
    public static bool UseHintRECOMPILE;

    //아래는 비동기 처리 기준 함수 예제
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        if (UseHintNOLOCK)
        {
            // Ex-A. Table 힌트 추가 (기존 SQL문 업데이트)
            command.CommandText = _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
        }
        
        if (UseHintRECOMPILE)
        {
            // Ex-B. 쿼리 힌트 추가 시 (SQL문 뒤에 추가)
            command.CommandText += "\n\tOPTION(RECOMPILE)";
        }

        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}


