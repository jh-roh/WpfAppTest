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
3장. 더 좋은 쿼리 작성하기
=====================================================================
*/

/*
기본 SQL 권장 사항
Join 작성 시 권장 사항
SubQuery 작성 시 권장 사항
파생 테이블,CTE,APPLY 활용
잠금차단회피
사례들
*/


USE Northwind
GO

SET STATISTICS IO ON

/*
-------------------------------------------------------------
IN vs. BETWEEN
-------------------------------------------------------------
*/

/*
의미와 용도에 맞게 선택
IN
 - Equal(=) 조건과 OR 연산(합집합) 결합
  ; Random Access(IO) 동작
  ; 검색 대상 값이 많을 수록 인데스 사용기회 감소
  ; DISTNCT/GROUP BY 등은 불필요
 - 비 연속 값 검색 시
 - 검색 대상이 적은 경우 적합
 - EQUAL(=) 조건이 필요한 경우

BETWEEN
 - Non-Equal(>= <=) 조건과 AND 연산 결합
  ;Sequence Access 동작
 - 연속 값 검색 시
*/






-- IN 이해
-- Nonclustered Index(PK, OrderID)
SELECT * 
FROM EPlanHeap.dbo.Orders 
WHERE OrderID IN (10248, 10249, 10250, 10251
					 , 10252, 10253)
GO

-- BETWEEN 이해
SELECT * 
FROM EPlanHeap.dbo.Orders 
WHERE OrderID BETWEEN 10248 AND 10253
GO



/*
TOP(N) 활용
-쿼리 최적화에 도움
-결과 집합 제한에 활요
 TOP(1)
  ; MIN/MAX vs. TOP(1) - NULL 값의 처리 방식 이해
 TOP(N)
  ; Paging쿼리(ex. 게시판)
  ; 전체 결과 집합 제한(ex. 뉴스, 이벤트 알림, 이력 데이터)

TOP + ORDER BY 절 주의
 - ORDER BY 절 생략?
  ; 기본적으로 지정
  ; Clustered Index 고려하지 말것
    ;NOLOCK 힌트 등이 사용될 경우 데이터 정합성 오류 발생 가능
 - 정렬 데이터의 유일설 보장 필요
  ;아니면 데이터 일관성 오류 발생 가능 
*/

/*
-------------------------------------------------------------
TOP + ORDER BY 절 주의
*/
SELECT TOP(5) Quantity, OrderID, ProductID
FROM EPlan.dbo.[Order Details]
ORDER BY Quantity DESC;

   SELECT TOP(12) Quantity, OrderID, ProductID
   FROM EPlan.dbo.[Order Details]
   ORDER BY Quantity DESC--, OrderID ASC;



/*
-------------------------------------------------------------
집계함수와 GROUP BY
-------------------------------------------------------------
*/

/*
-------------------------------------------------------------
COUNT vs. EXISTS

 용도에 맞는 구현
  - 집계(전체 검색) vs. 데이터 존재 여부 체크(부분 검색)
*/


/*
COUNT 주의
 - 데이터 무결성 고려
  1) NULL 허용 열인 경우, COUNT(열)
  2) NOT NULL 열인 경우 두가지는 동일 동작
*/

USE EPlan
GO

CREATE INDEX IX_OD_Quantity
ON EPlan.dbo.[Order Details] (Quantity)
GO

-- I/O 비교
IF (SELECT COUNT(*) 
	FROM Eplan.dbo.[Order Details] 
	WHERE Quantity > 50) > 0

  PRINT '있을까 없을까?'
GO

IF EXISTS (SELECT * 
		FROM Eplan.dbo.[Order Details] 
		WHERE Quantity > 50)

  PRINT '있을까 없을까?'


-- 정리
DROP INDEX IX_OD_Quantity ON EPlan.dbo.[Order Details];



/*
-------------------------------------------------------------
NULL 고려한 집계 연산

 - 대량 NULL 값을 가진 열의 경우
  ; 불필요한 NULL 데이터 사전 필터링 코드 추가


*/
USE EPlan
GO

CREATE INDEX IX_BigOrders_Freight
ON EPlan.dbo.BigOrders (Freight)
GO


SELECT SUM(Freight) FROM dbo.BigOrders
GO
SELECT SUM(Freight) FROM dbo.BigOrders
WHERE Freight IS NOT NULL
GO

SELECT MIN(Freight) FROM dbo.BigOrders;
GO
SELECT MIN(Freight) FROM dbo.BigOrders
WHERE Freight IS NOT NULL


DROP INDEX IX_BigOrders_Freight ON EPlan.dbo.BigOrders;



/*
불필요한 GROUP BY 열 제거
*/
SELECT c.CustomerID, CompanyName, COUNT(*)
FROM dbo.Customers AS c INNER JOIN dbo.Orders AS o
		ON c.CustomerID = o.CustomerID
GROUP BY c.CustomerID, CompanyName
GO

SELECT c.CustomerID, MAX(CompanyName), COUNT(*)
FROM dbo.Customers AS c INNER JOIN dbo.Orders AS o
		ON c.CustomerID = o.CustomerID
GROUP BY c.CustomerID



/*
-------------------------------------------------------------
UNION vs. UNION ALL
-------------------------------------------------------------
UNION 은 기본적으로 Distinct 연산(부하) 발생 가능
 ; 행 유일성이 명확한 경우 Query Optimizer가 자동 조정 가능하나
 ; 명확한 경우 명시적으로 ALL 지정

*/
SELECT firstname, city
 FROM Northwind.dbo.Employees
UNION
SELECT companyname, city
 FROM Northwind.dbo.Customers
GO

SELECT firstname, city
 FROM Northwind.dbo.Employees
UNION ALL
SELECT companyname, city
 FROM Northwind.dbo.Customers



/*
-------------------------------------------------------------
UPDATE SET 
-------------------------------------------------------------
UPDATE 결과 열 값 반환

UPDATE...SET 절의 다양한 기능 활용
UPDATE 채번
	    SET 일련번호 = 일련번호 + 1
WHERE .....

SELECT @일련번호 = 일련번호
FROM 채번 WITH(NOLOCK)
WHERE ....

RETURN @일련번호

위와 같은 코드를 아래와 같이 간단하게 사용가능

UPDATE 채번
		SET @일련번호 = 일련번호 = 일련번호 + 1
WHERE .....


*/
DECLARE @OrderDate datetime;

BEGIN TRAN
   SELECT @@trancount;

   UPDATE dbo.Orders
   SET @OrderDate = OrderDate = OrderDate + 365
   WHERE OrderID = 10248;

   SELECT @OrderDate;

ROLLBACK


/*
-------------------------------------------------------------
DML OUTPUT
*/

/*
DML 작업 결과 행 반환

INSERT/UPDATE/DELETE/MERGE 결과 행 반환
 - 과거엔 후속 SELECT 쿼리로 처리(UPDATE + SELECT)
 - 현재는 OUTPUT 절 활용 가능 (UPDATE outpu)
   ; 단순 결과 반환인 경우
   ; 혹은 테이블에 직접 입력 후 재사용도 가능
*/

BEGIN TRAN
   SELECT @@trancount;

   UPDATE dbo.Orders
   SET OrderDate = OrderDate + 365
   OUTPUT 'inserted', inserted.*, 'deleted', deleted.*
   WHERE OrderID = 10248;

ROLLBACK



/*
-------------------------------------------------------------
JOIN
-------------------------------------------------------------
*/
/*
-------------------------------------------------------------
재미난 퀴즈
-------------------------------------------------------------
*/
SELECT *
FROM dbo.Orders AS o 
INNER JOIN dbo.[Order Details] AS d
   ON o.OrderID = d.OrderID
WHERE o.OrderID <= 10249

SELECT *
FROM dbo.Orders AS o 
INNER JOIN dbo.[Order Details] AS d
   ON o.OrderID = d.OrderID
WHERE d.OrderID <= 10249


/*
-------------------------------------------------------------
중첩 반복(Nested Loop) JOIN 기본
*/
CREATE INDEX IX_BigOrders_CustomerID
ON dbo.BigOrders(CustomerID);
GO

SELECT c.CustomerID, c.CompanyName, o.OrderID, o.OrderDate
FROM dbo.Customers AS c INNER JOIN dbo.BigOrders AS o
    ON c.CustomerID = o.CustomerID
WHERE c.CustomerID IN ('VINET', 'VICTE');



/*
-------------------------------------------------------------
Outer Join - 조인 순서 유도
*/
SELECT s.SupplierID, p.ProductID, p.ProductName, p.UnitPrice 
FROM dbo.Suppliers AS s INNER JOIN dbo.Products AS p
  ON s.SupplierID = p.SupplierID
WHERE p.SupplierID = 2


SELECT s.SupplierID, p.ProductID, p.ProductName, p.UnitPrice 
FROM dbo.Suppliers AS s RIGHT OUTER JOIN dbo.Products AS p
  ON s.SupplierID = p.SupplierID
WHERE p.SupplierID = 2



/*
-------------------------------------------------------------
예제 - 언제 Subquery를 사용할 것인가?
-------------------------------------------------------------
*/
USE EPlan

-- 기본 조인
SELECT DISTINCT c.CompanyName
FROM dbo.Customers AS c INNER JOIN dbo.BigOrders AS o
ON c.CustomerID = o.CustomerID
GO


-- 서브쿼리
SELECT c.CompanyName
FROM dbo.Customers AS c
WHERE EXISTS (SELECT *
   FROM dbo.BigOrders AS o
   WHERE c.CustomerID = o.CustomerID)



/*
---------------------------------------------------------------------
파생테이블(인라인 뷰), CTE, APPLY
---------------------------------------------------------------------
*/
/*
예제 - SELECT절 Subquery 중복 IO
*/
SELECT OrderID
,  (
   SELECT COUNT(*) FROM dbo.[Order Details] AS d
   WHERE d.OrderID = o.OrderID
   ) AS OrderCnt
,  (
    SELECT SUM(Quantity) FROM dbo.[Order Details] AS d
   WHERE d.OrderID = o.OrderID
   ) AS QuantitySum

FROM dbo.Orders AS o
WHERE OrderID = 10248;


/*
---------------------------------------------------------------------
1) 파생테이블(인라인 뷰)을 이용한 경우
---------------------------------------------------------------------
*/
SELECT o.OrderID, OrderCnt, QuantitySum
FROM dbo.Orders AS o
LEFT JOIN (
   SELECT OrderID
      ,  COUNT(*) AS OrderCnt
      ,  SUM(Quantity) AS QuantitySum
   FROM dbo.[Order Details] AS d
   GROUP BY OrderID
   ) AS d
ON d.OrderID = o.OrderID
WHERE o.OrderID = 10248;



/*
---------------------------------------------------------------------
2) CTE를 이용한 경우
---------------------------------------------------------------------
*/
WITH ODSum(OrderID, OrderCnt, QuantitySum)
AS
(
   SELECT OrderID
      ,  COUNT(*)
      ,  SUM(Quantity) 
   FROM dbo.[Order Details] AS d
   GROUP BY OrderID
)
SELECT o.OrderID, OrderCnt, QuantitySum
FROM dbo.Orders AS o
LEFT JOIN ODSum AS d
	ON d.OrderID = o.OrderID
WHERE o.OrderID = 10248;



/*
---------------------------------------------------------------------
3) (CROSS|OUTER) APPLY를 활용한 방법
---------------------------------------------------------------------
*/
SELECT o.OrderID, OrderCnt, QuantitySum
FROM dbo.Orders AS o
OUTER APPLY (
   SELECT 
         COUNT(*) AS OrderCnt
      ,  SUM(Quantity) AS QuantitySum
   FROM dbo.[Order Details] AS d
   WHERE d.OrderID = o.OrderID
   ) AS d
WHERE o.OrderID = 10248;



/*
----------------------------------------------------------------------------
CASE 내 Subquery 문제
*/
SELECT
	OrderID, 
	CASE (SELECT Country FROM dbo.Customers cu WHERE cu.CustomerID = oh.CustomerID) 
		WHEN 'Germany' THEN 'Germany'    
		WHEN 'Mexico' THEN 'Mexico'
		WHEN 'UK' THEN 'UK'
		ELSE 'N/A'    
	END
FROM dbo.Orders AS oh
WHERE OrderID <= 10250;


/*
권장 - Subquery 내 CASE
*/
SELECT
	OrderID, 
	(SELECT CASE Country 
				WHEN 'Germany' THEN 'Germany'    
				WHEN 'Mexico' THEN 'Mexico'
				WHEN 'UK' THEN 'UK'
				ELSE 'N/A'    
			END	
	FROM dbo.Customers cu 
	WHERE cu.CustomerID = oh.CustomerID) AS Country
FROM dbo.Orders AS oh
WHERE OrderID <= 10250;


/*
-------------------------------------------------------------
CTE 재귀호출
*/
/*
SELECT EmployeeID, ReportsTo, * FROM dbo.Employees;
*/
WITH RCTE
AS 
(
   SELECT e.EmployeeID, e.ReportsTo, e.Title
   FROM dbo.Employees AS e
   WHERE e.EmployeeID = 9

   UNION ALL

   SELECT e.EmployeeID, e.ReportsTo, e.Title
   FROM dbo.Employees AS e INNER JOIN RCTE AS r
      ON e.EmployeeID = r.ReportsTo
)
SELECT *
FROM RCTE;


/*
-------------------------------------------------------------
잠금 차단 고려 – 두 가지 선택지
-------------------------------------------------------------
*/
