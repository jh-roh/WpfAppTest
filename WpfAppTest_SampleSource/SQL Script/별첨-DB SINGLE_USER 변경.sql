/*
DB Single_User ����

����. �׽�Ʈ�����θ� ���
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
