/*
------------------------------------------------------------------------------
과제 - CASE - Histogram 생성하기

내용:
 
1) 국가별 총 주문수량을 [39이하/40-79/80-119/120이상] 4개의 분포표로 출력

2) 분포별 개수를 숫자 대신 '■' 문자로 출력

------------------------------------------------------------------------------
*/
/*
준비
*/
USE Northwind;
GO

-- 국가별 총 주문수량
SELECT 
	ShipCountry
,	Orders = COUNT(*)
FROM dbo.Orders
GROUP BY ShipCountry;



/*
과제-1
*/
	SELECT 
		ShipCountry
	,	Orders = COUNT(*)
	FROM dbo.Orders
	GROUP BY ShipCountry


/*
해답-1
*/

SELECT
	Order40 = COUNT(CASE WHEN Orders < 40 THEN 1 END)
,	Order80 = COUNT(CASE WHEN Orders >= 40 AND Orders < 80 THEN 1 END)
,   Order120 = COUNT(CASE WHEN Orders >= 80 AND Orders < 120 THEN 1 END)
,   Order160 = COUNT(CASE WHEN Orders >= 120 THEN 1 END)

FROM(
	SELECT 
		ShipCountry
	,	Orders = COUNT(*)
	FROM dbo.Orders
	GROUP BY ShipCountry
) AS Orders

/*
해답-2
*/


SELECT
	Order40 = REPLICATE('■' ,COUNT(CASE WHEN Orders < 40 THEN 1 END))
,	Order80 = REPLICATE('■' ,COUNT(CASE WHEN Orders >= 40 AND Orders < 80 THEN 1 END))
,   Order120 = REPLICATE('■',COUNT(CASE WHEN Orders >= 80 AND Orders < 120 THEN 1 END))
,   Order160 = REPLICATE('■',COUNT(CASE WHEN Orders >= 120 THEN 1 END))

FROM(
	SELECT 
		ShipCountry
	,	Orders = COUNT(*)
	FROM dbo.Orders
	GROUP BY ShipCountry
) AS Orders
