using System.Data;
//using System.Diagnostics;
using System.Transactions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

using Sales.Models;
using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Reflection;

namespace Sales
{
    /// <summary>
    /// 아래 static Program은 ToListWithNoLockAsync() 정의 때문
    /// </summary>
    internal static class Program
    {
        static IEnumerable<Order> GetOrdersEnumable()
        {
            return (new SalesContext()).Orders;
        }
        static IQueryable<Order> GetOrdersQueryable()
        {
            return (new SalesContext()).Orders;
        }

        private static async Task Main(string[] args)
        {

            //// Hellow EF Core, 쿼리 테스트 
            //await HellowEFCore();

            /// <summary>
            /// DbContextPool
            /// </summary>
            /// <remarks>
            /// 아래는 Console 앱이라 코드 예제 목적으로 만듬
            /// </remarks>
            var services = new ServiceCollection(); // ConfigureServices()에서는 매개변수로 제공

            //Demo 편의상 직접 지정
            var connectionString = "Data Source=localhost;Initial Catalog=SalesSimple;Integrated Security=True;Application Name=Sales;Encrypt=False;";

            services.AddDbContextPool<SalesContext>(options => options
                .UseSqlServer(connectionString)
                    , poolSize: 1024
            );



            /*=====================================================================
             * DB Connection
             *===================================================================*/

            // DB 연결 Open/Close 시점 확인
            //await ConnectionOpenCloseAsync();



            /*=====================================================================
             * 조회 1부
             *===================================================================*/

            // 동기/비동기 성능 비교, 아래는 각각 개별적으로 두 번 이상 실행 후 비교
            //var i_s = 1; while (i_s++ <= 12) ComparetoSync();
            //var i_a = 1; while (i_a++ <= 12) await ComparetoASync();

            // Adhoc vs. Parameter Query (sp_executesql)
            await AdhocOrParameterQuery();

            // .ToQueryString()
            //ToQueryString();

            // AsNoTracking 함수 사용 여부에 따라 비교할 것
            //await AsNoTracking();

            // 필요한 열만 SELECT 
            //await SelectColumns();

            // IQueryable vs. IEnumerable (쿼리 시점 차이)
            //await IQueryableEnumerable_1();

            // DB 튜닝과는 무관하나 후속 내용 이해를 위해
            // IQueryable vs. IEnumerable (Entity 메모리 버퍼링 시점 차이)
            //await IQueryableEnumerable_2();

            // DB 튜닝과는 무관하므로 생략
            // IEnumerable 후 Streaming 필요한 경우 .AsEnumerable()
            //StreamingAfterBuffering();

            // Where() - .NET or DB에서 행 필터 처리
            //SelectRows();

            // Find, FindAsync
            //await FindOrFindAsync();

            // First vs. FirstOrDefault (혹은 Single() vs. SingleOrDefault())
            //await FirstOrFirstDefault();

            // FirstOrDefault (혹은 SingleOrDefault) 와 서브쿼리
            //await FirstDefaultWithSubquery();

            // SingleOrDefault()
            //await SingleOrDefault();

            // IN 조건으로 구현
            //await InPredicate();

            // STRING_AGG() 호출
            //await STRING_AGG();



            /*=====================================================================
             * 조회 2부
             *===================================================================*/
            // CROSS JOIN
            //await CrossJoin();

            // INNER JOIN
            //await InnerJoin();

            // OUTER JOIN
            //await OuterJoin();

            // APPLY
            //await OuterApply();

            // .Include 주의
            //await IncludeThenInclude();

            // .Include 및 분할 쿼리(EF Core 5+)
            //await IncludeSplitQuery();

            // EF Core Internal Buffering (DB 튜닝과 무관하나 Split Query, MARS 등 이해를 위해)
            //InternalBuffering();

            // Eager Lodding vs. Lazy Lodding
            //await LazyLoading();
            //await EagerLoading();

            // 불필요한 반복 쿼리(반복 호출, 반복 IO) 
            //await AggregateRepeat();

            //집합 연산 그리고 UNION ALL
            //await SetOpAndUnionAll();
            
            

            /*=====================================================================
             * 조회 3부
             *===================================================================*/
            // Non SARG - Column에 연산
            //await NonSARG_1();

            // Non SARG - Column에 함수 적용
            //NonSARG_2();

            // Non SARG - Column보다 Expression에 더 큰 데이터 형식
            //await NonSARG_3();

            // Non SARG - LIKE 검색 주의
            //await NonSARG_4();

            // Non SARG - 열 간 비교
            //await NonSARG_5();



            /*=====================================================================
             * 고급 쿼리 및 기타
             *===================================================================*/

            // Skip() + Take() 사용 시 Parameter 화 여부 
            //await SkipTake();

            // Paging at .NET or DB 
            //await Paging();

            // 동적 검색 조건 (만능 조회)
            //await DynamicSearching();

            // CompileQuery() or CompileAsyncQuery()
            //await CompileQuery();

            // 비동기 처리 취소, DB 성능 튜닝 주제는 아니지만
            //await CancelAsync();

            // Tag를 통한 쿼리 주석 
            //await Tag();



            /*=====================================================================
             * 쿼리 직접 호출
             *===================================================================*/
            // FromSql vs. FromSqlRaw
            //await FromSqlOrFromSqlRaw();

            // FromSql 에서 매개변수 개체 지정
            //await FromSqlParameter();

            // SQL Sever의 Table Valued Function을 EF에서 사용하기
            //await TVF();

            // 스칼라형(Non-Entity) 반환 쿼리
            //await SqlQuery();

            // Entity 기준 저장 프로시저 호출
            //await StoredProcedureByEntity();

            // 조회용(Non-Entity) 저장 프로시저 호출 by Native SQLClient
            //await StoredProcedureByComplexQuery();

            // 다중 결과 집합 반환 프로시저 호출 by Native SQLClient
            //await StoredProcedureByMultiResultsets();



            /*=====================================================================
             * DML
             *===================================================================*/
            // 1건 INSERT
            //await InsertOneOrder();

            // N건 INSERT
            //await InsertOrders();

            // 대량 데이터 변경과 배치처리
            // InsertOrders() 이용해서 데모

            //---------------------------------------------
            // EF Core 6 vs. 7 (어떤 개선 있을까?)
            //---------------------------------------------

            // 1건 UPDATE
            //await UpdateOneOrder();

            // N건 UPDATE
            //await UpdateOrders();

            //PK(UQ)가 아닌 조건으로 검색 후 UPDATE한 경우
            //await UpdateOneOrdersWithNonKey();

            // 1건 DELETE
            //await DeleteOneOrder();

            // N건 DELETE
            //await DeleteOrders();

            /*
             * Bulk Updates (EF Core 7)
             */
            //
            //await ExecuteDelete();
            //await ExecuteUpdate();

            // DELETE(또는 UPDATE) 문 직접 사용
            //await DeleteRawSQL();



            /*=====================================================================
             * 트랜잭션과 잠금
             *===================================================================*/
            // 로컬 트랜잭션, 격리수준 확인, Commit/Rollback/SavePoint 확인
            //await Transaction_Async_Default();

            // UPDATE 시 SELECT 잠금 대기 확인
            //await Transaction_Async_UPDATE_Lock();

            // SELECT 시 UPDATE 잠금 대기 확인
            //await Transaction_Async_SELECT_Lock_WithReadCommitted();

            // RepeatableRead 격리수준에서 SELECT 시 UPDATE 잠금 대기 확인
            //await Transaction_Async_SELECT_Lock_WithRepeatableRead();


            // Commit/Rollback 되지 않는 경우의 상태 확인
            //await Transaction_Async_WithoutCommit_Using();
            //await Transaction_Async_WithoutCommit_NonDispose();


            // READ UNCOMMITTED or READ COMMITTED SNAPSHOT
            //await ReadUncommitted();
            //await ReadUncommitted2();

            // RCSI
            //await ReadCommittedSnapshot();


            /*=====================================================================
             * 분산 트랜잭션
             *===================================================================*/
            // 분산 트랜잭션, System.Transactions.TransactionScope()
            //await DistributedTransaction_Async_Default();

            // 분산 트랜잭션, 격리수준 조정
            //await DistributedTransaction_Async_IsolationLevel();

            // 분산 트랜잭션, Commit / Rollback 시점 확인
            //await DistributedTransaction_Async_Timing();



            /*=====================================================================
             * 부록
             *===================================================================*/

            //사용자 정의 함수와 EF Core 모델
            //await ScalarUDFs();

            // 동시성 충돌(Concurrency Conflicts), Update 작업
            //   참고. Test 시나리오: [ConcurrencyCheck] 설정 여부에 따라 예외 발생 여부 비교
            //await Transaction_Async_ConcurrencyConflictsWithUpate();   //참고) 함수 내에서 orderkey 조정 필요           

            // 동시성 충돌(Concurrency Conflicts), Delete 작업
            //await Transaction_Async_ConcurrencyConflictsWithDelete();  //주의) 함수 내에서 orderkey, linenumber 조정 필요           

            //DbCommandInterceptor 예제
            //await QueryIntercept();

        } // Main



        /// <summary>
        /// EFCore 쿼리 테스트 
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        static async Task HellowEFCore()
        {
            var custkey = 10954;
            
            using SalesContext db = new SalesContext();

            var Orders = await db.Orders
                .Where(o => o.CustKey == custkey)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync()
                ;

            foreach (Order o in Orders)
            {
                Console.WriteLine($"OrderKey:    {o.OrderKey}");
                Console.WriteLine($"OrderDate:   {o.OrderDate}");
                Console.WriteLine($"TotalPrice:  {o.TotalPrice}");
                Console.WriteLine(new string('-', 20));
            }
        }


        /// <summary>
        /// DB 연결 Open/Close 시점 확인
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        static async Task ConnectionOpenCloseAsync()
        {
            var p_price = 2095.0m;

            //첫 번째 연결
            using SalesContext db = new SalesContext();

            var Parts = await db.Parts
                .Where(p => p.RetailPrice >= p_price)
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync();

            foreach (Part p in Parts)
            {
                Console.WriteLine($"PatKey: {p.PartKey}, Name: {p.Name}, Price: {p.RetailPrice}");
                Console.WriteLine(new string('-', 20));
            }
            Console.WriteLine("Enter any key to continue..."); Console.ReadKey();


            //두 번째 연결, 일부러 using() 형식 사용
            using (SalesContext db2 = new SalesContext())
            {
                Parts = await db2.Parts
                    .Where(p => p.RetailPrice >= p_price)
                    .OrderBy(p => p.Name)
                    .AsNoTracking()
                    .ToListAsync();

                //명시적 Open 가능?
                //db2.Database.OpenConnection();

                foreach (Part p in Parts)
                {
                    Console.WriteLine($"PatKey: {p.PartKey}, Name: {p.Name}, Price: {p.RetailPrice}");
                    Console.WriteLine(new string('-', 20));
                }
            }
            Console.WriteLine("Enter any key to continue..."); Console.ReadKey();


            //세 번째 연결
            using (SalesContext db3 = new SalesContext())
            {
                Parts = await db3.Parts
                        .Where(p => p.RetailPrice >= p_price)
                        .OrderBy(p => p.Name)
                        .AsNoTracking()
                        .ToListAsync();

                foreach (Part p in Parts)
                {
                    Console.WriteLine($"PatKey: {p.PartKey}, Name: {p.Name}, Price: {p.RetailPrice}");
                    Console.WriteLine(new string('-', 20));

                    //명시적인 Close 가능?
                    if (p.PartKey == 199999)
                        await db3.Database.CloseConnectionAsync();
                }
            }
        
        //여기에 중단점 설정 후 확인
        }//()



        
        

        
        /// <summary>
        /// Sync() vs. Async() 성능 비교용
        /// <remarks>Sync 예제</remarks>
        /// </summary>
        static void ComparetoSync()
        {
            var p_orderkey = 1;

            using SalesContext db = new SalesContext();

            //분할 쿼리 사용 전
            var lineitems = db.LineItems
                .TagWith("Sync")
                .AsNoTracking()
                .Where(li => li.OrderKey > p_orderkey)
                .Take(200000)
                .ToList()
                ;

            foreach (var li in lineitems)
            {
                //Console.WriteLine($"{li.OrderKey}, {li.PartKey}");
            }
            Console.WriteLine($"# of Rows returned ... {lineitems.Count}");
        }

        /// <summary>
        /// Sync() vs. Async() 성능 비교용
        /// <remarks>Async 예제</remarks>
        /// </summary>
        static async Task ComparetoASync()
        {
            var p_orderkey = 1;

            using SalesContext db = new SalesContext();

            //분할 쿼리 사용 전
            var lineitems = await db.LineItems
                .TagWith("Async")
                .AsNoTracking()
                .Where(li => li.OrderKey > p_orderkey)
                .Take(200000)
                .ToListAsync()
                ;

            foreach (var li in lineitems)
            {
                //Console.WriteLine($"{li.OrderKey}, {li.PartKey}");
            }
            Console.WriteLine($"# of Rows returned ... {lineitems.Count}");
        }


        /// <summary>
        /// Adhoc vs. Parameter Query (sp_executesql)
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        static async Task AdhocOrParameterQuery()
        {
            using (SalesContext db = new SalesContext())
            {
                string p_phone = "30-114-968-4951";

                var customers = await db.Customers
                    .AsNoTracking()
                    // 1 - Adhoc
                    //.Where(c => c.Phone == "30-114-968-4951")
                    // 2 - Parameter
                    .Where(c => c.Phone == p_phone)
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
        }


        /// <summary>
        /// .ToQueryString() - IQueryable 객체로 생성되는 쿼리 확인 (EF Core v5+)
        /// </summary>
        /// <remarks>
        /// 구문 형식과 변수값(실제론 매개변수)를 확인
        /// 후속으로 비동기 실행은 안된다?
        /// </remarks>
        static void ToQueryString()
        {
            using SalesContext db = new SalesContext();

            var p_price = 1880.0m;

            var parts = db
                .Parts
                .Where(p => p.RetailPrice >= p_price)
                .OrderByDescending(p => p.Name)
                .Take(2)
                ;
#if DEBUG
            //쿼리 확인
            var sql = parts.ToQueryString();
            Console.WriteLine($"{sql}\n");
#endif
            //실제 실행
            var result = parts.ToList();
        }


        /// <summary>
        /// .AsNoTracking() - EF Core v2+
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        static async Task AsNoTracking()
        {
            var p_orderkey = 1;

            // 쿼리 단위
            using SalesContext db = new SalesContext();

            // 메모리 사용량에 따른 비교를 위해, 대량 데이터 검색
            var lineitems = await db.LineItems
                .AsNoTracking()     // AsTracking() or AsNoTracking()
                .Where(li => li.OrderKey > p_orderkey)
                .Take(200000)
                .ToListAsync()
                ;

            // 참고. Context 인스턴스 단위
            //db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //lineitems = await db.LineItems.Where(li => li.OrderKey > p_orderkey).Take(200000).ToListAsync();

        }


        /// <summary>
        /// 필요한 열만 SELECT 
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        static async Task SelectColumns()
        {
            using SalesContext db = new SalesContext();

            //All - 불필요한 열까지 모두 선택
            string p_phone = "30-114-968-4951";

            var customers = await db.Customers
                .Where(c => c.Phone == p_phone)
                .AsNoTracking()
                .ToListAsync();

            //Projection - 특정 열만 선택
            var customers2 = await db.Customers
                .Where(c => c.Phone == p_phone)
                .Select(c => new { c.CustKey, c.Name })
                .AsNoTracking()
                .ToListAsync();

        }


        /// <summary>
        /// IQueryable vs. IEnumerable - 1/2
        /// </summary>
        /// <remarks>
        /// LINQ 결과에 추가 LINQ 함수 사용 시 선택에 따른 쿼리 성능 차이
        /// </remarks>
        static async Task IQueryableEnumerable_1()
        {
            //IQueryable 인터페이스 특성
            int? p_orderkey = 100;

            var orders_q = GetOrdersQueryable();

            if (p_orderkey.HasValue)
            {
                orders_q = orders_q.Where(o => o.OrderKey == p_orderkey);
            }
            Console.WriteLine($"RowCount is ... {orders_q.Count()}");


            //IEnumerable 인터페이스 특성
            var orders_e = GetOrdersEnumable()
                .Where(o => o.OrderKey == p_orderkey);
            Console.WriteLine($"RowCount is ... {orders_e.Count()}");
        }


        /// <summary>
        /// IQueryable vs. IEnumerable - 2 
        /// </summary>
        /// <remarks>
        /// 일명,  Buffering vs. Streaming
        /// </remarks>
        static async Task IQueryableEnumerable_2()
        {
            //Buffering: ToList()로 결과를 모두 메모리 올려두고 접근
            using SalesContext db = new SalesContext();
            var parts_e = await db
                .Parts
                .Where(p => p.PartKey <= 3)
                .AsNoTracking()
                .ToListAsync();

            foreach (Part p in parts_e)
            {
                Console.WriteLine($"{p.Name}");
            }

            //Streaming: IQueryable에서 개체를 반복 참조(part 생성자 반복 호출)
            var parts_q = db
                .Parts
                .Where(p => p.PartKey <= 3)
                .AsNoTracking()
                ;

            // 여기서 Debugging(F11), 참조 시마다 Part Entity 생성됨을 확인
            foreach (Part p in parts_q)
            {
                Console.WriteLine($"{p.Name}");
            }

        }


        /// <summary>
        /// IEnumerable 후 Streaming 필요한 경우
        /// </summary>
        /// <remarks>
        /// Buffering이 된 결과에서 다시 쿼리를 하되 + Stream으로 처리하고 싶다면
        /// ToLis(), ToArray() 대신 AsEnumerable() 사용 가능
        /// </remarks>    
        static void StreamingAfterBuffering()
        {
            //ToList() + Where() 사용 시 -> Buffering
            using SalesContext db = new SalesContext();
            var parts = db
                .Parts
                .Where(p => p.PartKey <= 3)
                .ToList()
                .Where(p => p.Name.StartsWith("D"))
                .ToList()
                ;

            foreach (Part p in parts)
            {
                Console.WriteLine($"{p.Name}");
            }

            //AsEnumerable() + Where() 사용 시 --> Streaming
            var parts2 = db
                .Parts
                .Where(p => p.PartKey <= 3)
                .AsEnumerable()                 //AsAsyncEnumerable
                .Where(p => p.Name.StartsWith("D"))
                ;

            // 여기서 디비깅, StartsWith()가 반복 호출됨
            foreach (Part p in parts2)
            {
                Console.WriteLine($"{p.Name}");
            }

        }


        /// <summary>
        /// Where() - .NET or DB에서 행 필터 처리
        /// <remarks>
        /// IEnumerable<>.ToList().Where() 시에는 테이블 전체 스캔 발생
        /// </remarks>
        /// </summary>
        static void SelectRows()
        {
            using SalesContext db = new SalesContext();

            int p_orderkey = 5;

            // ToList() 후 Where() 처리
            var orders = db
                .Orders
                .ToList()
                .Where(o => o.OrderKey <= p_orderkey)
                ;

            foreach (var o in orders) 
            {
                Console.WriteLine($"{o.OrderKey}, {o.OrderDate}");
            }
        }


        /// <summary>
        /// FindAsync() - PK 검색과 주의
        /// <remarks>
        /// DBContext를 조회해서 있으면 반환, 없으면 DB에서 검색 따라서 동일 값 조회 시 DB 조회 안함
        /// .AsNoTracking() 사용 못함, 이전 작업에서도 .AsNoTracking() 없어야 함
        /// </remarks>
        /// </summary>
        static async Task FindOrFindAsync()
        {
            var p_orderkey = 100;
            //var p_linenumber = 1;
            
            //Find() 동작 확인
            using (SalesContext db = new SalesContext())
            {
                var orderbykey = await db.Orders.FindAsync(p_orderkey);
                Console.WriteLine($"{orderbykey.OrderKey}, {orderbykey.OrderDate}");

                // 동일 키 다시 조회
                orderbykey = await db.Orders.FindAsync(p_orderkey);
                Console.WriteLine($"{orderbykey.OrderKey}, {orderbykey.OrderDate}");

                // 다른 값
                orderbykey = await db.Orders.FindAsync(130);
                Console.WriteLine($"{orderbykey.OrderKey}, {orderbykey.OrderDate}");

                // 단순 참고. 다중 키인 경우
                //var lineitem = await db.LineItems.FindAsync(p_orderkey, p_linenumber);
                //Console.WriteLine($"{lineitem.OrderKey}, {lineitem.LineNumber}");
            }

            // 새로운 DB Context
            using (SalesContext db = new SalesContext())
            {
                //다시 동일 값 조회
                var orderbykey = await db.Orders.FindAsync(p_orderkey);
                Console.WriteLine($"{orderbykey.OrderKey}, {orderbykey.OrderDate}");
            }

        }

        /// <summary>
        /// First vs. FirstOrDefault (혹은 Single() vs. SingleOrDefault())
        /// </summary>
        /// <remarks>
        /// SELECT 결과가 빈 행일 때 
        ///     - FirstOrDefault()는 null 반환 --> 따라서 null 결과 확인 처리 필요
        ///     - First()는 Exception 발생 --> 예외 처리 필요
        /// </remarks> 
        static async Task FirstOrFirstDefault()
        {
            using (SalesContext db = new SalesContext())
            {
                string p_name = "2"; //존재하는 값("Customer#000000001") or 없는 값("2")으로 조회

                //FirstOrDefaultAsync
                var customer = await db
                    .Customers
                    .Where(c => c.Name == p_name)
                    .FirstOrDefaultAsync()
                    ;

                if (customer != null)
                    Console.WriteLine($"CustKey: {customer.CustKey}\n");
                else
                    Console.WriteLine($"CustKey: null\n");


                try
                {
                    //FirstAsync
                    var customer2 = await db
                        .Customers
                        .Where(c => c.Name == p_name)
                        .FirstAsync()
                        ;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception is... {ex.Message}\n");
                }

            }
        }

        
        /// <summary>
        /// FirstOrDefault(or SingleOrDefault)와 서브쿼리
        /// </summary>
        /// <remarks>
        /// OUTER JOIN 이나 SUBQUERY 변환 시 COALESCE() 사용으로 실행계획 이슈
        /// </remarks>         
        static async Task FirstDefaultWithSubquery()
        {
            using (SalesContext db = new SalesContext())
            {
                var customers = await db
                    .Customers
                    .AsNoTracking()
                    //SSMS에서 직접 쿼리 테스트를 위해 Adhoc으로
                    .Where(c => c.Phone == "30-114-968-4951")   
                    .Select(c => new
                    {
                        Id = c.CustKey,
                        OrderID = c
                            .Orders
                            .OrderByDescending(o => o.OrderKey)
                            .Select(o => o.OrderKey)
                            //작성된 쿼리, SSMS에서 실행 계획 확인
                            //ISNULL()과 비교 (IO 등은 동일)
                            .FirstOrDefault()
                    })
                    .ToListAsync()
                    ;
            }
        }


        /// <summary>
        /// SingleOrDefault()
        /// </summary>
        /// <remarks>
        /// </remarks>     
        static async Task SingleOrDefault()
        {
            using SalesContext db = new SalesContext();

            try
            {
                //한 건일 때 여러 건일 때, "blue pink%" / "blue pink tan%"
                string prod_name = "blue pink tan%";  

                var part = await db
                    .Parts
                    // 예제로 EF.Functions 사용해 봄
                    .Where(p => EF.Functions.Like(p.Name, prod_name))
                    .SingleOrDefaultAsync()
                    ;

                if (part != null)
                    Console.WriteLine($"CustKey: {part.PartKey}, Name: {part.Name}\n");

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("more than two records found");
                Console.WriteLine($"Exception is... {ex.Message}");
            }

        }

        /// <summary>
        /// SQL의 IN 조건 구현
        /// </summary>
        /// <remarks>
        /// EF.Functions의 Constains를 활용
        /// </remarks>     
        static async Task InPredicate()
        {
            using SalesContext db = new SalesContext();

            int[] custkeys = new int[] { 1, 2, 3, 4, 5 };

            List<Customer> customers = await db.Customers
                .AsNoTracking()
                .Where(c => custkeys.Contains(c.CustKey))
                .ToListAsync();
        }


        /// <summary>
        /// SQL Server의 STRING_AGG() 구현
        /// </summary>
        /// <remarks>
        /// EF.Functions의 string.Join() 활용
        /// </remarks>  
        static async Task STRING_AGG()
        {
            using (SalesContext db = new SalesContext())
            {
                var query = db
                    .Orders
                    .Where(oh => oh.OrderKey <= 1000)
                    .GroupBy(oh => oh.OrderPriority)
                    .Select(grp => new
                    {
                        Priority = grp.Key,
                        Clerks = string.Join(",", grp.Select(oh => oh.Clerk.Trim()))
                    });

                // 개인적 관심, IAsyncEnumerable() 사용해 봄
                await foreach (var item in query.AsAsyncEnumerable())
                {
                    Console.WriteLine($"{item.Priority}, {item.Clerks}");
                }   
            }
        }
        
        
        
        
        
        /// <summary>
        /// CROSS JOIN
        /// </summary>
        /// <remarks>
        /// 불필요한 CROSS JOIN 하지 않아야한다.
        /// </remarks>
        /// <returns></returns>    
        static async Task CrossJoin()
        {
            var p_region = 1;

            using (SalesContext db = new SalesContext())
            {
                var query = from r in db.Regions.Where(r => r.RegionKey == p_region)
                            from n in db.Nations
                            select new
                            {
                                r.RegionKey,
                                n.NationKey
                            };

                foreach (var r in await query.Take(5).ToListAsync())
                {
                    Console.WriteLine($"Result: {r.RegionKey}, {r.NationKey}");
                    Console.WriteLine(new string('-', 20));
                }

                /*
                 * 아래는 쿼리 튜닝 필요한 예제 (Parts 테이블 2번 읽지 않도록 개선하기)
                 * 
                 *  방법-1) 쿼리에서는 Average만 구하고, .NET에서 Diff 계산
                 *  방법-2) 직접 쿼리 사용하기 (CROSS JOIN or 파생테이블 등 활용)
                 *  방법-3) 실제로 LINQ에서 1번만 읽도록 작성할 수 있나?
                 */
                var result = from r in db.Parts
                             select new
                             {
                                 r.Name,
                                 r.RetailPrice,
                                 Average = db.Parts.Average(c => c.RetailPrice),
                                 Diff = r.RetailPrice - db.Parts.Average(c => c.RetailPrice)
                             };

                foreach (var r in await result.Take(5).ToListAsync())
                {
                    Console.WriteLine($"Result: {r.Average}, {r.Diff}");
                    Console.WriteLine(new string('-', 20));
                }

            }
        }


        /// <summary>
        /// INNER JOIN
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>    
        static async Task InnerJoin()
        {
            using (SalesContext db = new SalesContext())
            {
                var query = from c in db.Customers
                            join o in db.Orders on c.CustKey equals o.CustKey
                            where o.OrderKey <= 2
                            select new
                            {
                                c.CustKey,
                                c.Name,
                                o.OrderDate
                            };

                foreach (var r in await query.ToListAsync())
                {
                    Console.WriteLine($"CustKey:{r.CustKey}, Name:{r.Name}, OrderDate:{r.OrderDate}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// OUTER JOIN
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>        
        static async Task OuterJoin()
        {
            var p_phone = "23-768-687-3665";    //30-114-968-4951, 23-768-687-3665

            using (SalesContext db = new SalesContext())
            {
                var query = from c in db.Customers
                            where c.Phone == p_phone
                            from o in db.Orders.Where(o => o.CustKey == c.CustKey).DefaultIfEmpty()
                            select new
                            {
                                c.CustKey,
                                c.Name,
                                OrderDate = o.OrderDate.ToShortDateString()
                            };

                foreach (var r in await query.ToListAsync())
                {
                    Console.WriteLine($"CustKey:{r.CustKey}, Name:{r.Name}, OrderDate:{r.OrderDate}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// OUTER APPLY
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>        
        static async Task OuterApply()
        {
            var p_phone = "25-989-741-2988";

            using (SalesContext db = new SalesContext())
            {
                var query = db
                    .Customers
                    .Where(c => c.Phone == p_phone)
                    .Select(c => new
                    {
                        CustKey = c.CustKey,
                        OrderDate = c
                            .Orders
                            .OrderByDescending(o => o.OrderDate)
                            .Select(o => new { o.OrderDate })
                            .Take(1)
                            // FirstOrDefault() or DefaultIfEmpty() 등 함수가 아니면 LEFT JOIN에 ROW_NUMBER()로 처리됨
                            .FirstOrDefault()   
                    });

                foreach (var r in await query.ToListAsync())
                {
                    Console.WriteLine($"CustKey:{r.CustKey}, OrderDate:{r.OrderDate}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }

        
        /// <summary>
        /// .Include
        /// </summary>
        /// <remarks>
        /// 기본적으로 OUTER JOIN
        /// </remarks>
        /// <returns></returns>    
        static async Task IncludeThenInclude()
        {
            using (SalesContext db = new SalesContext())
            {
                // .Include
                var customers = await db
                    .Customers.Where(c => c.CustKey <= 1)
                    .Include(o => o.Orders)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var c in customers)
                {
                    Console.WriteLine($"Name:   {c.Name}");
                    // 후속 Enumerable 연산 지원
                    foreach (var o in c.Orders.Take(2))
                    {
                        Console.WriteLine($"OrderKey:   {o.OrderKey}");
                    }
                    Console.WriteLine(new string('-', 20));
                }

                // .ThenInclude
                var customers2 = await db
                        .Customers.Where(c => c.CustKey <= 1)
                        .Include(o => o.Orders.Where(o => o.OrderKey <= 2))
                            .ThenInclude(l => l.LineItems.Where(l => l.OrderKey <= 2))
                        .AsNoTracking() 
                        .ToListAsync();

                foreach (var c in customers2)
                {
                    Console.WriteLine($"Name:   {c.Name}");
                    foreach (var o in c.Orders)
                    {
                        Console.WriteLine($"OrderKey:   {o.OrderKey}");
                        foreach (var l in o.LineItems)
                        {
                            Console.WriteLine($"PartKey:   {l.PartKey}");
                        }
                    }
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// .Include 및 쿼리 분할(EF Core 5)
        /// <remarks>
        /// 마지막 쿼리를 제외&MARS(false)하고 자동으로 Buffering 됨 (아래 Interal Buffering 설명 참조)
        /// </remarks>
        /// </summary>
        static async Task IncludeSplitQuery()
        {
            using (SalesContext db = new SalesContext())
            {
                //분할 쿼리 사용 전
                var orders = await db.Orders
                    .Include(o => o.LineItems)
                    .Where(o => o.CustKey == 1)
                    .ToListAsync()
                    ;

                foreach (var o in orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                    // 후속 Enumerable 연산 지원
                    foreach (var l in o.LineItems)
                    {
                        Console.WriteLine($"PartKey:   {l.PartKey}");
                    }
                    Console.WriteLine(new string('-', 20));
                }

                Console.WriteLine("Enter any key for next..."); Console.ReadKey();

                //분할 쿼리 사용
                orders = await db.Orders
                    .Include(o => o.LineItems)
                    .Where(o => o.CustKey == 1)
                    .AsSplitQuery()
                    .ToListAsync()
                    ;

                foreach (var o in orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                    // 후속 Enumerable 연산 지원
                    foreach (var l in o.LineItems)
                    {
                        Console.WriteLine($"PartKey:   {l.PartKey}");
                    }
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// EF Internal Buffering
        /// </summary>
        /// <remarks>
        /// </remarks>        
        static void InternalBuffering()
        {
            using SalesContext db = new SalesContext();
            var parts = db
                .Parts
                .Where(p => p.PartKey <= 2)
                ;

            foreach (Part p in parts)
            {
                Console.WriteLine($"{p.Name}");
            }

            parts = db
                .Parts
                .Where(p => p.PartKey <= 4)
                ;

            //여기서 중단점 걸로, Entity 생성 여부 확인
            foreach (Part p in parts)
            {
                Console.WriteLine($"{p.Name}");
            }

        }


        /// <summary>
        /// Lazy Lodding
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>    
        static async Task LazyLoading()
        {
            /*
             * Lazy Lodding 옵션으로 조정
             */
            using SalesContext db = new SalesContext();

            List<Customer> customers = await db.Customers
                .Where(c => c.CustKey <= 5)
                .ToListAsync();

            foreach (Customer c in customers)
            {
                Console.WriteLine($"CustKey:{c.CustKey}, Phone:{c.Phone}");
                foreach (Order o in c.Orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                }
                Console.WriteLine(new string('-', 20));
            }
        }


        /// <summary>
        /// Eager Lodding
        /// </summary>
        /// <remarks>
        /// Eager Lodding vs. Lazy Lodding
        /// </remarks>
        /// <returns></returns>    
        static async Task EagerLoading()
        {
            using SalesContext db = new SalesContext();

            // 1) Include()
            List<Customer> customers = await db.Customers
                .Include(c => c.Orders)
                .Where(c => c.CustKey <= 5)
                .TagWith("Include 사용")
                .ToListAsync();

            foreach (Customer c in customers)
            {
                Console.WriteLine($"CustKey:{c.CustKey}, Phone:{c.Phone}");
                foreach (Order o in c.Orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                }
                Console.WriteLine(new string('-', 20));
            }

            // 2) .Join은 INNER JOIN 수행
            var results = await db.Customers
                .Join(db.Orders, c => c.CustKey, o => o.CustKey, (c, o) => new { Customers = c, Orders = o })
                .Where(co => co.Customers.CustKey <= 2)
                .OrderBy(co => co.Customers.Name)
                .Select(co => new
                {
                    CustID = co.Customers.CustKey,
                    Phone = co.Customers.Phone,
                    OrderID = co.Orders.OrderKey
                })
                .TagWith("Join 사용")
                .ToListAsync();

            foreach (var r in results)
            {
                Console.WriteLine($"CustID:{r.CustID}, Phone:{r.Phone}, OrderID:{r.OrderID}");
                Console.WriteLine(new string('-', 20));
            }

            // 3) Projection
            var customers2 = await db.Customers
                .Where(c => c.CustKey <= 2)
                .Select(c => new { c.CustKey, c.Phone, c.Orders })
                .TagWith("Projection 사용")
                .ToListAsync();

            foreach (var c in customers2)
            {
                Console.WriteLine($"CustKey:{c.CustKey}, Phone:{c.Phone}");
                foreach (Order o in c.Orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                }
                Console.WriteLine(new string('-', 20));
            }

            // 4) ChangeTracker.LazyLoadingEnabled = false
            db.ChangeTracker.LazyLoadingEnabled = false;

            customers = await db.Customers
                .Where(c => c.CustKey <= 2)
                .TagWith("LazyLoadingEnabled=False 사용")
                .ToListAsync();

            foreach (Customer c in customers)
            {
                Console.WriteLine($"CustKey:{c.CustKey}, Phone:{c.Phone}");
                foreach (Order o in c.Orders)
                {
                    Console.WriteLine($"OrderKey:   {o.OrderKey}");
                }
                Console.WriteLine(new string('-', 20));
            }
        }


        /// <summary>
        /// 불필요한 중복 쿼리(호출, I/O)
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        static async Task AggregateRepeat()
        {
            using SalesContext db = new SalesContext();

            /*
             * Before
             */
            var customers_1 = await db
                .Orders
                .Select(o => new { o.CustKey })
                .Distinct()
                .Take(20)	// 예제로 일부만
                .ToListAsync();

            foreach (var customer in customers_1)
            {
                var orders = db
                    .Orders
                    .Where(o => o.CustKey == customer.CustKey)
                    .GroupBy(o => o.OrderDate)
                    .Count();
            }

            /*
             * After
             */
            var customers_2 = await db
                .Orders
                .GroupBy(o => new { o.CustKey })
                .Select(o => new
                {
                    o.Key,
                    count = o.Count()
                })
                .ToListAsync();
        }


        /// <summary>
        /// 집합 연산자 그리고 UNION ALL
        /// INTERSECT
        /// EXCEPT
        /// </summary>
        /// <remarks>
        /// UNION vs. UNION ALL
        /// </remarks>
        /// <returns></returns>
        static async Task SetOpAndUnionAll()
        {
            using SalesContext db = new SalesContext();

            // Suppliers #1
            var result1 = db
                    .Suppliers
                    .Where(s => s.Suppkey >= 1 && s.Suppkey <= 5)
                    .Select(s => new { key = s.Nationkey, s.Name });
            
            // Suppliers #2
            var result2 = db
                    .Suppliers
                    .Where(s => s.Suppkey >= 1 && s.Suppkey <= 10)
                    .Select(s => new { key = s.Nationkey, s.Name });

            
            /**********************
             * A. 교집합
             */
            var result_intersect = await result1.Intersect(result2).ToListAsync();


            /**********************
             * B. 차집합
             */
            var result3 = db
                    .Suppliers
                    .Where(s => s.Suppkey >= 5 && s.Suppkey <= 10)
                    .Select(s => new { key = s.Nationkey, s.Name });

            var result_except = await result1.Except(result3).ToListAsync();

            
            /**********************
             * C. 합집합: UNION vs. UNION ALL
             */
            var result_union = await result1.Union(result2).ToListAsync();
            var result_concat = await result1.Concat(result2).ToListAsync();

        }



        /// <summary>
        /// Non SARG - Column에 연산
        /// </summary>
        /// <remarks>
        /// </remarks>  

        // 예제 추가로 구현할 것
        static async Task NonSARG_1()
        {
            using SalesContext db = new SalesContext();

            var custkey = 10955;

            var customers = await db.Orders
                .AsNoTracking()
                .Where(o => o.CustKey + 1 == custkey)
                .ToListAsync();
        }


        /// <summary>
        /// Non SARG - Column에 함수 적용
        /// </summary>
        /// <remarks>
        /// </remarks>  
        static void NonSARG_2()
        {
            using SalesContext db = new SalesContext();

            var p_orderdate = DateTime.Parse("1994-03-25");

            var orders = db.Orders
                .Where(o => o.OrderDate.AddDays(10) == p_orderdate)
                .ToListAsync()
                ;

            Console.WriteLine($"{orders}");
        }


        /// <summary>
        /// Non SARG - Column보다 Expression에 더 큰 데이터 형식
        /// </summary>
        /// <remarks>
        /// </remarks>    
        static async Task NonSARG_3()
        {
            using SalesContext db = new SalesContext();

            /*
             * A) (entity 모델엔 nvarchar, 실제 Column은 varchar) = nvarchar 식
             */
            string p_name = "Customer#000000001";
            var customers = await db.Customers
                .AsNoTracking()
                .Where(c => c.Name == p_name)
                .ToListAsync();

            /*
             * B) int = bigint --> 쿼리 식은 Non SARG이나 실제론 Convert() 얼어나지 않음
             */
            Int64 p_orderkey = 2200000000;
            var orders = await db.Orders
                .AsNoTracking()
                .Where(o => o.OrderKey == p_orderkey)     
                .ToListAsync();
        }


        /// <summary>
        /// Non SARG - LIKE 검색 주의
        /// </summary>
        /// <remarks>
        /// </remarks>  
        static async Task NonSARG_4()
        {
            /*
             * 검색 값이 리터럴(상수)인 경우와 매개변수인 경우 SQL 다르게 생성
             */
            using (SalesContext db = new SalesContext())
            {
                //string.Contains()
                var customers = await db.Customers
                    .Where(c => c.Phone.Contains("010"))
                    .AsNoTracking()
                    .TagWith(".Contains()")
                    .ToListAsync()
                    ;

                //string.StartsWidth()
                customers = await db.Customers
                    .Where(c => c.Phone.StartsWith("010"))
                    .AsNoTracking()
                    .TagWith(".StartsWithNotNull")
                    .ToListAsync()
                    ;

                //string.StartsWidth()의 경우 열이 NULL 허용이면 IS NOT NULL 부가 조건이 붙는다.
                var dml = await db.DmlMasters
                    .Where(c => c.CustomerKey.StartsWith("123"))
                    .AsNoTracking()
                    .TagWith(".StartsWithNullable")
                    .ToListAsync()
                    ;

                //EF.Functions.Like
                customers = await db.Customers
                    .Where(c => EF.Functions.Like(c.Phone, "010%"))
                    .AsNoTracking()
                    .TagWith("EF.Functions.Like")
                    .ToListAsync()
                    ;
            }

            string p_phone = "010";
            string p_phone_like = "010%";

            using (SalesContext db = new SalesContext())
            {
                //string.Contains()
                var customers = await db.Customers
                    .Where(c => c.Phone.Contains(p_phone))
                    .AsNoTracking()
                    .TagWith(".Contains()")
                    .ToListAsync()
                    ;

                //string.StartsWidth()
                customers = await db.Customers
                    .Where(c => c.Phone.StartsWith(p_phone))
                    .AsNoTracking()
                    .TagWith(".StartsWithNotNull")
                    .ToListAsync()
                    ;

                //EF.Functions.Like
                customers = await db.Customers
                    .Where(c => EF.Functions.Like(c.Phone, p_phone_like))
                    .AsNoTracking()
                    .TagWith("EF.Functions.Like")
                    .ToListAsync()
                    ;
            }
        }


        /// <summary>
        /// Non SARG - 열 간 비교
        /// </summary>
        /// <remarks>
        /// </remarks>  
        static async Task NonSARG_5()
        {
            string p_custkey = "abcde";    // null, "abcde"

            using SalesContext db = new SalesContext();

            var orders = await db
                .DmlMasters
                .AsNoTracking()
                .Where(o => o.CustomerKey == (p_custkey ?? o.CustomerKey))
                .ToListAsync()
                ;
        }






        /// <summary>
        /// Skip() + Take() 기본 
        /// </summary>
        /// <remarks>
        /// 1) Parameterization 여부 (과거 EF는 안됐음)
        ///     --> Parameter 화 되는 것 확인, Cool~
        /// </remarks> 
        static async Task SkipTake()
        {
            var currentpage = 100;
            var rowsperpage = 20;

            using SalesContext db = new SalesContext();
            var orders = await db
                .Orders
                .AsNoTracking()
                .OrderByDescending(o => o.OrderKey)
                .Skip((currentpage - 1) * rowsperpage)
                .Take(rowsperpage)
                .ToListAsync()
                ;

            foreach (var r in orders.Take(5))
            {
                Console.WriteLine($"OrderKey: {r.OrderKey}, OrderDate: {r.OrderDate}");
                Console.WriteLine(new string('-', 20));
            }
        }


        /// <summary>
        /// Paging at .NET or DB (필요한 데이터만 요청한다)
        /// </summary>
        /// <remarks>
        /// 필요한 데이타만 요청한다
        /// </remarks> 
        static async Task Paging()
        {
            int currentpage = 100;
            int rowsperpage = 20;
            int p_orderkey = 100000;

            using SalesContext db = new SalesContext();

            /*
             * A) Paging을 .NET에서
             */
            var lineitems_e = await db
                .LineItems
                .AsNoTracking()
                .Where(li => li.OrderKey <= p_orderkey)
                .OrderByDescending(li => li.OrderKey)
                .ToListAsync()
                ;

            int count_e = lineitems_e.Count();

            var page_e = lineitems_e
                .Skip((currentpage - 1) * rowsperpage)
                .Take(rowsperpage)
                ;

            /*
             * B) Paging을 DB에서
             */
            var lineitems_q = db
                .LineItems
                .AsNoTracking()
                .Where(li => li.OrderKey <= p_orderkey)
                .OrderByDescending(li => li.OrderKey)
                .Select(li => new { li.OrderKey, li.LineNumber }) //Projection도
                ;

            int count_q = lineitems_q.Count();

            var page_q = await lineitems_q
                .Skip((currentpage - 1) * rowsperpage)
                .Take(rowsperpage)
                .ToListAsync()
                ;
        }


        /// <summary>
        /// 만능 조회 (동적 검색 조건)
        /// </summary>
        /// <remarks>
        /// </remarks>  
        //OR 방식 사용 시 과거 EF와 달리(버전 확인 필요) PEO를 자동으로 수행함, Cool~
        static async Task DynamicSearching()
        {
            int? p_nationKey = null;                  // 5
            string? p_name = "Customer#000000010";    // "Customer#000000010"
            string? p_phone = "15-741-346-9870";      // "15-741-346-9870"

            using SalesContext db = new SalesContext();
            var customers = await db.Customers.Where(c =>
                       (c.CustKey <= 100)
                    && (p_nationKey == null || c.NationKey == p_nationKey)
                    && (p_name == null      || c.Name == p_name)
                    && (p_phone == null     || c.Phone == p_phone)
                    )
                .OrderBy(c => c.CustKey)
                .Take(10)
                .ToListAsync();
        }


        /// <summary>
        /// CompileQuery() or CompileAsyncQuery()
        /// </summary>
        /// <remarks>
        /// </remarks>      
        static async Task CompileQuery()
        {
            var p_custkey = 10954;
            
            // 컴파일 쿼리로 호출 (SalesContext에 생성)
            using (SalesContext db = new SalesContext())
            {
                await foreach (var c in SalesContext.getCustByKey(db, p_custkey))
                {
                    Console.WriteLine($"CustKey: {c.CustKey}, Phone: {c.Phone}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// (학습용) 비동기 처리 취소용 CancelAfter() 의 간단한 예제
        /// <remarks>
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        static async Task CancelAsync()
        {
            var cancelquery = new CancellationTokenSource();
            var p_orderkey = 1500000;   // 150만건, 10초 대기, (아래 쿼리는 결과 처리 전까지 걸리는 시간은 평균 2초대)

            using (SalesContext db = new SalesContext())
            {
                try
                {
                    // 중단시간은 EF Core 측 기준 (실제 DB에서 시간은 지정보다 짧다)
                    cancelquery.CancelAfter(10 * 1000);

                    // 쿼리 자체 시간이 궁금하면 SSMS에서 "결과 삭제" 옵션 선택하고 쿼리 실행
                    var orders = await db.LineItems
                        .Where(o => o.OrderKey <= p_orderkey)
                        .ToListAsync(cancelquery.Token)                       
                        ;
                    foreach (var o in orders) { };
                    Console.WriteLine("----- end -----");
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine($"\nQuery cancelled: \n{ex.Message}, \n{ex.InnerException}");
                }

                catch (SqlException ex) //when (ex.Number == -2)
                {
                    //참고. CommandTimeout 발생
                    if (ex.Number == -2)
                    {
                        Console.WriteLine($"\nTimeout 예외 발생: {ex.Message}, \n{ex.InnerException}");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Tag를 통한 쿼리 주석 (김정선 권고^^)
        /// </summary>
        /// <remarks>
        /// 쿼리 앞부분에 적용됨 --> 위치를 조정할 수 있으면 좋을 듯.
        /// </remarks>
        static async Task Tag()
        {
            //string tag = MethodBase.GetCurrentMethod().DeclaringType.FullName;
            string p_phone = "25-989-741-2988";

            using (SalesContext db = new SalesContext())
            {
                var customers = await db.Customers
                    .TagWith(tag: "[Sales.Program.Tag()]")              //"[Sales.Program.Tag()]", tag
                    .Where(c => c.Phone == p_phone)
                    .ToListAsync()
                    ;
            }
        }

        

        
        


        /// <summary>
        /// FromSqlRaw vs. FromSqlInterpolated 
        /// </summary>
        /// <remarks>
        /// Adhoc or Parameterization
        /// </remarks>  
        static async Task FromSqlOrFromSqlRaw()
        {
            string value = "23-768-687-3665";
            

            using (SalesContext db = new SalesContext()) 
            {
                // .FromSql()
                var customers = await db
                    .Customers
                    .FromSql($"SELECT * FROM dbo.Customers AS c WHERE Phone = {value}")
                    .AsNoTracking()
                    .ToListAsync();

                foreach (Customer c in customers)
                {
                    Console.WriteLine($"CustKey: {c.CustKey}, Phone: {c.Phone}");
                    Console.WriteLine(new string('-', 20));
                }

                // .FromSqlRaw()
                customers = await db
                    .Customers
                    .FromSqlRaw($"SELECT * FROM dbo.Customers AS c WHERE Phone = '{value}'")
                    .AsNoTracking()
                    .ToListAsync();

                foreach (Customer c in customers)
                {
                    Console.WriteLine($"CustKey: {c.CustKey}, Phone: {c.Phone}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// Parameter 개체 사용한 직접 매개변수화
        /// </summary>
        /// <remarks>
        /// EF Core 매개변수화에 성능 이슈 문제 해결
        /// </remarks>  
        static async Task FromSqlParameter()
        {
            SqlParameter p_phone = new SqlParameter("Phone", SqlDbType.VarChar, 30) { Value = "23-768-687-3665" };

            using (SalesContext db = new SalesContext()) 
            {
                // A) FromSql()
                var customers = await db.Customers
                    .FromSql($@"SELECT * FROM dbo.Customers AS c
                                WHERE Phone = {p_phone}")
                    .AsNoTracking()
                    .ToListAsync();

                foreach (Customer c in customers)
                {
                    Console.WriteLine($"CustKey: {c.CustKey}, Phone: {c.Phone}");
                    Console.WriteLine(new string('-', 20));
                }

                // A) FromSqlRaw()
                customers = await db.Customers
                    .FromSqlRaw($@"SELECT * FROM dbo.Customers AS c 
                                   WHERE Phone = @Phone", p_phone)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (Customer c in customers)
                {
                    Console.WriteLine($"CustKey: {c.CustKey}, Phone: {c.Phone}");
                    Console.WriteLine(new string('-', 20));
                }
            }
        }


        /// <summary>
        /// SQL Sever의 Table Valued Function을 EF에서 사용하기
        /// <remarks>
        /// 특히 인라인 테이블 값 함수(iTVFs) 유용
        /// </remarks>
        /// </summary>
        static async Task TVF()
        {
            int p_orderkey = 10;

            using (SalesContext db = new SalesContext()) 
            {
                // GetOrdersList() 함수는 SalesContext.cs에 정의
                var orders = await db
                    .GetOrdersList(p_orderkey)    // WHERE OrderKey < @orderkey
                    .OrderBy(o => o.OrderKey)
                    .ToListAsync()
                    ;

                foreach (var o in orders)
                {
                    Console.WriteLine(o.OrderKey);
                }
            }
        }


        /// <summary>
        /// 스칼라형(Non-Entity) 반환 쿼리
        /// </summary>
        /// <remarks>
        /// db.SqlQuery() - 결과 스칼라 값의 이름은 "Value"로 지정
        /// </remarks>
        static async Task SqlQuery()
        {
            var p_partkey = 5;

            //formattablestring 에서 라인 연결은 $@
            FormattableString query;
            query = $@"SELECT p.RetailPrice - pp.Average AS Value 
                        FROM dbo.Parts AS p
                        CROSS JOIN (SELECT AVG(pp.RetailPrice) AS Average FROM dbo.Parts pp) AS pp
                        WHERE p.PartKey <= {p_partkey}";
                
            using (SalesContext db = new SalesContext())
            {
                // A) SqlQuery()
                var prices = await db
                    .Database
                    .SqlQuery<decimal>(query)
                    .OrderBy(p => p)
                    .Take(5)
                    .ToListAsync()
                    ;

                foreach (var p in prices)
                {
                    Console.WriteLine($"Diff: {p}");
                }

            }
        }


        /// <summary>
        /// Entity 기준 저장 프로시저 호출
        /// <remarks></remarks>
        /// </summary>
        static async Task StoredProcedureByEntity()
        {
            var p_custkey = new SqlParameter("@p_CustKey", SqlDbType.Int, 4) { Direction = ParameterDirection.Input, Value = 10 };
            var p_count = new SqlParameter("@p_Count", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
            // 주의. 여기서는 return 값도 output로 정의
            var p_result = new SqlParameter("@return", SqlDbType.Int, 4) { Direction = ParameterDirection.Output}; 

            using (SalesContext db = new SalesContext())
            {
                var customers = await db
                    .Customers
                    // 실제로는 sp_executesql로 호출
                    .FromSql($"EXEC {p_result} = dbo.up_CustomerInfo @p_CustKey={p_custkey}, @p_Count={p_count} OUTPUT")
                    .ToListAsync()
                    ;

                foreach (var c in customers)
                {
                    Console.WriteLine($"{c.CustKey}, {c.Phone}");
                }

                Console.WriteLine($"@p_Count: {p_count.Value}, @return: {p_result.Value}");
            }
        }


        /// <summary>
        /// 조회 전용 프로시저 호출  by Native SQLClient
        /// <remarks>
        /// 다중 조인이나 복잡한 형태 쿼리로 구성된 프로시저 호출
        /// </remarks>
        /// </summary>
        static async Task StoredProcedureByComplexQuery()
        {
            var paramInfo = new[]
            {
                new SqlParameter("@p_CustKey", SqlDbType.Int, 4) {Direction = ParameterDirection.Input, Value = 10},
                new SqlParameter("@p_Count", SqlDbType.Int, 4) {Direction = ParameterDirection.Output},
                //Native는 ParameterDirection.ReturnValue
                new SqlParameter("@return", SqlDbType.Int, 4) {Direction = ParameterDirection.ReturnValue}
            };

            using SalesContext db = new SalesContext();
            using (var cnn = db.Database.GetDbConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_OrdersInfoByCustomer";
                cmd.Parameters.AddRange(paramInfo);
                cmd.Connection = cnn;

                await cnn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]}, {reader[1]}, {reader[2]}, {reader[3]}");
                    }
                    await reader.CloseAsync();
                    Console.WriteLine($"@p_Count: {paramInfo[1].Value}, @return: {paramInfo[2].Value}");
                }
                else
                {
                    Console.WriteLine($"No resultset");
                }
            }
        }


        /// <summary>
        /// 다중 결과 집합 반환 프로시저 호출 by Native SQLClient
        /// <remarks>
        /// 다중 SELECT 쿼리로 구성된 프로시저
        /// </remarks>
        /// </summary>
        static async Task StoredProcedureByMultiResultsets()
        {
            var paramInfo = new[]
            {
                new SqlParameter("@p_CustKey", SqlDbType.Int, 4) {Direction = ParameterDirection.Input, Value = 5},
                new SqlParameter("@p_PartKey", SqlDbType.Int, 4) {Direction = ParameterDirection.Input, Value = 5}
            };

            using SalesContext db = new SalesContext();
            using (var cnn = db.Database.GetDbConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.up_CustomerAndPartsInfo";
                cmd.Parameters.AddRange(paramInfo);
                cmd.Connection = cnn;

                await cnn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                if (reader.HasRows)
                {
                    Console.WriteLine($"Customers Info");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]}, {reader[1]}, {reader[2]}");
                    }
                }
                else
                {
                    Console.WriteLine($"No resultset for Customers");
                }

                // 다음 결과집합 패치
                reader.NextResult();

                if (reader.HasRows)
                {
                    Console.WriteLine($"Parts Info");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]}, {reader[1]}, {reader[2]}");
                    }
                    await reader.CloseAsync();
                }
                else
                {
                    Console.WriteLine($"No resultset for Parts");
                }
            }
        }



        
        
        

        /// <summary>
        /// DML 예제
        /// <remarks>1건 INSERT</remarks>
        /// </summary>
        static async Task InsertOneOrder()
        {
            using SalesContext db = new SalesContext();

            var dml_m = new DmlMaster()
            {
                OrderDate = DateTime.Now,
                CustomerKey = "abcde",
                TotalPrice = 2500.59m,
                Note = "Good"
            };

            try
            {
                await db.DmlMasters.AddAsync(dml_m);

                var rowsAffected = await db.SaveChangesAsync();
                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML 예제
        /// <remarks>N건 INSERT</remarks>
        /// </summary>
        static async Task InsertOrders()
        {
            List<DmlMaster> orders = new();

            int maxInsert = 2; //디폴트 42건 기준: 3, 4, 42, 45, 46, 85
            for (int i = 0; i < maxInsert; i++)
            {
                orders.Add(new DmlMaster()
                {
                    //OrderKey = i + 1,
                    OrderDate = DateTime.Now,
                    CustomerKey = i.ToString(),
                    TotalPrice = i * 100,
                    Note = "Good"
                });
            }

            using (SalesContext db = new SalesContext())
            {
                try
                {
                    await db.DmlMasters.AddRangeAsync(orders);

                    var rowsAffected = await db.SaveChangesAsync();
                    Console.WriteLine($"{rowsAffected} rows affected.");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// DML 예제
        /// <remarks>1건 UPDATE</remarks>
        /// </summary>
        static async Task UpdateOneOrder()
        {
            var p_orderkey = 35;  // 기존 Key 확인 후 사용
            using SalesContext db = new SalesContext();

            try
            {
                var order = await db.DmlMasters.FindAsync(p_orderkey);

                order.TotalPrice -= 1000;

                db.DmlMasters.Update(order);
                var rowsAffected = await db.SaveChangesAsync();

                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML 예제
        /// <remarks>N건 UPDATE</remarks>
        /// </summary>
        static async Task UpdateOrders()
        {
            var p_orderkey = 200;
            using SalesContext db = new SalesContext();

            try
            {
                //3건 vs. 4건이후
                var orders = await db
                    .DmlMasters
                    .Where(o => o.OrderKey <= p_orderkey)
                    .Take(2)
                    .ToListAsync();

                foreach (DmlMaster o in orders)
                {
                    o.TotalPrice += 100;
                }

                db.DmlMasters.UpdateRange(orders);
                var rowsAffected = await db.SaveChangesAsync();

                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML 예제
        /// <remarks>PK(UQ)가 아닌 조건으로 검색 후 UPDATE한 경우</remarks>
        /// </summary>
        static async Task UpdateOneOrdersWithNonKey()
        {
            var p_custkey = "abcde";

            using SalesContext db = new SalesContext();

            try
            {
                var orders = await db.DmlMasters
                    .Where(o => o.CustomerKey == p_custkey)
                    .Take(4)
                    .ToListAsync();

                foreach (DmlMaster o in orders)
                {
                    o.TotalPrice += 100;
                }

                db.DmlMasters.UpdateRange(orders);
                var rowsAffected = await db.SaveChangesAsync();

                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML 예제
        /// <remarks>1건 DELETE</remarks>
        /// </summary>
        static async Task DeleteOneOrder()
        {
            var p_orderkey = 39;  // 기존 Key 확인 후 사용

            using SalesContext db = new SalesContext();

            try
            {
                var order = await db.DmlMasters.FindAsync(p_orderkey);

                db.DmlMasters.Remove(order);
                var rowsAffected = await db.SaveChangesAsync();

                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML 예제
        /// <remarks>N건 DELETE</remarks>
        /// </summary>
        static async Task DeleteOrders()
        {
            var p_orderkey = 200;

            using SalesContext db = new SalesContext();

            try
            {
                var orders = await db
                    .DmlMasters
                    .Where(o => o.OrderKey <= p_orderkey)
                    .Take(4)
                    .ToListAsync();

                db.DmlMasters.RemoveRange(orders);
                var rowsAffected = await db.SaveChangesAsync();

                Console.WriteLine($"{rowsAffected} rows affected.");
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ExecuteDelete
        /// <remarks>
        /// 
        /// </remarks>
        /// </summary>
        static async Task ExecuteDelete()
        {
            /*
             * ExecuteDelete()
             */
            var p_orderkey = 10;

            using (SalesContext db = new SalesContext())
            {
                // 단순 DELETE
                // tx는 예제용 & 데이터 중간 확인용
                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    var rows = await db
                        .DmlMasters
                        .Where(d => d.OrderKey <= p_orderkey)
                        .ExecuteDeleteAsync();

                    Console.WriteLine($"{rows} rows deleted, any key to continue...\n"); Console.ReadKey();
                    await tx.RollbackAsync();

                }//using tx

                // JOIN(Subquery) + DELETE
                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    var rows = await db
                        .Orders
                        .Where(o => o.OrderKey <= p_orderkey && o.LineItems.All(l => l.Discount > 0.01m))
                        .ExecuteDeleteAsync();

                    Console.WriteLine($"{rows} rows deleted, any key to continue..."); Console.ReadKey();
                    await tx.RollbackAsync();

                }//using tx
            }//using db
        }


        /// <summary>
        /// ExecuteUpdate
        /// <remarks>
        /// 
        /// </remarks>
        /// </summary>
        static async Task ExecuteUpdate()
        {
            /*
             * ExecuteUpdate()
             */
            var p_orderkey = 50;

            using (SalesContext db = new SalesContext())
            {
                // tx는 예제용 & 데이터 중간 확인용
                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    var rows = await db
                        .DmlMasters
                        .Where(d => d.OrderKey <= p_orderkey)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(d => d.OrderDate, d => d.OrderDate.Value.AddDays(+365))
                            .SetProperty(d => d.TotalPrice, d => (d.TotalPrice * 2))
                            );

                    if (rows > 0)
                    {
                        await tx.CommitAsync();
                        Console.WriteLine($"{rows} rows updated");
                    }
                    else
                    {
                        await tx.RollbackAsync();
                    }
                }//using tx
            }//using db
        }
                           
        
        /// <summary>
        /// DML 예제
        /// <remarks>DELETE or UPDATE 쿼리 직접 호출</remarks>
        /// </summary>
        static async Task DeleteRawSQL()
        {
            try
            {
                var o_orderkey = 100;

                FormattableString query;
                query = $@"DELETE d
                        FROM dbo.DML_Master AS d
                        WHERE (SELECT COUNT(*) FROM dbo.DML_Master AS m
                                WHERE m.CustomerKey = d.CustomerKey) > 1
                            AND d.OrderKey <= {o_orderkey}";

                using (SalesContext db = new SalesContext())
                {
                    // 아래 tx는 디비깅용
                    using (var tx = await db.Database.BeginTransactionAsync())
                    {
                        // A) SqlQuery()
                        int rowsAffected = await db
                            .Database
                            .ExecuteSqlAsync(query)
                            ;

                        Console.WriteLine($"{rowsAffected} rows affected.");
                        await tx.RollbackAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        
        
        
        
        /// <summary>
        /// 로컬 트랜잭션
        /// </summary>
        /// <remarks>격리수준 확인, Commit/Rollback/SavePoint 확인</remarks>
        /// <returns></returns>
        static async Task Transaction_Async_Default()
        {
            //주문번호 - 먼저 존재하는지 확인
            var orderkey = 100;

            using (SalesContext db = new SalesContext())
            {
                var order = await db
                    .Orders
                    .FirstOrDefaultAsync(o => o.OrderKey == orderkey);

                Console.WriteLine($"TotalPrice: {order.TotalPrice} (Before)");

                //데이터 변경
                order.TotalPrice -= 10000;

                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    //여기서 중단점 후에 디버깅
                    // 1. 격리수준 확인
                    Console.WriteLine(tx.GetDbTransaction().IsolationLevel.ToString());

                    // 사용자 정의 Savepoint 정의
                    tx.CreateSavepoint("BeforeUpdate");

                    //Update
                    // 2. MARS = OFF/ON에 따른 동작 비교(콘솔 경고 메시지도 확인)
                    var rowsAffected = await db.SaveChangesAsync();

                    Console.WriteLine($"{rowsAffected} rows affected, TotalPrice: {order.TotalPrice} (After)");

                    // 3. Savepoint 동작 확인
                    await tx.RollbackToSavepointAsync("BeforeUpdate");
                    Console.WriteLine($"Rollback to SavePoint, [BeforeUpdate]");

                    order = await db
                        .Orders
                        .AsNoTracking() //DBContext가 아니라 DB에서 새로 검색하기 위해
                        .FirstOrDefaultAsync(o => o.OrderKey == orderkey);

                    Console.WriteLine($"TotalPrice: {order.TotalPrice} (After Rollback)");

                    //다루지는 않음
                    //await tx.RollbackToSavepointAsync("__EFSavePoint");
                    //Console.WriteLine($"Rollback to SavePoint, __EFSavePoint");

                    // 4. Commit or Rollback
                    //await tx.CommitAsync();
                    await tx.RollbackAsync();
                }
            }
        }
        

        /// <summary>
        /// DML
        /// </summary>
        /// <returns>
        /// UPDATE 시 SELECT 잠금 대기 확인 (디폴트 격리수준 ReadCommittd)
        /// </returns>
        static async Task Transaction_Async_UPDATE_Lock()
        {
            var p_orderkey = 100;
            var waittime = 30;

            try
            {
                using (SalesContext db = new SalesContext())
                {
                    using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                    {
                        var order = await db.Orders.FindAsync(p_orderkey);
                        order.TotalPrice += 100;

                        var rowsAffected = await db.SaveChangesAsync();
                        Console.WriteLine($"\n{rowsAffected} rows affected and waiting for {waittime}sec...");

                        await Task.Delay(waittime * 1000); //ms 단위
                        // 1. 대기 중에 SSMS에서 SELECT 수행 후 차단 여부 확인
                        // SELECT TOP(1) * FROM SalesSimple.dbo.Orders WHERE OrderKey = 100

                        // 2. 잠금 정보는 "잠금 상태 확인.sql" 참조
                        await tx.RollbackAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// DML
        /// </summary>
        /// <returns>
        /// SELECT 시 UPDATE 잠금 대기 확인
        /// </returns>
        static async Task Transaction_Async_SELECT_Lock_WithReadCommitted()
        {
            var p_orderkey = 100;
            var waittime = 30;

            using (SalesContext db = new SalesContext())
            {
                using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                {
                    var order = await db.Orders.FindAsync(p_orderkey);

                    Console.WriteLine($"\nSELECT TotalPrice: {order.TotalPrice}, and waiting for {waittime}sec...");
                    await Task.Delay(waittime * 1000);

                    //대기 중에 SSMS에서 UPDATE 수행 시 차단 여부 확인
                    /*
                    USE SalesSimple;

                    EXEC sp_lock;

                    BEGIN TRAN
                        UPDATE dbo.Orders SET TotalPrice = 0 
                        OUTPUT inserted.* WHERE OrderKey = 100
                    ROLLBACK
                    */

                    await tx.RollbackAsync();
                }
            }
        }

        
        /// <summary>
        /// DML
        /// </summary>
        /// <returns>
        /// RepeatableRead 격리수준에서 SELECT 시 UPDATE 잠금 대기 확인
        /// </returns>
        static async Task Transaction_Async_SELECT_Lock_WithRepeatableRead()
        {
            var p_orderkey = 100;
            var waittime = 30;

            using (SalesContext db = new SalesContext())
            {
                using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
                {
                    var order = await db.Orders.FindAsync(p_orderkey);

                    Console.WriteLine($"\nSELECT with RepeatableRead and waiting for {waittime}sec...");
                    await Task.Delay(waittime * 1000);

                    //대기 중에 SSMS에서 UPDATE 수행 시 차단 여부 확인
                    /*
                    USE SalesSimple;

                    EXEC sp_lock;

                    BEGIN TRAN
                       UPDATE dbo.Orders SET TotalPrice = 0 OUTPUT inserted.* WHERE OrderKey = 100
                    ROLLBACK
                    */

                    await tx.RollbackAsync();
                }
            }
        }


        /// <summary>
        /// 로컬 트랜잭션 - Commit 되지 않는 경우의 상태 확인
        /// </summary>
        /// <remarks>
        /// 정상적으로 Using or Dispose 수행한 경우
        /// </remarks>
        static async Task Transaction_Async_WithoutCommit_Using()
        {
            var p_orderkey = 100;

            using (SalesContext db = new SalesContext())
            {
                var order = await db.Orders.FindAsync(p_orderkey);
                order.TotalPrice += 100;

                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    var rowsAffected = await db.SaveChangesAsync();
                    Console.WriteLine($"{rowsAffected} rows affected");

                    // 1) 여기서 중단점 걸고 "휴면 세션과 트랜잭션.sql" 스크립트로 확인
                    if (1 == 2) //시나리오 재현용
                    {
                        await tx.CommitAsync();
                    }

                }//using tx
                // 2) using() 후 트랜잭션 자동 처리 여부 확인

            }//using db
        }


        /// <summary>
        /// 로컬 트랜잭션 - Commit 되지 않는 경우의 상태 확인
        /// </summary>
        /// <remarks>
        /// Using 또는 Dispose 하지 않은 경우
        /// </remarks>
        static async Task Transaction_Async_WithoutCommit_NonDispose()
        {
            var p_orderkey = 100;

            /*
             * using이나 Dispose() 하지 않는 경우
             */
            SalesContext db = new SalesContext();

            var tx = await db.Database.BeginTransactionAsync();

            var order = await db.Orders.FindAsync(p_orderkey);
            order.TotalPrice += 100;

            var rowsAffected = await db.SaveChangesAsync();
            Console.WriteLine($"{rowsAffected} rows affected");

            // 1) 여기서 중단점 걸고 "휴면 세션과 트랜잭션.sql" 스크립트로 확인
            if (1 == 2) //시나리오 재현용
            {
                await tx.CommitAsync();
            }

            // 3) 수동 Dispose() 후 트랜잭션 처리 확인
            //db.DisposeAsync();

            // 2) 함수 종료 후 트랜잭션 자동 처리 여부 확인
        }


        /// <summary>
        /// ReadUncommitted 처리
        /// </summary>
        /// <returns></returns>
        static async Task ReadUncommitted()
        {
            /*
            -- 1) SSMS에서 아래 쿼리 수행 후
            BEGIN TRAN
                UPDATE SalesSimple.dbo.Orders 
                SET OrderDate = OrderDate + 365 OUTPUT inserted.* 
                WHERE OrderKey = 100
            
            -- 3) *테스트 완료 후 ROLLBACK 처리
            IF @@TRANCOUNT > 0 ROLLBACK TRAN;
            SELECT OrderDate FROM SalesSimple.dbo.Orders WHERE OrderKey = 100;
            */
            var p_orderkey = 100;
            
            using (SalesContext db = new SalesContext())
            {
                // 2) SELECT with READUNCOMMITTED
                using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var orders = await db.Orders
                        .Where(o => o.OrderKey == p_orderkey)
                        .ToListAsync()
                        ;
                    
                    Console.WriteLine($"\nOrder: {orders[0].OrderKey}, {orders[0].OrderDate}");                    
                }
            }
        }

        
        /// <summary>
        /// farhadzm @ github에 올라온 예제 (참조해서 변경한 코드)
        /// </summary>
        /// <returns></returns>
        static async Task ReadUncommitted2()
        {
            var p_orderkey = 100;
            
            using (SalesContext db = new SalesContext()) 
            {
                List<Order> order = await db.Orders
                    .Where(o => o.OrderKey == p_orderkey)
                    .ToListWithNoLockAsync();

                Console.WriteLine($"Order: {order[0].OrderKey}, {order[0].OrderDate}");
            }
        }


        /// <summary>
        /// RCSI 동작
        /// </summary>
        /// <returns></returns>
        static async Task ReadCommittedSnapshot()
        {
            /*
             * 1) "06.READ COMMITTED SNAPSHOT DB 옵션.sql" 스크립트로 옵션 설정
             * 2) ReadUncommitted()에 있는 UPDATE 예제 사용
            */
            var p_orderkey = 100;

            using (SalesContext db = new SalesContext())
            {
                // 2) SELECT with READUNCOMMITTED
                var orders = await db.Orders
                    .Where(o => o.OrderKey == p_orderkey)
                    .ToListAsync()
                    ;

                Console.WriteLine($"\nOrder: {orders[0].OrderKey}, {orders[0].OrderDate}");
            }
        }




        /// <summary>
        /// 분산 트랜잭션 - System.Transactions.TransactionScope
        /// </summary>
        /// <remarks>
        /// 디폴트 격리수준 확인, 비동기처리 시 TransactionScopeAsyncFlowOption 옵션
        /// </remarks>
        /// <returns></returns>
        static async Task DistributedTransaction_Async_Default()
        {
            var p_orderkey = 10;
            List<Order> orders;

            //비동기 처리 시 TransactionScopeAsyncFlowOption.Enabled 지정
            //  Complete/Rollback 전에 Connection이 Logout/Login 후에 처리됨
            using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required,
                                                              TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SalesContext db = new SalesContext())
                {
                    // 비동기 처리 기준
                    orders = await db
                        .Orders
                        .Where(o => o.OrderKey <= p_orderkey)
                        .ToListAsync();

                }//using db

                //트랜잭션 완료
                tx.Complete();

                // 세션 격리수준 확인용
                Console.WriteLine($"Completed, any key to continume..."); Console.ReadKey();

            }//using tx
        }


        /// <summary>
        /// 분산 트랜잭션 - TransactionScope()
        /// <remarks>
        /// 격리수준 조정
        /// </remarks>
        /// </summary>
        static async Task DistributedTransaction_Async_IsolationLevel()
        {
            var p_orderkey = 10;
            
            var txoptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,             
            };

            using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required,
                                                                txoptions,
                                                                TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SalesContext db = new SalesContext())
                {
                    var orders = await db.Orders.Where(o => o.OrderKey <= p_orderkey).ToListAsync();
                    tx.Complete();

                    // 세션 격리수준 확인용
                    Console.WriteLine($"Completed, any key to continume..."); Console.ReadKey();
                }
            }
        }


        /// <summary>
        /// 분산 트랜잭션, TransactionScope()
        /// </summary>
        /// <remarks>
        /// Commit과 Rollback 시점이 어떻게 다른지 확인
        /// </remarks>
        /// <returns></returns>
        static async Task DistributedTransaction_Async_Timing()
        {
            var p_orderkey = 0; // 100, 0

            var txoptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
            };

            using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required,
                                                              txoptions,
                                                              TransactionScopeAsyncFlowOption.Enabled))
            {
                using (SalesContext db = new SalesContext())
                {
                    try
                    {
                        // 여기서 Debugging, 트랜잭션 완료 시점 확인
                        var order = await db.Orders.FindAsync(p_orderkey);
                        if (order != null)
                        {
                            order.TotalPrice += 100;
                            var rowsAffected = await db.SaveChangesAsync();

                            tx.Complete();
                            Console.WriteLine($"{rowsAffected} rows affected");
                        }
                        else
                        {
                            //여기에 중단점
                            tx.Dispose();
                            Console.WriteLine($"transaction rollback");
                        }
                    }
                    catch (Exception)
                    {
                        //ToDo:
                        throw;
                    }
                }//db
                //여기에 중단점
            }//tx
        }//()
        


        /// <summary>
        /// ToListWithNoLockAsync()
        /// </summary>
        /// <remarks>
        /// by farhadzm @ github가 원본
        /// by Jungsun Kim, 데모용으로 간단 코드로 수정해 봄
        ///     
        /// 주의) 아래 코드는 데모 및 학습을 위한 참고용이므로, 실제 운영에 바로 사용하면 안됩니다.
        /// 
        /// farhadzm의 원본 소개하는 글
        /// https://www.brianwalls.org/brian-walls/using-nolock-in-entity-framework-core
        /// </remarks>

        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="dbcontext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            List<T> result;

            using (var tx = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions()
                                                 {
                                                     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                                 },
                                                 TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await query.ToListAsync(cancellationToken);
                tx.Complete();
            }
            return result;
        }
        




        
        /*=====================================================================
         * 부록
         *===================================================================*/

        /// <summary>
        /// 사용자 정의 스칼라 함수와 EF Core 모델
        /// </summary>
        /// <remarks>
        /// 부록으로 다루거나 혹은 제외
        /// </remarks>
        static async Task ScalarUDFs()
        {
            var suppkey = 5;

            using (SalesContext db = new SalesContext())
            {
                var suppliers = await db
                                .Suppliers
                                .Where(s => s.Suppkey <= suppkey
                                            && db.PartsCountforSupplier(s.Suppkey) >= 1)
                                .Select(ps => ps)
                                .ToListAsync()
                                ;
            }
        }


        /// <summary>
        /// 동시성 충돌(Concurrency Conflicts) for Update
        /// </summary>
        /// <remarks>
        /// EF Core에서 제공하는 기능, 옵션, 처리들 확인
        /// </remarks>
        /// <returns></returns>
        static async Task Transaction_Async_ConcurrencyConflictsWithUpate()
        {
            var p_orderkey = 100;
            var waittime = 10;

            using (SalesContext db = new SalesContext())
            {
                using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                {
                    //TotalPrice 열에 [ConcurrencyCheck] 설정
                    var order = await db.Orders.FindAsync(p_orderkey);
                    order.TotalPrice -= 100;

                    try
                    {
                        // UPDATE SalesSimple.dbo.Orders SET TotalPrice -= 10000 OUTPUT inserted.* WHERE OrderKey = 100;
                        // SELECT *　FROM SalesSimple.dbo.Orders WHERE OrderKey = 100;
                        Console.WriteLine($"\nWaiting for {waittime}sec before SaveChanges()");
                        // 대기 중에 SSMS에서 위 쿼리로 동일 행/열 UPDATE 한 후 예외 확인
                        await Task.Delay(waittime * 1000);

                        var rowsAffected = await db.SaveChangesAsync();
                        Console.WriteLine($"\n{rowsAffected} rows affected\n");

                        await tx.CommitAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //동시성 충돌 발생
                        foreach (var entry in ex.Entries)
                        {
                            //각 엔터티별로 
                            if (entry.Entity is Order) //EntityFrameworkCore.ChangeTracking.EntityEntry.Entity
                            {
                                // 전체 열의 현재 EF Core Entity값
                                var proposedValues = entry.CurrentValues;

                                // 원래 읽은 값
                                //var originalValues = entry.OriginalValues;

                                // DB에서 변경된 값 읽기. 주의. SELECT 쿼리 추가 호출됨
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    //각 열(필드)별로 값 확인
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues[property];

                                    //현재 변경 전과 후 그리고 (필요 시) DB에서 변경값 확인 가능
                                    Console.WriteLine($"Property({property.Name}): Proposed value: {proposedValue} but Database value is {databaseValue}");
                                }

                                //데이터베이스에서 변경된 값으로 재설정 가능
                                //entry.OriginalValues.SetValues(databaseValues);
                            }
                            else
                            {
                                throw new NotSupportedException($"동시성 충돌 처리 방법을 알 수 없는 경우, {entry.Metadata.Name}");
                            }
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                    }
                }
            }
        }

        
        /// <summary>
        /// 동시성 충돌(Concurrency Conflicts) for Delete
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        static async Task Transaction_Async_ConcurrencyConflictsWithDelete()
        {
            var p_orderkey = 100;
            var p_linenumber = 1;
            var waittime = 10;

            using (SalesContext db = new SalesContext())
            {
                using (var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                {
                    //OrderDate 열에 [ConcurrencyCheck] 설정
                    var lineitem = await db.LineItems.FindAsync(p_orderkey, p_linenumber);
                    db.LineItems.Remove(lineitem);

                    try
                    {
                        // UPDATE SalesSimple.dbo.LineItems SET Quantity += 10 OUTPUT inserted.* WHERE OrderKey = 100 AND LineNumber = 1
                        // SELECT * FROM SalesSimple.dbo.LineItems WHERE OrderKey = 100 AND LineNumber = 1
                        Console.WriteLine($"\nWaiting for {waittime}sec before SaveChanges()");
                        // 대기 중에 SSMS에서 위 쿼리로 동일 행의 열 UPDATE 한 후 예외 확인
                        await Task.Delay(waittime * 1000);

                        var rowsAffected = await db.SaveChangesAsync();
                        Console.WriteLine($"\n{rowsAffected} rows affected\n");

                        await tx.CommitAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //
                        //동시성 충돌 발생 시,
                        //  1) 충돌 오류를 일으킬 수도 있고
                        //  2) (옵션) 아래와 같이 충돌 내용을 확인해서 추가 처리를 할 수도 있을 듯.
                        foreach (var entry in ex.Entries)
                        {
                            //각 엔터티별로 
                            if (entry.Entity is LineItem) //EntityFrameworkCore.ChangeTracking.EntityEntry.Entity
                            {
                                // DELETE 작업이므로(수정은 없음), 읽은 값을 참조
                                var originalValues = entry.OriginalValues;

                                // DB에서 변경된 값 읽기. 주의. SELECT 쿼리가 추가 호출됨
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in originalValues.Properties)
                                {
                                    //각 열(필드)별로 값 확인
                                    var originalValue = originalValues[property];
                                    var databaseValue = databaseValues[property];

                                    //변경 전/후 값 확인 가능
                                    Console.WriteLine($"Property({property.Name}): Original value is {originalValue} but Database value is {databaseValue}");
                                }

                                //데이터베이스에서 변경된 값으로 설정 가능
                                //entry.OriginalValues.SetValues(databaseValues);
                            }
                            else
                            {
                                throw new NotSupportedException($"동시성 충돌 처리 방법을 알 수 없는 경우, {entry.Metadata.Name}");
                            }
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                    }
                }
            }
        }


        /// <summary>
        /// 테이블 힌트, 쿼리 힌트 처리
        /// </summary>
        /// <returns></returns>
        static async Task QueryIntercept()
        {
            var p_orderkey = 100;

            using (SalesContext db = new SalesContext())
            {
                //NOLOCK 힌트 허용 (Context 생성 시 초기화)
                CommandInterceptorForHint.UseHintNOLOCK = true;

                var orders = await db.Orders
                    .Where(o => o.OrderKey == p_orderkey)
                    .ToListAsync()
                    ;

                Console.WriteLine($"\nOrder: {orders[0].OrderKey}, {orders[0].OrderDate}");
            }

            using (SalesContext db = new SalesContext())
            {
                var orders = await db.Orders
                    .Where(o => o.OrderKey == p_orderkey)
                    .ToListAsync()
                    ;

                Console.WriteLine($"\nOrder: {orders[0].OrderKey}, {orders[0].OrderDate}");
            }
        }

    }//Program class
}//namespace