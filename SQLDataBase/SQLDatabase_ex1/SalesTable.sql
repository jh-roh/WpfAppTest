--------------------최종 실습------------------
--dbo.tbl_members : 회원 테이블
--dbo.tbl_sales1  : 매출(주문) 테이블1
--dbo.tbl_sales2  : 매출(주문) 테이블2
--dbo.tbl_products : 상품 테이블
--dbo.tbl_vendors  : 벤더사

-------------------------------------------------------
--[1]dbo.tbl_members : 회원 테이블
-------------------------------------------------------

CREATE TABLE dbo.tbl_members
(
	m_id		char(10)		NOT NULL	PRIMARY Key,
	m_name		NVARCHAR(100)   NOT NULL,
	m_address	NVARCHAR(100)	NULL,
	m_country	CHAR(50)		NULL,
	m_tel		CHAR(50)		NULL,
	m_email		CHAR(300)		NULL,
);


-------------------------------------------------------
--[2]dbo.tbl_sales1 : 매출(주문) 테이블1
-------------------------------------------------------

create table dbo.tbl_sales1
(
	s1_num		INT			NOT	NULL	PRIMARY KEY,
	s1_date		DATETIME	NOT	NULL,
	m_id		CHAR(16)	NOT	NULL,
)


-------------------------------------------------------
--[3]dbo.tbl_sales2 : 매출(주문) 테이블2
-------------------------------------------------------
create table dbo.tbl_sales2
(
	s2_num			INT				NOT	NULL,
	s2_ordertem	INT				NOT	NULL,
	p_id			NVARCHAR(50)	NOT	NULL,
	qty				INT				NOT	NULL,
	otem_price		MONEY			NOT	NULL,
)

create table dbo.tbl_products
(
	p_id		NVARCHAR(50)	NOT	NULL	PRIMARY KEY,
	p_name		NVARCHAR(300)	NOT	NULL,
	p_price		MONEY			NOT	NULL,
	p_detail	NVARCHAR(1000)	NULL,
	v_id		NVARCHAR(25)	NOT	NULL,
)


create table dbo.tbl_vendors
(
	v_id	NVARCHAR(25)	NOT	NULL	PRIMARY KEY,
	v_name	NVARCHAR(300)	NOT	NULL,
)