/*
-------------------------------------------------------------------------------
과제 - 매장별 판매 비중과 편차 구하기

내용: CTE에서 수행했던 과제 내용을 Window Function으로 재 작성
		(단, stor_sum/avg, sales_sum/avg 열은 생략)
*/

/*
준비
*/
USE pubs;
GO


/*
-------------------------------------------------------------------------------
해답.
*/


SELECT 
	stor_id, s.title_id, s.qty
FROM dbo.sales AS s
ORDER BY s.stor_id ASC, s.title_id ASC

SELECT 
	stor_id, s.title_id, s.qty

,	[판매비중/매장]	= CAST(100.0 * s.qty / SUM(qty) OVER(PARTITION BY stor_id) AS DECIMAL(18,2))
, 	[판매편차/매장]	= s.qty - AVG(QTY) OVER(PARTITION BY stor_id)
,  [전체비중]		= CAST(100.0 * s.qty / SUM(qty) OVER() AS DECIMAL(18,2))
,  [전체편차]		= s.qty - AVG(qty) OVER()

FROM dbo.sales AS s
ORDER BY s.stor_id ASC, s.title_id ASC
;

