
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



--���� ���Ἲ ����(����) �׽�Ʈ
--���̺�� ���̺��� ���踦 �ΰԵǸ�(�ܷ�Ű ����) �����ϴ� ���̺��� ���� �ƴ� �ٸ� ���� ���� �� ����.
--���� �Էµ� ���� �����ϴ� ���̺� ���� ���� �ƴ� �ٸ������� ����, ������ ���� ����.
--�����ͺ��̽� �ý����� �ϰ����ְ� ������ �� �ֵ��� �����Ѵ�.



--����/����
UPDATE dbo.tbl_sales1
SET m_id = 'superman'
where s1_num = '1001'


--------------------------------------------------------------------------
-- INSERT DATA
--------------------------------------------------------------------------
--[ 1 ] dbo.tbl_members
--���ǿ����� ��¥, ����Ʈ
select * from dbo.tbl_members;

--�⺻ �Է�
Insert into dbo.tbl_members values('antman', '��Ʈ��', '����', 'America', '010-1234-5678', 'antman@antman.com');

Insert into dbo.tbl_members(m_id, m_name, m_address, m_country, m_tel, m_email)
			values('batman','��Ʈ��', '��õ', 'Korea', '010-1234-5678', 'batman@batman.com')

--���� �Է�
insert into dbo.tbl_members(m_id, m_name, m_address, m_country, m_tel, m_email)
			 values
				('superman', '���۸�', '��õ', 'Canada', '010-1234-5678', 'superman@superman.com'),
				('skyman', '�ϴø�', '����', 'Russia', '010-1234-5678', 'skyman@skyman.com'),
				('brickman', '������', '�λ�', 'China', '010-1234-5678', 'brickman@brickman.com');


insert into dbo.tbl_members values('bookman', '������', '����', '�ѱ�', '010-1234-0000', 'bookman@bookman.com');
insert into dbo.tbl_members values('flowerman', 'ȭ�и�', 'û��', '�ѱ�', '010-1234-0000', 'flowerman@flowerman.com');


insert into dbo.tbl_members(m_id,m_name,m_tel,m_email)
			values ('abcman', 'ABC��', '010-1234-5678', 'abcman@abcman.com')

--[2] dbo.tbl_products
select * from dbo.tbl_products

insert into dbo.tbl_products (p_id, p_name, p_price, p_detail, v_id)
			values ('GS101', '�ڷ�����', 90000, 'Ŀ������� �̷������� �ֽ��� ��', '��Ű�ݼ�'),
				    ('GS102', '�����', 770000, '�������� �󸰵��� �� ������ �����', '����'),
					('GS103', '��ġ�����', 550000, '��ġ�� ���� ��ġ����� �ְ�', '����'),
					('GS104', '�����', 440000, '������� ������ ���� �ְ��� �����', '��Ƽ��'),
					('GS105', '��ǻ��', 2200000, '���迡�� �ְ�� ������ ��ǻ�� �׷�', '�׷�');
						

--[3] dbo.tbl_sales1

select * from dbo.tbl_sales1
select * from dbo.tbl_sales2


delete from dbo.tbl_sales1 where s1_num = '1006'
insert into dbo.tbl_sales1(s1_num, s1_date,m_id)
			values
					(1004,'2006-12-28 20:21:20', 'skyman'),
					(1005,'2006-12-29 22:11:10', 'brickman'),
					(1006,'2006-12-29', 'superman');

--�����Է�
insert into dbo.tbl_sales2(s2_num, s2_orderitem, p_id, qty, otem_price)
			values
					(1001,1, 'GS101', 1, 990000),
					(1001,2, 'GS102', 1, 770000),
					(1001,3, 'GS103', 1, 550000);
					

--�����Է�
insert into dbo.tbl_sales2 VALUES(1002,1,'GS101', 1, 990000);
insert into dbo.tbl_sales2 VALUES(1003,1,'GS103', 1, 550000);
insert into dbo.tbl_sales2 VALUES(1004,1,'GS102', 1, 770000);
insert into dbo.tbl_sales2 VALUES(1005,1,'GS104', 1, 440000);
insert into dbo.tbl_sales2 VALUES(1005,2,'GS105', 1, 2200000);


-- ������ ȸ���� ���� �̸�, �ּ�, �̸����� ������ �Ѵٸ�?

select * from dbo.tbl_members;
select * from dbo.tbl_sales1;


select m_name, m_address, m_email 
from tbl_members, tbl_sales1
where tbl_members.m_id = tbl_sales1.m_id


--�� ������ ��Ī�� ����Ͽ� ����غ���.
select m.m_id,m_name, m_address, m_email
from tbl_members m, tbl_sales1 s
where m.m_id = s.m_id

--�� ������ ��Ī�� ����Ͽ� ����غ���.
select m.m_id as '���_ID',m_name, m_address, m_email
from tbl_members as m, tbl_sales1 as s1
where m.m_id = s1.m_id


----------------------------------------
-- JOIN (3�� �̻� ���̺� ���� �ǽ�)
----------------------------------------
--Q. ��ġ �����(GS103)�� ������ ���� ���̵�� �̸��� �˰� �ʹٸ�?


select * from dbo.tbl_sales1
select * from dbo.tbl_sales2

select tb_m.m_id, tb_m.m_name, tb_s2.p_id
from tbl_members as tb_m, tbl_sales1 as tb_s1, tbl_sales2 as tb_s2
where tb_m.m_id = tb_s1.m_id
   and tb_s1.s1_num = tb_s2.s2_num
   and p_id = 'GS103'


----------------------------------------------------------------------
-- View
----------------------------------------------------------------------
-- �ϳ� �̻��� ���̺� ��� �ִ� �����͸� ���� ���̺�� ���´�.
-- �������� ���̺��� ��� �並 ����� ��� '���պ�' ��� �θ���.

SELECT * from dbo.tbl_members;

-----------------------------------------------------------------
-- [1] VIEW ���� �� ���� �׸��� ��� : �⺻


CREATE VIEW MembersSelectAll
	AS
		SELECT * from dbo.tbl_members;

select * from MembersSelectAll

--------------------------------------------------------------------
-- [2] VIEW ���� �� ���� �׸��� ��� : ��Ȱ��
--------------------------------------------------------------------
--�� ����

DROP VIEW MembersSales1Sales2;
CREATE VIEW MembersSales1Sales2
	AS
		--��� ���� ������ �ۼ�
		--��� ������ �ۼ��ϴ� ���� ���̺�
		SELECT M.m_id, m_name, m_address, m_tel, m_email	
			FROM dbo.tbl_members M, dbo.tbl_sales1 S1, dbo.tbl_sales2 S2
			WHERE
				S1.m_id = M.m_id
			AND
				S1.s1_num = S2.s2_num
			AND
				S2.p_id = 'GS102'


select * from MembersSales1Sales2 where m_id = 'skyman'

select * from MembersSelectAll



