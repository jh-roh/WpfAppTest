/*
"휴면 트랜잭션"

User Session별 "휴면 세션" 상태와 "트랜잭션 수" 확인
	- Profiler에서 트랜잭션 Begin/Commit/Rollback 추적

*/
SELECT se.session_id, se.status, se.open_transaction_count
	, se.transaction_isolation_level
	, DB_NAME(se.database_id) AS DB, se.program_name, se.client_interface_name
	, se.login_time, se.last_request_start_time, se.last_request_end_time
FROM sys.dm_exec_sessions AS se
WHERE se.is_user_process = 1
