/*
-------------------------------------------------------------------------------
���� - �̵� ��� ���ϱ�

����: ������������ ������ ������ Window Function�� �̿��� �ڵ�� ���ۼ�	
*/

/*
�غ�
*/
USE Northwind;
GO

SELECT 
	d1.OrderID,	d1.Quantity, d1.UnitPrice
FROM dbo.[Order Details] AS d1
WHERE d1.OrderID <= 10260;



/*
-------------------------------------------------------------------------------
���� - �̵� ��� ���ϱ�

����: ������������ ������ ������ Window Function�� �̿��� �ڵ�� ���ۼ�	
*/
SELECT
	d1.OrderID
,	Amount
,	AmountSlide =

FROM 

;

--�ش�

	SELECT
	 d1.OrderID
	,Amount = SUM(d1.Quantity * d1.UnitPrice)
	FROM dbo.[Order Details] AS d1
	WHERE d1.OrderID <= 10260
	GROUP BY d1.OrderID

SELECT
	d1.OrderID
,	Amount
,	AmountSlide = AVG(Amount) 
				  OVER(ORDER BY OrderID
				       ROWS BETWEEN 2 PRECEDING AND CURRENT ROW)

FROM 
(
	SELECT
	 d1.OrderID
	,Amount = SUM(d1.Quantity * d1.UnitPrice)
	FROM dbo.[Order Details] AS d1
	WHERE d1.OrderID <= 10260
	GROUP BY d1.OrderID

) AS d1
ORDER BY d1.OrderID


;

