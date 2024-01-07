
--CREATE DATABASE
CREATE DATABASE [7TO7];


--CHANGE DATABASE
USE [7TO7];


-----------------------------
--[1]dbo.tbl_members: 회원 테이블
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
--[2] dbo.tbl_sales1 : 매출(주문) 테이블1
---------------------------------------------
CREATE TABLE dbo.tbl_sales1
(
	s1_num		INT			NOT NULL	PRIMARY KEY,
	s1_date		DATETIME	NOT	null	,
	m_id		char(16)	not null	,
);

------------------------------------------
--외래키(foreign key) 정의
------------------------------------------
--alter table dbo.tbl_sales1
--	add	
--		constraint	FK_지정테이블명_참조테이블명

alter table dbo.tbl_sales1 add constraint FK_tbl_sales1_members
foreign key(m_id) references tbl_members(m_id);

----------------------------------------
--[3] dbo.tbl_sales2 : 매출(주문) 테이블2
----------------------------------------
create table dbo.tbl_sales2
(
	s2_num			INT				not	null,	-- 2개 컬럼을 기본키로 잡을 거기 떄문에 여기서는 기본키 지정 안함
	s2_orderitem	INT				not null,	--상품 정렬 넘버
	p_id			nvarchar(50)	not null,	--PRODUCT ID (상품테이블)
	qty				INT				not null,	--수량
	otem_price		money			not null,	--decimal(8,2)
	PRIMARY KEY CLUSTERED
	(
		s2_num,
		s2_orderitem
	)
)

--------------------------------------
--[4] dbo.tbl_products : 상품 테이블2
--------------------------------------
CREATE TABLE dbo.tbl_products
(
	p_id		NVARCHAR(50)	not null	primary key,
	p_name		nvarchar(300)	not null	,
	p_price		money			not	null	,
	p_detail	nvarchar(1000)	null		,--상품상세설명
	v_id		nvarchar(25)	not null	,--벤더사

)

------------------------------------------
--외래키(foreign key) 정의
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
--[5] dbo.tbl_vendors : 벤더사
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


--기본입력
--회원테이블에 입력
insert into dbo.tbl_members values('antman', N'앤트맨', N'서울', 'America', '010-1111-2222', 'antman@antman.com');
insert into dbo.tbl_members values('batman', N'배트맨', N'부산', 'America', '010-1111-2222', 'batman@batman.com');


insert into dbo.tbl_sales1 values(1001,'2000-12-25 15:22:10', 'batman');
insert into dbo.tbl_sales1 values(1002,'2000-12-25 15:22:10', 'antman');
insert into dbo.tbl_sales1 values(1003,'2000-12-25 15:22:10', 'batman');
