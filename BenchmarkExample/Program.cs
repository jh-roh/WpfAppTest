using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Benchmark 테스트 
/// 
///     1. Program.Main()에서 BenchmarkRunner.Run<class_name>() 부분에 namespace.class_name 수정
///         - 테스트할 클래스에서 적용할 SQL Server에 맞게 Connection Sring 조정
///     
///     8. "시작 프로젝트" 설정
///     9. "Release" 모드에서 실행
///     
/// </summary>
namespace Benchmarks
{
    /// <summary>
    /// BechmarkRunner 패키지 소개
    /// https://benchmarkdotnet.org/articles/overview.html
    /// 
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            * 테스트 방법은 여러가지 - 1 - 테스트할 클래스 type을 아래 지정
            */
            //예제-1. 동기 vs. 비동기 비교
            //var summary = BenchmarkRunner.Run<Benchmarks.ListAsync>();

            //예제-2. Adhoc vs. Parameter 쿼리
            var summary = BenchmarkRunner.Run<Benchmarks.DynamicallyConstructedQueries>();

            /*
            * 테스트 방법은 여러가지 - 2 - 클래스 목록 중에 고르기
            */
            //예제-3. 데이터 조회 후 집계하는 방법별 비교
            //var summary = BenchmarkSwitcher.FromAssembly(typeof(Benchmarks.AverageBlogRanking).Assembly).Run();

        }
    }
}