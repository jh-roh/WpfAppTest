
--CREATE DATABASE
CREATE DATABASE [7TO7];


--CHANGE DATABASE
USE [7TO7];


-----------------------------
--[1]dbo.tbl_members: ȸ�� ���̺�
------------------------------
create table dbo.tbl_members
(
	m_id		CHAR(16)		NOT NULL	PRIMARY KEY,
	m_name		nvarchar(100)	NOT NULL	,
	m_address	nvarchar(100)	NULL		,
	m_country	char(50)		NULL		,
	m_tel		char(50)		null		,
	m_email		char(300)		null		,
);

---------------------------------------------
--[2] dbo.tbl_sales1 : ����(�ֹ�) ���̺�1
---------------------------------------------
CREATE TABLE dbo.tbl_sales1
(
	s1_num		INT			NOT NULL	PRIMARY KEY,
	s1_date		DATETIME	NOT	null	,
	m_id		char(16)	not null	,
);

------------------------------------------
--�ܷ�Ű(foreign key) ����
------------------------------------------
--alter table dbo.tbl_sales1
--	add	
--		constraint	FK_�������̺��_�������̺��

alter table dbo.tbl_sales1 add constraint FK_tbl_sales1_members
foreign key(m_id) references tbl_members(m_id);

----------------------------------------
--[3] dbo.tbl_sales2 : ����(�ֹ�) ���̺�2
----------------------------------------
create table dbo.tbl_sales2
(
	s2_num			INT				not	null,	-- 2�� �÷��� �⺻Ű�� ���� �ű� ������ ���⼭�� �⺻Ű ���� ����
	s2_orderitem	INT				not null,	--��ǰ ���� �ѹ�
	p_id			nvarchar(50)	not null,	--PRODUCT ID (��ǰ���̺�)
	qty				INT				not null,	--����
	otem_price		money			not null,	--decimal(8,2)
	PRIMARY KEY CLUSTERED
	(
		s2_num,
		s2_orderitem
	)
)

--------------------------------------
--[4] dbo.tbl_products : ��ǰ ���̺�2
--------------------------------------
CREATE TABLE dbo.tbl_products
(
	p_id		NVARCHAR(50)	not null	primary key,
	p_name		nvarchar(300)	not null	,
	p_price		money			not	null	,
	p_detail	nvarchar(1000)	null		,--��ǰ�󼼼���
	v_id		nvarchar(25)	not null	,--������

)

------------------------------------------
--�ܷ�Ű(foreign key) ����
------------------------------------------

alter table dbo.tbl_sales2 
	add
		constraint FK_tbl_sales2_sales1
			foreign key(s2_num)
				references tbl_sales1(s1_num),
		constraint FK_tbl_sales2_products
				foreign key(p_id) 
					references tbl_products(p_id);






------------------------------
--[5] dbo.tbl_vendors : ������
------------------------------

--create table dbo.tbl_vendors
--(
--)

--alter table tbo.tbl_products 
--	add constraint	FK_tbl_products_vendors
--		foreign key(v_id) 
--			references tbl_vendors(v_id)

exec sp_helpconstraint tbl_sales1
exec sp_helpconstraint tbl_sales2


select * from dbo.tbl_members;

select * from tbl_sales1;
select * from tbl_sales2;


--�⺻�Է�
--ȸ�����̺� �Է�
insert into dbo.tbl_members values('antman', N'��Ʈ��', N'����', 'America', '010-1111-2222', 'antman@antman.com');
insert into dbo.tbl_members values('batman', N'��Ʈ��', N'�λ�', 'America', '010-1111-2222', 'batman@batman.com');


insert into dbo.tbl_sales1 values(1001,'2000-12-25 15:22:10', 'batman');
insert into dbo.tbl_sales1 values(1002,'2000-12-25 15:22:10', 'antman');
insert into dbo.tbl_sales1 values(1003,'2000-12-25 15:22:10', 'batman');
