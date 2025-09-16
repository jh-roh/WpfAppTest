/*
잠금 상태 모니터링
*/
SELECT *
FROM sys.dm_tran_locks AS lk
WHERE lk.resource_database_id = DB_ID('SalesSimple')
	AND lk.resource_type NOT IN ('DATABASE')