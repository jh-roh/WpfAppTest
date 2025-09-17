/*
다중문 테이블 값 함수 테스트용
*/
CREATE OR ALTER FUNCTION GetOrdersList_M
(
	@p_OrderKey int
)
RETURNS 
@result TABLE 
(
	OrderKey int PRIMARY KEY
,	CustKey int
,	OrderStatus varchar(2)
,	TotalPrice decimal(15, 2)
,	OrderDate datetime
,	OrderPriority varchar(30)
,	Clerk varchar(30)
,	ShipPriority int
,	Comment varchar(200)
)
AS
BEGIN
	INSERT @result
    SELECT [OrderKey], [CustKey], [OrderStatus], [TotalPrice], [OrderDate], [OrderPriority], [Clerk], [ShipPriority], [Comment]
    FROM dbo.Orders AS o
    WHERE OrderKey < @p_OrderKey
	
	RETURN 
END
GO