/*
------------------------------------------------------------------------------
���� - ROW-TO-COLUMN

����: 	��ü ���� ���� �ŷ� ��� ������� ��ü �ڵ带 ','�� �����ؼ� �ϳ��� ��(column)�� ���
		��, �ߺ� EmployeeID�� �ϳ��� ���, �ֹ� ������ NULL�� ���
------------------------------------------------------------------------------
*/
USE Northwind;
GO


SELECT CustomerID, EmployeeID
FROM dbo.Orders
WHERE CustomerID = 'FRANS';



/*
����-1. 
*/
SELECT 
	CustomerID
,	EmpList = 
FROM dbo.Customers AS c
ORDER BY EmpList;


/*
����-2.
*/

/*
�ش�-1
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
�ش�-2
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

