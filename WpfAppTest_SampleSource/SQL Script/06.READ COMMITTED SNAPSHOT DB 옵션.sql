use [master];
GO

/*
DB�� ����� ������ ����� ����
*/
/*
ALTER DATABASE [SalesSimple] 
	SET READ_COMMITTED_SNAPSHOT ON	-- ON, OFF
*/

-- ���� Ȯ��
SELECT db.is_read_committed_snapshot_on FROM sys.databases AS db
WHERE db.database_id = DB_ID('SalesSimple');



