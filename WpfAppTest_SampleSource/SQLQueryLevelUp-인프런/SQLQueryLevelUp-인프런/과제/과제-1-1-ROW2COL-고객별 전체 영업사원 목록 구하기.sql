/*
------------------------------------------------------------------------------
과제 - ROW-TO-COLUMN

내용: 	전체 고객에 대해 거래 담당 영업사원 전체 코드를 ','로 구분해서 하나의 열(column)로 출력
		단, 중복 EmployeeID는 하나만 출력, 주문 없으면 NULL로 출력
------------------------------------------------------------------------------
*/
USE Northwind;
GO


SELECT CustomerID, EmployeeID
FROM dbo.Orders
WHERE CustomerID = 'FRANS';



/*
과제-1. 
*/
SELECT 
	CustomerID
,	EmpList = 
FROM dbo.Customers AS c
ORDER BY EmpList;


/*
과제-2.
*/

/*
해답-1
*/
SELECT 
	CustomerID
,	EmpList = (
				SELECT CAST(EmployeeID AS varchar(10)) + ','
				FROM 
				(
					SELECT DISTINCT EmployeeID
					FROM dbo.Orders AS o
					WHERE o.CustomerID = c.CustomerID
				) AS e
				ORDER BY EmployeeID
				FOR XML PATH('')
			  )
FROM dbo.Customers AS c
ORDER BY EmpList;


/*
해답-2
*/
WITH EmpListbyCustID AS
(
	SELECT c.CustomerID, o.EmployeeID
	FROM Customers AS c LEFT JOIN dbo.Orders AS o
	ON c.CustomerID = o.CustomerID
	GROUP BY c.CustomerID, o.EmployeeID
)

SELECT
  CustomerID
, STRING_AGG(EmployeeID, ',') WITHIN GROUP (ORDER BY EmployeeID ASC) AS EmpList

FROM EmpListbyCustID
GROUP BY CustomerID
ORDER BY EmpList;

