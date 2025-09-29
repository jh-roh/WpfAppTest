/*
-------------------------------------------------------------------------------
���� - ��� CTE - �޴�/���� ��� �˻�

����: Menu ���̺��� MenuID = 6�� �������� �ֻ��� �޴����� �̵��ϸ鼭,
	�� �޴��� �̸��� '/' ���ڷ� ������ ����Ѵ�. �� �޴� �̸��� ��¼�����
	�����޴� ������ �ϸ� ����� ������ ����.

*/
/*
�غ�
*/
USE tempdb;
GO

IF OBJECT_ID('dbo.Menus', 'U') IS NOT NULL
	DROP TABLE dbo.Menus
GO

SELECT *
INTO dbo.Menus
FROM (VALUES
	('A', 1, NULL) 
,	('B', 2, 1)
,	('C', 3, 2)
,	('D', 4, 3)
,	('E', 5, 2) 
,	('F', 6, 2) 
) AS Menus(Name, MenuID, ParentMenuID);

-- ��ü ������
SELECT * FROM dbo.Menus;


/*
-------------------------------------------------------------------------------
����

	MenuID      ParentMenuID lvl         MenuPath
	----------- ------------ ----------- ---------------------------------------
	6           2            0           F
	2           1            1           B/F
	1           NULL         2           A/B/F

*/
WITH MenuPath (MenuID, ParentMenuID, lvl, MenuPath)
AS
(
	SELECT 
	FROM dbo.Menus AS m
	WHERE MenuID = 6

	UNION ALL

	SELECT 
	FROM dbo.Menus AS m

)
SELECT *
FROM MenuPath AS p
;


--�ش�
WITH MenuPath (MenuID, ParentMenuID, lvl, MenuPath)
AS
(
	SELECT m.MenuID, m.ParentMenuID, lvl = 0, CAST(m.Name AS varchar(1000))
	FROM dbo.Menus AS m
	WHERE MenuID = 6

	UNION ALL

	SELECT m.MenuID, m.ParentMenuID, lvl + 1, CAST(m.Name + '/' + MenuPath AS varchar(1000))
	FROM dbo.Menus AS m
	INNER JOIN MenuPath AS p
		ON m.MenuID = p.ParentMenuID

)
SELECT *
FROM MenuPath AS p
;
