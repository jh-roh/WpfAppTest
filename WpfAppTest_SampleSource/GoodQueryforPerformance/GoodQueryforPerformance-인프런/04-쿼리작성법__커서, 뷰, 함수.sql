/*
*********************************************************************
SW 개발자를 위한 성능 좋은 쿼리 작성법

작성자: 김정선 (jskim@sqlroad.com)
        (주)씨퀄로 대표컨설턴트/이사
        Microsoft Data Platform MVP


여기에 사용된 코드와 정보들은 단지 데모를 위해서 제공되는 것이며 
그 외 어떤 보증이나 책임도 없습니다. 테스트나 개발을 포함해 어떤 용도로
코드를 사용할 경우 주의를 요합니다.

*********************************************************************
*/

/*
=====================================================================
4장. 커서, 뷰, 함수
=====================================================================
서비스용 쿼리에서는 최대한 금지
 ; PIVOT 목적으로는 GROUP BY + CASE 문 혹은 PIVOT 명령으로 구현
 ; Row-to-Col 등은 STRING_AGG() 등 사용
 ; 그 외 가능한 T-SQL로 구현

꼭 필요 시 튜닝 고려
 ; LOCAL
 ; FAST_FOWARD 추천
  - tempdb/worktable 부하 제거
  - 주의
   => Cursor 유형에 따라 실행 계획 선택에 영향, 그 결과가 Better or Worse
   => 실제 확인 후 조정 필요 (DBA/QA 등 전문가 지원)

*/
USE EPlan
GO

SET STATISTICS IO ON


/*
------------------------------------------------------------
Cursor 사용 권장 사항
-------------------------------------------------------------
*/
-- 1) Cursor를 이용한 Pivoting
SET NOCOUNT ON
GO

IF OBJECT_ID('tempdb.dbo.#Order_Cnt') IS NOT NULL
	DROP TABLE #Order_Cnt

CREATE TABLE #Order_Cnt (
  EmpID  int
, Emp_Cnt  int
, France   int
, Germany  int
, Argentina   int
)

DECLARE EmployeeInfo CURSOR
FOR SELECT DISTINCT EmployeeID FROM dbo.Orders

OPEN EmployeeInfo

DECLARE @EmployeeID int
FETCH NEXT FROM EmployeeInfo INTO @EmployeeID
WHILE @@FETCH_STATUS = 0
BEGIN
  
	INSERT #Order_Cnt
	SELECT 
			@EmployeeID
	  , Emp_Cnt = (SELECT COUNT(EmployeeID) 
	                FROM dbo.Orders    
	                WHERE EmployeeID = @EmployeeID
	                  AND ShipCountry IN ('France', 'Germany', 'Argentina') )

	  , [France] = ( SELECT COUNT(EmployeeID) 
	                 FROM dbo.Orders   
	                 WHERE EmployeeID = @EmployeeID
							AND ShipCountry = 'France' )
	
	  , [Germany] = (SELECT COUNT(EmployeeID) 
	                 FROM dbo.Orders   
	                 WHERE EmployeeID = @EmployeeID
							AND ShipCountry = 'Germany' )  
	
	  , [Argentina] = (SELECT COUNT(EmployeeID) 
	                 FROM dbo.Orders 
	                 WHERE EmployeeID = @EmployeeID
							AND ShipCountry = 'Argentina' )  

  FETCH NEXT FROM EmployeeInfo INTO @EmployeeID
END

CLOSE EmployeeInfo
DEALLOCATE EmployeeInfo

SELECT * FROM #Order_Cnt
ORDER BY EmpID;

SET NOCOUNT OFF
GO


/*
2) T-SQL: CASE + GROUP BY로 구현한 경우
*/
SELECT EmployeeID
	, EmpCnt = COUNT(EmployeeID)
	, [France]= COUNT(CASE ShipCountry WHEN 'France' THEN 1 END)
	, [Germany] = COUNT(CASE ShipCountry WHEN 'Germany' THEN 1 END)
	, [Argentina] = COUNT(CASE ShipCountry WHEN 'Argentina' THEN 1 END)
FROM Orders 
WHERE 
	ShipCountry IN ('France', 'Germany', 'Argentina')
GROUP BY EmployeeID
ORDER BY EmployeeID;


/*
3) T-SQL: PIVOT 으로 구현한 경우
*/
SELECT EmployeeID
	, EmpCnt = [France]+[Germany]+[Argentina]
	, [France], [Germany], [Argentina]
FROM (
	SELECT EmployeeID, ShipCountry 
	FROM Orders
) AS Orders
PIVOT (
	COUNT(ShipCountry) FOR ShipCountry IN ([France], [Germany], [Argentina])
) AS Pivots
ORDER BY EmployeeID;



/*
-------------------------------------------------------------
--  Row to Column
-------------------------------------------------------------
*/
SELECT CustomerID
FROM Northwind.dbo.Orders
WHERE OrderID <= 10250;


-- STRING_AGG() 활용 - 2017+
SELECT STRING_AGG(CustomerID, ',') AS CustomerIDs 
FROM Northwind.dbo.Orders
WHERE OrderID <= 10250;



/*
-------------------------------------------------------------
Curosr 옵션 조정
*/
USE Northwind;
GO

ALTER TABLE Products
  ADD ProductTotalQty int 
GO


/*
1) 기본 Cursor
*/
DECLARE ProductQty CURSOR
FOR SELECT ProductID, Quantity FROM dbo.[Order Details]

OPEN ProductQty

DECLARE @ProductID int, @Quantity int
FETCH NEXT FROM ProductQty INTO @ProductID, @Quantity
WHILE @@FETCH_STATUS = 0
BEGIN
    
   UPDATE Products
   SET ProductTotalQty = ISNULL(ProductTotalQty, 0) + @Quantity
   WHERE ProductID = @ProductID

   FETCH NEXT FROM ProductQty INTO @ProductID, @Quantity
END

CLOSE ProductQty
DEALLOCATE ProductQty
GO

-- 확인
SELECT ProductTotalQty, * FROM dbo.Products;


/*
2) Local + FAST_FORWARD
*/
DECLARE ProductQty CURSOR LOCAL FAST_FORWARD -- FAST_FORWARD or FORWARD_ONLY STATIC READ_ONLY
FOR SELECT ProductID, Quantity FROM dbo.[Order Details]

OPEN ProductQty

DECLARE @ProductID int, @Quantity int
FETCH NEXT FROM ProductQty INTO @ProductID, @Quantity
WHILE @@FETCH_STATUS = 0
BEGIN
    
   UPDATE Products
   SET ProductTotalQty = ProductTotalQty + @Quantity
   WHERE ProductID = @ProductID

   FETCH NEXT FROM ProductQty INTO @ProductID, @Quantity
END

CLOSE ProductQty
DEALLOCATE ProductQty
GO

-- 정리
ALTER TABLE Products
  DROP COLUMN ProductTotalQty;



/*
-------------------------------------------------------------
View
-------------------------------------------------------------
*/
USE Northwind
GO

/*
예 - 만능 뷰

만능 View는 위험
예
 1) 대량 Table을 Join 한 View
 2) 많은 원격 테이블을 UNION 한 View

불필요한 IO 유발
실행 계획 최적화 방행


과자 중첩 View

사례
 - View 안에 View 안에 View안에 View안에 테이블
중첩 수준
 - 1 단계만 사용 추천 (개인적 의견)
 - SQL Server에서는 2~3단계까지

참고 View의 권장 용도 (개인적 의견)
 - 가상 Entity(테이블) 용도
 - 단지 개발 편의를 위한 목적의 중첨 View는 비 추천

View 내에서 Index 열 변형 주의
 - View 대상 검색 열이 Index 열인 경우
  ; View 내부에서 변형 시 Non-SARG 가능 - SARG 만족하도록 구현
*/
CREATE OR ALTER VIEW dbo.vi_Orders
AS
SELECT o.OrderID, o.CustomerID, o.OrderDate, e.LastName
  ,  c.CompanyName, c.Address
FROM dbo.Customers AS c 
  INNER JOIN dbo.Orders AS o
    ON c.CustomerID = o.CustomerID
  INNER JOIN dbo.Employees AS e
    ON e.EmployeeID = o.EmployeeID
GO

-- View vs. Join
SELECT OrderID, CustomerID, OrderDate
FROM dbo.vi_Orders
GO

SELECT o.OrderID, o.CustomerID, o.OrderDate
FROM dbo.Orders AS o


/*
예 - View 내에서 Index 열 변형 주의
*/
CREATE OR ALTER VIEW dbo.vTest
AS
SELECT OrderID
   ,  CONVERT(varchar(10), OrderDate, 112) AS OrderDay
   ,  OrderDate
FROM dbo.Orders
GO

SELECT *
FROM dbo.vTest A
WHERE OrderDay = '19960704';



/*
-------------------------------------------------------------
User Defined Function

Scalr Function
 - 행 단위로 반복 호출 - 대량 결과 집합에서 호출 시 큰 부하
  ; 함수 내부에 SELECT 쿼리 포함 시 검색 행 수 비례하는 I/O 유발
  ; 단순 연산 함수의 경우 CPU 부하 발생
 - 권장 방법들
  ; SELECT 쿼리의 경우 필요 시 쿼리 자체에 병합(함수 제거)
    => 참고. SQL Server 2019부터 "Scalar UDF Inline" 자동 튜닝 지원(선행 조건이 복잡) 
  ; 함수 내부 로직과 연산 최대한 단순하게

B. Table Valued Function
 1) Inline Table-Valued Function
   ; View 대체용으로 활용 권장, "매개 변수를 가진 View"
     ; Partitioned View 대채용으로도 활용 권장

 2) Multi-statement Table-Valued Function
   ; 가장 부하가 큰 개체
     예. Split()
   ; 대량 동시 호출 환경에서는 비 권장 or 반드시 튜닝 요망
     ; 예상 행 수 추정 오류 주의
	    - 참고. SQL Server 2017부터 "Interleaved Execution for MSTVF" 자동 튜닝 지원

-------------------------------------------------------------
*/
/*
-------------------------------------------------------------
Scalar Function 동작

행 단위 반복 호출 동작
 - 함수 내부 SELECT 쿼리 포함
"Scalar UDF Inline" 동작 이해
*/
USE Northwind
GO

CREATE OR ALTER FUNCTION dbo.fn_OrderSum
( @ProductID int )
RETURNS int
AS
BEGIN
  RETURN (
    SELECT SUM(Quantity)
    FROM [Order Details]
    WHERE ProductID = @ProductID
  )
END
GO


/*
  서브쿼리 자체 I/O 평가
*/
SELECT SUM(Quantity)
    FROM [Order Details]
    WHERE ProductID = 1

--'Order Details' 테이블. 스캔 수 1, 논리적 읽기 수 11, 물리적 읽기 수 0, 미리 읽기 수 0.

/*
SELECT 절에서 함수 호출 시 I/O
*/
SELECT ProductName, dbo.fn_OrderSum(ProductID)
FROM dbo.Products
WHERE ProductID <= 5
--테이블 'Products'. 검색 수 1, 논리적 읽기 수 2, 물리적 읽기 수 1, 미리 읽기 수 0, LOB 논리적 읽기 수 0, LOB 물리적 읽기 수 0, LOB 미리 읽기 수 0.

--/* SQL Server 2019 "TSQL_SCALAR_UDF_INLINING" 기능 적용 여부
OPTION(USE HINT('DISABLE_TSQL_SCALAR_UDF_INLINING'))  
--*/


/*
vs. 함수없이 쿼리에서 직접 처리한 경우
*/
SELECT ProductName
  , ( SELECT SUM(Quantity)
       FROM [Order Details] d 
       WHERE d.ProductID = p.ProductID )
FROM dbo.Products AS p
WHERE ProductID <= 5

/*
아래 결과는 실행계획의 테이블 순서에 따라 달라짐
*/
--테이블 'Order Details'. 스캔 수 5, 논리적 읽기 55, 실제 읽기 0, 페이지 서버 읽기 0, 미리 읽기 읽기 0, 페이지 서버 미리 읽기 읽기 0, lob 논리적 읽기 0, lob 실제 읽기 0, lob 페이지 서버 읽기 0, lob 미리 읽기 읽기 0, lob 페이지 서버 미리 읽기 읽기 0.
--테이블 'Products'. 스캔 수 1, 논리적 읽기 2, 실제 읽기 0, 페이지 서버 읽기 0, 미리 읽기 읽기 0, 페이지 서버 미리 읽기 읽기 0, lob 논리적 읽기 0, lob 실제 읽기 0, lob 페이지 서버 읽기 0, lob 미리 읽기 읽기 0, lob 페이지 서버 미리 읽기 읽기 0.


--Scalar Function - 내부 로직과 연산 단순화
CREATE OR ALTER FUNCTION dbo.fn_Complex
(@date DATETIME)
RETURNS DATETIME
AS
BEGIN
	DECLARE @d VARCHAR(40)
	SET @d = CONVERT(VARCHAR(10), @date - 30, 21)
	--SET @d = REPLACE(@d,'-','')
	--SET @d = @d + '00:00:00'

	RETURN @d
END

SELECT TOP 5000
	o1.OrderDate, dbo.fn_Complex(o1.OrderDate)
FROM dbo.BigOrders o1

SELECT * from dbo.BigOrders


SELECT TOP 5000
	o1.OrderDate, CONVERT(VARCHAR(10), o1.OrderDate - 30, 21)
FROM dbo.BigOrders o1


/*
참고 FORMAT() 성능 고려
 - 소량 SELECT에서는 괜찮음
 - 대량 SELECT 에서 문제
   ; IO, CPU 부하 상대적으로 큼
   ; 다른 함수나 방식으로 구현
*/
SELECT TOP(10000) FORMAT(OrderKey, '0000000000')
FROM SalesSimple.dbo.Orders;

/*
-------------------------------------------------------------
Inline Table-Valued Function 

특징
 - 매개변수를 가진 View
   ; 검색(index)열의 상수화 유도
 - APPLY 문과 연계
 - Partitioned View 기능과 연계
   ; 실행 시점 필터링 지원
*/
CREATE OR ALTER FUNCTION dbo.if_Orders
( @date datetime )
RETURNS table
AS
RETURN 
    SELECT CustomerID, EmployeeID, COUNT(*) AS Cnt
    FROM   dbo.Orders
    WHERE  OrderDate >= @date
    GROUP BY CustomerID, EmployeeID
GO

SELECT * FROM dbo.if_Orders ('19980506');


/*
-------------------------------------------------------------
정리
*/
DROP FUNCTION IF EXISTS dbo.fn_OrderSum;
DROP FUNCTION IF EXISTS dbo.if_Orders;


/*
핵심 요약
- Cursor
 ; 반드시 필요한 경우 외는 T-SQL로 구현
- View
 ; R-DB 모델에 맞게
 ; 과도한 중첩은 피하기
 ; 불필요한/과도한 JOIN/UNION 피하기
 ; 외부 쿼리 참조열의 Non-SARG 주의

- FUNCTION
 ; 단순하게
 ; 중요 조건 절에 사용되는 함수는 피하거나 튜닝
 ; 함수 내 SELECT 쿼리가 문제 생기면 큰일
 ; SQL Server 2017/2019 자동 튜닝 기능 도움

*/

/*
쿼리 튜닝 학습
 - 아키텍처 이해
   ; Query Optimizing / Index / Statistics / Lock
 - 쿼리 성능 진단 분석
 - 인덱스 튜닝
 - 쿼리 튜닝
*/