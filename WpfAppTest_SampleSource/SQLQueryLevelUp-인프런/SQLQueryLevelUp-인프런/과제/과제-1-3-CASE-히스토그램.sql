/*
------------------------------------------------------------------------------
���� - CASE - Histogram �����ϱ�

����:
 
1) ������ �� �ֹ������� [39����/40-79/80-119/120�̻�] 4���� ����ǥ�� ���

2) ������ ������ ���� ��� '��' ���ڷ� ���

------------------------------------------------------------------------------
*/
/*
�غ�
*/
USE Northwind;
GO

-- ������ �� �ֹ�����
SELECT 
	ShipCountry
,	Orders = COUNT(*)
FROM dbo.Orders
GROUP BY ShipCountry;



/*
����-1
*/
	SELECT 
		ShipCountry
	,	Orders = COUNT(*)
	FROM dbo.Orders
	GROUP BY ShipCountry


/*
�ش�-1
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
�ش�-2
*/


SELECT
	Order40 = REPLICATE('��' ,COUNT(CASE WHEN Orders < 40 THEN 1 END))
,	Order80 = REPLICATE('��' ,COUNT(CASE WHEN Orders >= 40 AND Orders < 80 THEN 1 END))
,   Order120 = REPLICATE('��',COUNT(CASE WHEN Orders >= 80 AND Orders < 120 THEN 1 END))
,   Order160 = REPLICATE('��',COUNT(CASE WHEN Orders >= 120 THEN 1 END))

FROM(
	SELECT 
		ShipCountry
	,	Orders = COUNT(*)
	FROM dbo.Orders
	GROUP BY ShipCountry
) AS Orders
