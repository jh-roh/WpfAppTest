/*
부록. CommandTimeout 테스트

	- CancelAsync() 예제랑 함께 비교
*/
begin tran; select @@trancount;

	update lineitems
	set [ExtendedPrice] = [ExtendedPrice] + 1
	where orderkey = 2

rollback; select @@trancount;



/*
150만건 기준
	- 결과 출력 없는 경우 약 2초
	- 결과 출력 포함 약 약 11초
*/
/*
exec sp_executesql N'SELECT *
FROM [LineItems] AS [l]
WHERE [l].[OrderKey] <= @__p_orderkey_0',N'@__p_orderkey_0 int',@__p_orderkey_0=1500000
*/