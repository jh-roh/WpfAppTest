--날짜
--Q1. 회원 아이디가 mmm에 대해서 날짜를 5일 더 연장해주고 싶다면
--Q2. 회원 이름이 강길동에 대해서 날짜를 6개월 추가시키고 싶다면
--Q3. 강길동에게 다시 6개월을 연장시키면 년도가 바뀌는지 확인하고 싶다면?
--Q4. 회원 아이디가 eee에 대해서 기간을 5일 빼고 싶다면?
--Q5. 데이터를 수정이나 변경하지 않고 5일 추가된 결과만 보고 싶다면
--Q6. 실제 마감일(2002-08-25) 보다 5일 연장된 날짜 마감을 넘기 회원은 몇명인지 알고 싶다면?


--테이블 생성
CREATE TABLE dbo.tbl_date77
(
	cus_id	CHAR(10)	NOT	NULL	CONSTRAINT	PK_CUSID77	PRIMARY KEY,
	cus_name	NVARCHAR(20)	NOT	NULL,
	cus_date	DATE			NOT NULL
)

INSERT INTO dbo.tbl_date77 VALUES ('aaa', N'강길동', '2000-01-01');
INSERT INTO dbo.tbl_date77 VALUES ('bbb', N'박길동', '2000-10-01');
INSERT INTO dbo.tbl_date77 VALUES ('ccc', N'장호랑', '2001-04-01');
INSERT INTO dbo.tbl_date77 VALUES ('ddd', N'성진길', '2001-12-01');
INSERT INTO dbo.tbl_date77 VALUES ('eee', N'유정걸', '2002-03-01');
INSERT INTO dbo.tbl_date77 VALUES ('fff', N'왕용인', '2002-11-01');
INSERT INTO dbo.tbl_date77 VALUES ('ggg', N'김호수', '2003-07-01');
INSERT INTO dbo.tbl_date77 VALUES ('hhh', N'박길산', '2003-12-01');
INSERT INTO dbo.tbl_date77 VALUES ('iii', N'이공항', '2004-09-01');
INSERT INTO dbo.tbl_date77 VALUES ('jjj', N'오대산', '2004-02-01');


select * from dbo.tbl_date77;
-----------------------------------------------------------------
--[!]DATEADD 함수
-----------------------------------------------------------------
--어떤 날짜에 기간을 더하는 함수이다.
--형식 : DATEADD (datepart, number, date)
--형식 : DATEADD (더할날짜단위, 간격(더할숫자), 날짜(컬럼명))

--일(DAY) 단위
select dateadd(day, 5, '2004-02-01');

--월(MONTH) 단위
select DATEADD(MONTH, 6, '2004-02-01');

--년(YEAR) 단위
select dateadd(year, 1, '2004-02-01');

--Q1. 회원 아이디가 jjj에 대해서 날짜를 5일 더 연장해주고 싶다면

update tbl_date77
set cus_date = DATEADD(day, 5, cus_date) 
where cus_id = 'jjj'

--Q2. 회원 이름이 강길동에 대해서 날짜를 6개월 추가시키고 싶다면

update tbl_date77
set cus_date = DATEADD(MONTH, 6, cus_date) 
where cus_name = N'강길동'

--Q3. 강길동에게 다시 6개월을 연장시키면 년도가 바뀌는지 확인하고 싶다면?

select DateADD(MONTH, 6, cus_date) from tbl_date77 where cus_name = '강길동'

--Q4. 회원 아이디가 eee에 대해서 기간을 5일 빼고 싶다면?
update tbl_date77
set cus_date = DATEADD(day, -5, cus_date)
where cus_id = 'eee'

--Q5. 데이터를 수정이나 변경하지 않고 5일 추가된 결과만 보고 싶다면
select * from dbo.tbl_date77;
select cus_id, cus_name,DATEADD(day, 5, cus_date) as cus_date from tbl_date77


--Q6. 실제 마감일(2002-08-25) 보다 5일 연장된 날짜 마감을 넘기 회원은 몇명인지 알고 싶다면?


select count(*) from tbl_date77
where cus_date > DATEADD(day, 5, '2002-08-25')

-- B(변수 사용)
DECLARE @aaa INT; -- 변수선언 @변수명 데이터 타입
SET @aaa =10;
select @aaa;

DECLARE @bbb DATE;
SET @bbb = '2002-11-15'
select @bbb;

--연장된 마감일
DECLARE @duedate DATE = DATEADD(DAY, @aaa, @bbb);
select @duedate;

select * from dbo.tbl_date77 where cus_date > @duedate


