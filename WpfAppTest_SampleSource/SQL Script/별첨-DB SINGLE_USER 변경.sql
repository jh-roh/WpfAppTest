/*
DB Single_User 변경

주의. 테스트용으로만 사용
*/
USE [master]
GO
ALTER DATABASE [SalesSimple] 
	SET SINGLE_USER 
	WITH NO_WAIT
GO

ALTER DATABASE [SalesSimple] 
	SET MULTI_USER WITH 
	NO_WAIT
GO
