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
