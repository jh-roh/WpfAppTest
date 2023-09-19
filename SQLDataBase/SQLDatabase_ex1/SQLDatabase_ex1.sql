-- 데이터베이스 생성
CREATE DATABASE [88TESTDB];

-- 작업 DB 변경
USE [88TESTDB];
-- 기본 테이블 생성 스크립트

CREATE TABLE dbo.tbl_members
(
	m_id CHAR(16) PRIMARY KEY,
	m_name CHAR(50) NOT NULL,
	m_address CHAR(300) ,
	m_country CHAR(50) ,
	m_tel CHAR(50) ,
	m_email CHAR(300) NULL,
)


-- 데이터베이스 삭제
USE MASTER;
DROP DATABASE [88TESTDB];

--2개 컬럼을 기본키로 만든다?
--매출(주문) 테이블 생성 시 한명이 여러개의 물건을 구매하는 경우를 고려
--따라서, 매출 테이블 하나는 2개 컬럼을 하나의 기본키로 묶는다.

--주문내역서에 물품이 여러개 연결됨.
--    매출(주문)코드		매출상품번호		상품코드     복합키
--		   1001				1			  GT101       10011
--	       1001				2			  GT201       10012
--         1001				3			  GC301       10013
-- 실제 DB 구성시는 이와 같이 매출코드와 매출상품번호를 묶어서 하나의 기본키로 구성하면 좋음
-- '매출상품번호' 를 만들면 조회시에도 보다 용이하고 편리한 부분이 있다
-- 2개의 컬럼값이 하나의 값으로 묶여져서(또는 만들어져서) 기본키 역할을 하게 됨

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
-- DB 내 인덱스 확인
exec sp_helpIndex customers;
execute sp_helpIndex customers;

--NONCLUSTERED 인덱스
--테이블내 NONCLUSTERED 인덱스는 여러개를 생성할 수 있다.
--PRIMARY KEY로 지정된 열이 아닌 다른 열(컬럼) UNIQUE를 지정해주면 해당 열에 대해서는 NONCLUSTERED 인덱스가 적용



-- CLUSTERED INDEX VS NONCLUSTERED INDEX

CREATE TABLE dbo.tbl_customers
(
	cus_id	CHAR(10)	NOT	NULL	PRIMARY	KEY,
	cus_name	NVARCHAR(20)	NOT NULL	UNIQUE,
	cus_class	NVARCHAR(20)	NOT NULL	UNIQUE,
)


exec sp_helpIndex [dbo.tbl_customers];

--insert
insert into dbo.tbl_customers values('CCC', '홍길동', '씨앗1');
insert into dbo.tbl_customers values('BBB', '장길동', '씨앗2');
insert into dbo.tbl_customers values('AAA', '김길동', '씨앗3');

--select
select * from dbo.tbl_customers

--insert2
insert into dbo.tbl_customers values('YYY', '황길동', '씨앗4');
insert into dbo.tbl_customers values('DDD', '조길동', '씨앗5');
insert into dbo.tbl_customers values('에이에이', '노길동', '씨앗6');
insert into dbo.tbl_customers values('홍길동', '광길동', '씨앗7');
insert into dbo.tbl_customers values('광길동', '길동', '씨앗8');

--한글 영문 우선순위(1)
--기본적으로 한글이 우선순의를 갖는다(한글 완성형 기준)
--이유는 데이터베이스의 기본데이터 정렬 속성이 Korean_Wansung_CI_AS(한글 완성형 기준)로 되어 있기 때문
--그러나 우선순위를 변경하는 것도 가능하다.
--Collation 정보
-- => SQL Server의 기본 데이터 정렬을 설정하는 옵션
-- => DB 설치시에 셋팅

--MSSQL 을 설치할때 대부분 Korean_Wansung_CI_AS로 설정
-- => 한국어(한글) 완성형 문자라는 의미

--Collation 정보 보기
-- => GUI 모드에서 바로 확인 가능
SELECT * FROM ::fn_helpcollations() where name like 'latin%'

--Collation 정보 변경
-- => ALTER 명령어를 사용
-- => 기본키 제약조건 등으로 안될 수 있으며 초보자는 DB 백업을 권한다.

--ALTER DATABASE 디비명 COLLATE Korean_Wansung_CI_AI(Latin1_General_CI_AS)



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
INSERT into dbo.tbl_customers values('CCC','홍길동','씨앗1')
INSERT into dbo.tbl_customers values('BBB','김길동','씨앗2')
INSERT into dbo.tbl_customers values('AAA','장길동','씨앗3')
INSERT into dbo.tbl_customers values('홍길동','HONG','씨앗4') -- 한글입력
INSERT into dbo.tbl_customers values('박길동','PARK','씨앗5')
INSERT into dbo.tbl_customers values('강길동','KANG','씨앗6')


select * from dbo.tbl_customers order by cus_id 


--collations 정보보기
SELECT * FROM ::fn_helpcollations() where name like 'latin%'



-- 기본 데이터 정렬 속성값 변경(COLLATE 쿼리문 실습)
--[1]
-- DATABASE COLLATIOM 정보 보기
--DATABASE COLLATION 보기
SELECT DATABASEPROPERTYEX('9TO9', 'collation') AS COLLATION

--데이터베이스 기본 데이터 정렬 속성 변경
--테이블(tbl_customers) 내 컬럼 기본 데이터 정렬 속성 변경


--[2]
-- DATABASE > TABLE COLLATION 정보보기
exec sp_help tbl_customers
execute sp_help tbl_customers
sp_help tbl_customers
exec sp_help [dbo.tbl_customers]
exec sp_help 'dbo.tbl_customers'
exec sp_help "dbo.tbl_customers"

--[3]
--기본 데이터 정렬 속성 변경(DB)
--데이터베이스 COLLATION 정보를 변경한다는 것은 '기본 데이터 정렬' 속성을 변경한다는 뜻이다.
--그렇게 되면 변경 이후에 생성되는 신규 테이블들은 변경된 '기본 데이터 정렬' 속성을 따라서 생성되게 된다.
--물론 변경전에 생성된 테이블들의 속성은 변하지 않는다. 생성시의 속성 상태가 유지된다.
--ALTER

ALTER DATABASE [9TO9] COLLATE Korean_Wansung_CI_AS

SELECT DATABASEPROPERTYEX('9TO9', 'collation') as collation


--ALTER(TABLE > COLUMN)
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_id CHAR(10) COLLATE Korean_Wansung_CI_AS
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_class nvarCHAR(20) COLLATE Korean_Wansung_CI_AS
ALTER TABLE dbo.tbl_customers ALTER COLUMN cus_name nvarCHAR(20) NOT NULL


exec sp_helpindex [dbo.tbl_customers]



