/*
�η�. CommandTimeout �׽�Ʈ

	- CancelAsync() ������ �Բ� ��
*/
begin tran; select @@trancount;

	update lineitems
	set [ExtendedPrice] = [ExtendedPrice] + 1
	where orderkey = 2

rollback; select @@trancount;



/*
150���� ����
	- ��� ��� ���� ��� �� 2��
	- ��� ��� ���� �� �� 11��
*/
/*
exec sp_executesql N'SELECT *
FROM [LineItems] AS [l]
WHERE [l].[OrderKey] <= @__p_orderkey_0',N'@__p_orderkey_0 int',@__p_orderkey_0=1500000
*/