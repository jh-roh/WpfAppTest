/*
------------------------------------------------------------------------------
���� - CASE - Ư�� �� ���� ����

����: @pid���� ������ ProductID���� ������������ ����/����ϵ�,
		@pid���� ���� ���� ������ ProductID ���Ŀ� ����Ѵ�.

------------------------------------------------------------------------------
*/
USE Northwind
GO


/*
����
*/
DECLARE @pid int = 65;

SELECT ProductID
FROM dbo.Products
ORDER BY 

;


/*
�ش�-1
*/
DECLARE @pid int = 65;

SELECT ProductID
FROM dbo.Products
ORDER BY 
(
CASE 
WHEN ProductID < @pid THEN ProductID + 1048576
ELSE ProductID
END ) ASC 
;




