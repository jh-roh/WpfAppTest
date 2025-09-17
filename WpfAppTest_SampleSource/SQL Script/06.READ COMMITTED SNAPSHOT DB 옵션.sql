use [master];
GO

/*
DB에 연결된 세션이 없어야 실행
*/
/*
ALTER DATABASE [SalesSimple] 
	SET READ_COMMITTED_SNAPSHOT ON	-- ON, OFF
*/

-- 변경 확인
SELECT db.is_read_committed_snapshot_on FROM sys.databases AS db
WHERE db.database_id = DB_ID('SalesSimple');



