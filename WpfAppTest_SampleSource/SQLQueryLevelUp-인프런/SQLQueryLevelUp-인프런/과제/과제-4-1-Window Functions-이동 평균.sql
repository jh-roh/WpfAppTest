/*
-------------------------------------------------------------------------------
과제 - 이동 평균 구하기

내용: 서브쿼리에서 수행한 과제를 Window Function을 이용한 코드로 재작성	
*/

/*
준비
*/
USE Northwind;
GO

SELECT 
	d1.OrderID,	d1.Quantity, d1.UnitPrice
FROM dbo.[Order Details] AS d1
WHERE d1.OrderID <= 10260;



/*
-------------------------------------------------------------------------------
과제 - 이동 평균 구하기

내용: 서브쿼리에서 수행한 과제를 Window Function을 이용한 코드로 재작성	
*/
SELECT
	d1.OrderID
,	Amount
,	AmountSlide =

FROM 

;

--해답

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

