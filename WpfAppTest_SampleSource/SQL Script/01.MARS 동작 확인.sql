/*
MARS 동작 확인

	1) session 1개 당 connection N개
	2) Profiler에서 RequestID가 서로 다르게 출려됨
*/

/*
세션 확인
*/
SELECT * FROM sys.dm_exec_sessions AS s
WHERE s.is_user_process = 1

/*
세션별 Connection 확인(MARS의 경우 다중 Connection)
*/
SELECT * FROM sys.dm_exec_connections AS c
WHERE c.session_id in (SELECT s.session_id FROM sys.dm_exec_sessions AS s
						WHERE s.is_user_process = 1)
ORDER BY c.session_id DESC

