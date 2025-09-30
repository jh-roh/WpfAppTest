/*
*********************************************************************
SW �����ڸ� ���� ���� ���� ���� �ۼ���

�ۼ���: ������ (jskim@sqlroad.com)
        (��)������ ��ǥ������Ʈ/�̻�
        Microsoft Data Platform MVP


���⿡ ���� �ڵ�� �������� ���� ���� ���ؼ� �����Ǵ� ���̸� 
�� �� � �����̳� å�ӵ� �����ϴ�. �׽�Ʈ�� ������ ������ � �뵵��
�ڵ带 ����� ��� ���Ǹ� ���մϴ�.

*********************************************************************
*/

/*
=====================================================================
4��. Ŀ��, ��, �Լ�
=====================================================================
���񽺿� ���������� �ִ��� ����
 ; PIVOT �������δ� GROUP BY + CASE �� Ȥ�� PIVOT ������� ����
 ; Row-to-Col ���� STRING_AGG() �� ���
 ; �� �� ������ T-SQL�� ����

�� �ʿ� �� Ʃ�� ���
 ; LOCAL
 ; FAST_FOWARD ��õ
  - tempdb/worktable ���� ����
  - ����
   => Cursor ������ ���� ���� ��ȹ ���ÿ� ����, �� ����� Better or Worse
   => ���� Ȯ�� �� ���� �ʿ� (DBA/QA �� ������ ����)

*/
USE EPlan
GO

SET STATISTICS IO ON


/*
------------------------------------------------------------
Cursor ��� ���� ����
-------------------------------------------------------------
*/
-- 1) Cursor�� �̿��� Pivoting
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
2) T-SQL: CASE + GROUP BY�� ������ ���
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
3) T-SQL: PIVOT ���� ������ ���
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


-- STRING_AGG() Ȱ�� - 2017+
SELECT STRING_AGG(CustomerID, ',') AS CustomerIDs 
FROM Northwind.dbo.Orders
WHERE OrderID <= 10250;



/*
-------------------------------------------------------------
Curosr �ɼ� ����
*/
USE Northwind;
GO

ALTER TABLE Products
  ADD ProductTotalQty int 
GO


/*
1) �⺻ Cursor
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

-- Ȯ��
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

-- ����
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
�� - ���� ��

���� View�� ����
��
 1) �뷮 Table�� Join �� View
 2) ���� ���� ���̺��� UNION �� View

���ʿ��� IO ����
���� ��ȹ ����ȭ ����


���� ��ø View

���
 - View �ȿ� View �ȿ� View�ȿ� View�ȿ� ���̺�
��ø ����
 - 1 �ܰ踸 ��� ��õ (������ �ǰ�)
 - SQL Server������ 2~3�ܰ����

���� View�� ���� �뵵 (������ �ǰ�)
 - ���� Entity(���̺�) �뵵
 - ���� ���� ���Ǹ� ���� ������ ��÷ View�� �� ��õ

View ������ Index �� ���� ����
 - View ��� �˻� ���� Index ���� ���
  ; View ���ο��� ���� �� Non-SARG ���� - SARG �����ϵ��� ����
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
�� - View ������ Index �� ���� ����
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
 - �� ������ �ݺ� ȣ�� - �뷮 ��� ���տ��� ȣ�� �� ū ����
  ; �Լ� ���ο� SELECT ���� ���� �� �˻� �� �� ����ϴ� I/O ����
  ; �ܼ� ���� �Լ��� ��� CPU ���� �߻�
 - ���� �����
  ; SELECT ������ ��� �ʿ� �� ���� ��ü�� ����(�Լ� ����)
    => ����. SQL Server 2019���� "Scalar UDF Inline" �ڵ� Ʃ�� ����(���� ������ ����) 
  ; �Լ� ���� ������ ���� �ִ��� �ܼ��ϰ�

B. Table Valued Function
 1) Inline Table-Valued Function
   ; View ��ü������ Ȱ�� ����, "�Ű� ������ ���� View"
     ; Partitioned View ��ä�����ε� Ȱ�� ����

 2) Multi-statement Table-Valued Function
   ; ���� ���ϰ� ū ��ü
     ��. Split()
   ; �뷮 ���� ȣ�� ȯ�濡���� �� ���� or �ݵ�� Ʃ�� ���
     ; ���� �� �� ���� ���� ����
	    - ����. SQL Server 2017���� "Interleaved Execution for MSTVF" �ڵ� Ʃ�� ����

-------------------------------------------------------------
*/
/*
-------------------------------------------------------------
Scalar Function ����

�� ���� �ݺ� ȣ�� ����
 - �Լ� ���� SELECT ���� ����
"Scalar UDF Inline" ���� ����
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
  �������� ��ü I/O ��
*/
SELECT SUM(Quantity)
    FROM [Order Details]
    WHERE ProductID = 1

--'Order Details' ���̺�. ��ĵ �� 1, ���� �б� �� 11, ������ �б� �� 0, �̸� �б� �� 0.

/*
SELECT ������ �Լ� ȣ�� �� I/O
*/
SELECT ProductName, dbo.fn_OrderSum(ProductID)
FROM dbo.Products
WHERE ProductID <= 5
--���̺� 'Products'. �˻� �� 1, ���� �б� �� 2, ������ �б� �� 1, �̸� �б� �� 0, LOB ���� �б� �� 0, LOB ������ �б� �� 0, LOB �̸� �б� �� 0.

--/* SQL Server 2019 "TSQL_SCALAR_UDF_INLINING" ��� ���� ����
OPTION(USE HINT('DISABLE_TSQL_SCALAR_UDF_INLINING'))  
--*/


/*
vs. �Լ����� �������� ���� ó���� ���
*/
SELECT ProductName
  , ( SELECT SUM(Quantity)
       FROM [Order Details] d 
       WHERE d.ProductID = p.ProductID )
FROM dbo.Products AS p
WHERE ProductID <= 5

/*
�Ʒ� ����� �����ȹ�� ���̺� ������ ���� �޶���
*/
--���̺� 'Order Details'. ��ĵ �� 5, ���� �б� 55, ���� �б� 0, ������ ���� �б� 0, �̸� �б� �б� 0, ������ ���� �̸� �б� �б� 0, lob ���� �б� 0, lob ���� �б� 0, lob ������ ���� �б� 0, lob �̸� �б� �б� 0, lob ������ ���� �̸� �б� �б� 0.
--���̺� 'Products'. ��ĵ �� 1, ���� �б� 2, ���� �б� 0, ������ ���� �б� 0, �̸� �б� �б� 0, ������ ���� �̸� �б� �б� 0, lob ���� �б� 0, lob ���� �б� 0, lob ������ ���� �б� 0, lob �̸� �б� �б� 0, lob ������ ���� �̸� �б� �б� 0.


--Scalar Function - ���� ������ ���� �ܼ�ȭ
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
���� FORMAT() ���� ���
 - �ҷ� SELECT������ ������
 - �뷮 SELECT ���� ����
   ; IO, CPU ���� ��������� ŭ
   ; �ٸ� �Լ��� ������� ����
*/
SELECT TOP(10000) FORMAT(OrderKey, '0000000000')
FROM SalesSimple.dbo.Orders;

/*
-------------------------------------------------------------
Inline Table-Valued Function 

Ư¡
 - �Ű������� ���� View
   ; �˻�(index)���� ���ȭ ����
 - APPLY ���� ����
 - Partitioned View ��ɰ� ����
   ; ���� ���� ���͸� ����
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
����
*/
DROP FUNCTION IF EXISTS dbo.fn_OrderSum;
DROP FUNCTION IF EXISTS dbo.if_Orders;


/*
�ٽ� ���
- Cursor
 ; �ݵ�� �ʿ��� ��� �ܴ� T-SQL�� ����
- View
 ; R-DB �𵨿� �°�
 ; ������ ��ø�� ���ϱ�
 ; ���ʿ���/������ JOIN/UNION ���ϱ�
 ; �ܺ� ���� �������� Non-SARG ����

- FUNCTION
 ; �ܼ��ϰ�
 ; �߿� ���� ���� ���Ǵ� �Լ��� ���ϰų� Ʃ��
 ; �Լ� �� SELECT ������ ���� ����� ū��
 ; SQL Server 2017/2019 �ڵ� Ʃ�� ��� ����

*/

/*
���� Ʃ�� �н�
 - ��Ű��ó ����
   ; Query Optimizing / Index / Statistics / Lock
 - ���� ���� ���� �м�
 - �ε��� Ʃ��
 - ���� Ʃ��
*/