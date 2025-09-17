/*
-------------------------------------------------------------------------------
���� - �شܰ� ���ϱ�(���� �� ��Ͽ��� ���� �۾�)

����: 	�ֹ��� MAX(OrderDate, RequiredDate, ShippedDate)�� ��µǵ��� 
		�ڵ带 �ϼ��Ͻÿ� (���÷� 5�Ǹ�).
*/
USE Northwind;
GO


/*
����
*/
SELECT TOP(5)
	OrderID
,	OrderDate, RequiredDate, ShippedDate
,	Greatest_LastDate = /* complete code here for MAX(OrderDate, RequiredDate, ShippedDate) */

FROM dbo.Orders
;

/*
Ǯ��
*/
SELECT TOP(5)
	OrderID
,	OrderDate
,   RequiredDate
,   ShippedDate
,	Greatest_LastDate = (
							SELECT MAX(whichdate)
							FROM (VALUES (OrderDate),(RequiredDate), (ShippedDate)) as dv(whichdate)
						)

FROM dbo.Orders
;