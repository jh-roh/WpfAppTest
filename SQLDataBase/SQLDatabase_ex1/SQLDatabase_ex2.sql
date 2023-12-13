--PK/UK 제약조건 추가/삭제
--PRIMARY KEY CONSTRAINT ADD/DROP
--UNIQUE CONSTRAINT ADD/DROP
--먼저 제약조건(PRIMARY KEY, UNIQUE) 없이 테이블 생성
--제약조건 추가 및 확인
--제약조건 삭제 및 확인
--테이블 삭제


CREATE DATABASE [9TO9]

--작업 DB 변경
USE [9TO9]



------------------------------------------------------------
-- [!] PRIMARY KEY CONSTRAINT ADD / DROP
------------------------------------------------------------
-- [1] 테이블 생성
-- 먼저 기본키 제약조건, 유니크 제약조건 없이 테이블을 생성해본다.

CREATE TABLE dbo.tbl_customers4
(
	cus_id CHAR(10) NOT NULL,
	cus_name NVARchar(20) NOT NULL,
	cus_class NVARchar(20) NOT NULL,
)

-- [2] 인덱스 확인
-- 기본키 지정이나 유니크 지정을 안했기 때문에 인덱스 확인을 해보면 없다라고 나온다.
sp_helpindex [dbo.tbl_customers4]
sp_helpconstraint tbl_customers4


-- [3] 기본키 추가(제약조건 추가)
exec sp_helpconstraint tbl_customers4

-- CLUSTERED는 안써줘도 기본으로 클러스터드 인덱스 생성해줌.
ALTER TABLE dbo.tbl_customers4 
	ADD CONSTRAINT PK_tbl_customers4 PRIMARY KEY CLUSTERED (cus_id);
GO

EXEC sp_helpindex tbl_customers4
GO

--[4] 기본키 제약조건 삭제
ALTER TABLE dbo.tbl_customers4 DROP CONSTRAINT PK_tbl_customers4
GO

EXEC sp_helpindex tbl_customers4
GO


--[5] 테이블 삭제
DROP TABLE dbo.tbl_customers4




------------------------------------------------------------
-- [!] UNIQUE CONSTRAINT ADD / DROP
------------------------------------------------------------

--작업 DB 변경
USE [9TO9]

--[1] 테이블 생성
CREATE TABLE dbo.tbl_customers4
(
	cus_id CHAR(10) NOT NULL,
	cus_name NVARchar(20) NOT NULL,
	cus_class NVARchar(20) NOT NULL,
)

--[2]인덱스 확인
exec sp_helpindex [dbo.tbl_customers4]

--[3]UNIQUE 추가(제약조건 추가)
ALTER TABLE dbo.tbl_customers4 
	ADD 
		CONSTRAINT UQ_tbl_customers4 UNIQUE NONCLUSTERED(cus_name)
GO

ALTER TABLE dbo.tbl_customers4
	DROP	
		CONSTRAINT UQ_tbl_customers4
GO

--[4]
DROP TABLE [tbl_customers4]

-----------------------------------------------------------------------------------
--기본키 삭제 및 확인
--기본키 삭제 및 확인하는 다른 방법들
-----------------------------------------------------------------------------------
--A
ALTER TABLE dbo.tbl_customers4 DROP CONSTRAINT PK_tbl_customers4
GO

--B
ALTER TABLE dbo.tbl_customers4
	DROP PK_tbl_customers4

exec sp_helpconstraint [dbo.tbl_customers4]

--기본키 확인(시스템 뷰 - 정보 스키마 뷰)
--데이터베이스 뷰쪽에 DB마다 생성이 되어 있다.
--테이블 각 컬럼에 대한 정보를 보거나 활용하고자 할 때 사용할 수 있다.
--조인 등을 통하여 여러 정보를 묶어서 활용할 수 도 있다.

select * from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where TABLE_NAME = 'tbl_customers4'

------------------------------------------------------------------------------------
--[!] 기본키(제약조건) 존재 시 삭제하기
------------------------------------------------------------------------------------
use [9TO9]

--[1] 테이블 생성
CREATE TABLE dbo.tbl_customers6
(
	cus_id	CHAR(10)	NOT NULL	PRIMARY	KEY,
	cus_name	NVARCHAR(20)	NOT	NULL	UNIQUE,
	cus_class	NVARCHAR(20)	NOT	NULL	UNIQUE,
)

exec sp_helpconstraint [dbo.tbl_customers6]

alter table dbo.tbl_customers6
	drop constraint
		PK__tbl_cust__E84D41E8B55E04F4

alter table dbo.tbl_customers6
	add constraint
		PK_tbl_customers6_cus_id primary key clustered(cus_id)




CREATE TABLE dbo.tbl_test
(
	cus_id	CHAR(10)	PRIMARY KEY,
	cus_name CHAR(10)   UNIQUE,  
)

--값 입력
INSERT INTO dbo.tbl_test VALUES('AAA', 'AAA'); --가능
INSERT INTO dbo.tbl_test VALUES('', ''); --가능(빈값도 값이니깐)
INSERT INTO dbo.tbl_test VALUES('NULL', 'BBB'); -- 가능
INSERT INTO dbo.tbl_test VALUES(NULL, 'CCC'); -- 불가능
INSERT INTO dbo.tbl_test VALUES('CCC', NULL); -- 가능
INSERT INTO dbo.tbl_test VALUES('DDD', NULL); -- 불가능(기존에 NULL이 들어가 있으면 불가능)



select * from dbo.tbl_test

--기본키명 직접 지정하기
--테이블을 만들때 직접 기본키명을 지정해서 만들 수 있다.
--cus_id	CHAR(10)	NOT NULL	제약 기본키명(CONSTRAINT PK_CUSID)		PRIMARY KEY
                                        
--테이블 생성1
CREATE TABLE dbo.tbl_pktest
(
	cus_id		char(10)	NOT	NULL	PRIMARY KEY,
	cus_name	nvarchar(20)	NOT	NULL	,
)

--테이블 생성2
CREATE TABLE dbo.tbl_pktest2
(
	cus_id		char(10)	NOT	NULL  CONSTRAINT  PK_ABC77  PRIMARY KEY,
	cus_name	nvarchar(20)	NOT	NULL	,
)



--인덱스 확인
EXEC sp_helpindex tbl_pktest2










