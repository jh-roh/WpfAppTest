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


use [SalesSimple]
-- DB �� �ε��� Ȯ��
exec sp_helpIndex customers;
execute sp_helpIndex customers;

--NONCLUSTERED �ε���
--���̺� NONCLUSTERED �ε����� �������� ������ �� �ִ�.
--PRIMARY KEY�� ������ ���� �ƴ� �ٸ� ��(�÷�) UNIQUE�� �������ָ� �ش� ���� ���ؼ��� NONCLUSTERED �ε����� ����



-- CLUSTERED INDEX VS NONCLUSTERED INDEX

CREATE TABLE dbo.tbl_customers
(
	cus_id	CHAR(10)	NOT	NULL	PRIMARY	KEY,
	cus_name	NVARCHAR(20)	NOT NULL	UNIQUE,
	cus_class	NVARCHAR(20)	NOT NULL	UNIQUE,
)


exec sp_helpIndex [dbo.tbl_customers];

--insert
insert into dbo.tbl_customers values('CCC', 'ȫ�浿', '����1');
insert into dbo.tbl_customers values('BBB', '��浿', '����2');
insert into dbo.tbl_customers values('AAA', '��浿', '����3');

--select
select * from dbo.tbl_customers

--insert2
insert into dbo.tbl_customers values('YYY', 'Ȳ�浿', '����4');
insert into dbo.tbl_customers values('DDD', '���浿', '����5');
insert into dbo.tbl_customers values('���̿���', '��浿', '����6');
insert into dbo.tbl_customers values('ȫ�浿', '���浿', '����7');
insert into dbo.tbl_customers values('���浿', '�浿', '����8');

--�ѱ� ���� �켱����(1)
--�⺻������ �ѱ��� �켱���Ǹ� ���´�(�ѱ� �ϼ��� ����)
--������ �����ͺ��̽��� �⺻������ ���� �Ӽ��� Korean_Wansung_CI_AS(�ѱ� �ϼ��� ����)�� �Ǿ� �ֱ� ����
--�׷��� �켱������ �����ϴ� �͵� �����ϴ�.
--Collation ����
-- => SQL Server�� �⺻ ������ ������ �����ϴ� �ɼ�
-- => DB ��ġ�ÿ� ����

--MSSQL �� ��ġ�Ҷ� ��κ� Korean_Wansung_CI_AS�� ����
-- => �ѱ���(�ѱ�) �ϼ��� ���ڶ�� �ǹ�

--Collation ���� ����
-- => GUI ��忡�� �ٷ� Ȯ�� ����
SELECT * FROM ::fn_helpcollations() where name like 'latin%'

--Collation ���� ����
-- => ALTER ��ɾ ���
-- => �⺻Ű �������� ������ �ȵ� �� ������ �ʺ��ڴ� DB ����� ���Ѵ�.

--ALTER DATABASE ���� COLLATE Korean_Wansung_CI_AI(Latin1_General_CI_AS)



-- CREATE DATABASE
CREATE DATABASE [9TO9];

-- CHANGE DB
USE [9TO9];


-- CREATE TABLE
CREATE TABLE dbo.tbl_customers
(
	cus_id	CHAR(10)	NOT	NULL	PRIMARY	KEY,
	cus_name	nvarchar(20)	NOT NULL UNIQUE,
	cus_class	nvarchar(20)	NOT NULL UNIQUE
)


--insert
INSERT into dbo.tbl_customers values('CCC','ȫ�浿','����1')
INSERT into dbo.tbl_customers values('BBB','��浿','����2')
INSERT into dbo.tbl_customers values('AAA','��浿','����3')
INSERT into dbo.tbl_customers values('ȫ�浿','HONG','����4') -- �ѱ��Է�
INSERT into dbo.tbl_customers values('�ڱ浿','PARK','����5')
INSERT into dbo.tbl_customers values('���浿','KANG','����6')


select * from dbo.tbl_customers order by cus_id 


--collations ��������
SELECT * FROM ::fn_helpcollations() where name like 'latin%'



-- �⺻ ������ ���� �Ӽ��� ����(COLLATE ������ �ǽ�)
--[1]
-- DATABASE COLLATIOM ���� ����
--DATABASE COLLATION ����
SELECT DATABASEPROPERTYEX('9TO9', 'collation') AS COLLATION

--�����ͺ��̽� �⺻ ������ ���� �Ӽ� ����
--���̺�(tbl_customers) �� �÷� �⺻ ������ ���� �Ӽ� ����


--[2]
-- DATABASE > TABLE COLLATION ��������
exec sp_help tbl_customers
execute sp_help tbl_customers
sp_help tbl_customers
exec sp_help [dbo.tbl_customers]
exec sp_help 'dbo.tbl_customers'
exec sp_help "dbo.tbl_customers"

--[3]
--�⺻ ������ ���� �Ӽ� ����(DB)
--�����ͺ��̽� COLLATION ������ �����Ѵٴ� ���� '�⺻ ������ ����' �Ӽ��� �����Ѵٴ� ���̴�.
--�׷��� �Ǹ� ���� ���Ŀ� �����Ǵ� �ű� ���̺���� ����� '�⺻ ������ ����' �Ӽ��� ���� �����ǰ� �ȴ�.
--���� �������� ������ ���̺���� �Ӽ��� ������ �ʴ´�. �������� �Ӽ� ���°� �����ȴ�.
--ALTER

ALTER DATABASE [9TO9] COLLATE Korean_Wansung_CI_AS

SELECT DATABASEPROPERTYEX('9TO9', 'collation') as collation


--ALTER(TABLE > COLUMN)
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_id CHAR(10) COLLATE Korean_Wansung_CI_AS
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_class nvarCHAR(20) COLLATE Korean_Wansung_CI_AS
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_name nvarCHAR(20) NOT NULL


exec sp_helpindex [dbo.tbl_customers]



