using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using Microsoft.EntityFrameworkCore;

namespace Benchmarks
{
    /// <summary>
    /// BLOB열을 가진 Entity에서 ToListAsync()가 훨씬 더 느리다?
    /// https://github.com/dotnet/efcore/issues/18221
    ///     --> 개인적으로 테스트해 보면 Async()가 10배 이상 느리긴 하지만^^; 위 문서 정도의 차이는 아님
    /// </summary>
    [MinColumn, MaxColumn, HtmlExporter, RPlotExporter]
    public class ListAsync
    {
        private int id;

        public ListAsync()
        {
            using (var context = new AppDbContext())
            {
                var array = new char[5 * 1024 * 1024];
                var random = new Random();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (char)random.Next(32, 126);
                }

                var entity = new Blob { Lob = new string(array) };
                context.Blobs.Add(entity);
                context.SaveChanges();

                id = entity.Id;
            }
        }

        [Benchmark(Baseline = true)]
        public void GetWithToList()
        {
            using (var context = new AppDbContext())
            {
                var entity = context.Blobs.Where(b => b.Id == id).ToList();
            }
        }

        [Benchmark]
        public async Task GetWithToListAsync()
        {
            using (var context = new AppDbContext())
            {
                var entity = await context.Blobs.Where(b => b.Id == id).ToListAsync();
            }
        }
    }


    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=SalesSimple;Integrated Security=True;Encrypt=False;");
        }

        public DbSet<Blob> Blobs { get; set; }
    }

    public class Blob
    {
        //ID 열로
        public int Id { get; set; }
        public string Lob { get; set; }
    }

}
