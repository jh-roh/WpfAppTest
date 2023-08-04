-- �����ͺ��̽� ����
CREATE DATABASE [88TESTDB];

-- �۾� DB ����
USE [88TESTDB];
-- �⺻ ���̺� ���� ��ũ��Ʈ

CREATE TABLE dbo.tbl_members
(
	m_id CHAR(16) PRIMARY KEY,
	m_name CHAR(50) NOT NULL,
	m_address CHAR(300) ,
	m_country CHAR(50) ,
	m_tel CHAR(50) ,
	m_email CHAR(300) NULL,
)


-- �����ͺ��̽� ����
USE MASTER;
DROP DATABASE [88TESTDB];

--2�� �÷��� �⺻Ű�� �����?
--����(�ֹ�) ���̺� ���� �� �Ѹ��� �������� ������ �����ϴ� ��츦 ���
--����, ���� ���̺� �ϳ��� 2�� �÷��� �ϳ��� �⺻Ű�� ���´�.

--�ֹ��������� ��ǰ�� ������ �����.
--    ����(�ֹ�)�ڵ�		�����ǰ��ȣ		��ǰ�ڵ�     ����Ű
--		   1001				1			  GT101       10011
--	       1001				2			  GT201       10012
--         1001				3			  GC301       10013
-- ���� DB �����ô� �̿� ���� �����ڵ�� �����ǰ��ȣ�� ��� �ϳ��� �⺻Ű�� �����ϸ� ����
-- '�����ǰ��ȣ' �� ����� ��ȸ�ÿ��� ���� �����ϰ� ���� �κ��� �ִ�
-- 2���� �÷����� �ϳ��� ������ ��������(�Ǵ� ���������) �⺻Ű ������ �ϰ� ��

use [88TESTDB]

CREATE TABLE dbo.tbl_primarykey
(
	A	INT	NOT	NULL,
	B	INT	NOT	NULL,
	C	INT	NOT	NULL,
	PRIMARY KEY CLUSTERED (
		A, B
	)
)

INSERT INTO dbo.tbl_primarykey values (1001, 1, 101)
INSERT INTO dbo.tbl_primarykey values (1002, 1, 201)
INSERT INTO dbo.tbl_primarykey values (1002, 2, 301)


select * from tbl_primarykey
