
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



--참조 무결성 수정(변경) 테스트
--테이블과 테이블간의 관계를 맺게되면(외래키 지정) 참조하는 테이블열의 값이 아닌 다른 값이 들어올 수 없다.
--역시 입력된 값도 참조하는 테이블 열의 값이 아닌 다른값으로 수정, 변경할 수도 없다.
--데이터베이스 시스템이 일관성있게 유지될 수 있도록 관리한다.



--수정/변경
UPDATE dbo.tbl_sales1
SET m_id = 'superman'
where s1_num = '1001'


--------------------------------------------------------------------------
-- INSERT DATA
--------------------------------------------------------------------------
--[ 1 ] dbo.tbl_members
--강의예제상 날짜, 포인트
select * from dbo.tbl_members;

--기본 입력
Insert into dbo.tbl_members values('antman', '앤트맨', '서울', 'America', '010-1234-5678', 'antman@antman.com');

Insert into dbo.tbl_members(m_id, m_name, m_address, m_country, m_tel, m_email)
			values('batman','배트맨', '춘천', 'Korea', '010-1234-5678', 'batman@batman.com')

--다중 입력
insert into dbo.tbl_members(m_id, m_name, m_address, m_country, m_tel, m_email)
			 values
				('superman', '슈퍼맨', '인천', 'Canada', '010-1234-5678', 'superman@superman.com'),
				('skyman', '하늘맨', '제주', 'Russia', '010-1234-5678', 'skyman@skyman.com'),
				('brickman', '벽돌맨', '부산', 'China', '010-1234-5678', 'brickman@brickman.com');


insert into dbo.tbl_members values('bookman', '도서맨', '전주', '한국', '010-1234-0000', 'bookman@bookman.com');
insert into dbo.tbl_members values('flowerman', '화분맨', '청주', '한국', '010-1234-0000', 'flowerman@flowerman.com');


insert into dbo.tbl_members(m_id,m_name,m_tel,m_email)
			values ('abcman', 'ABC맨', '010-1234-5678', 'abcman@abcman.com')

--[2] dbo.tbl_products
select * from dbo.tbl_products

insert into dbo.tbl_products (p_id, p_name, p_price, p_detail, v_id)
			values ('GS101', '텔레비전', 90000, '커브드형의 미래지향적 최신형 모델', '럭키금성'),
				    ('GS102', '냉장고', 770000, '얼음물에 얼린듯한 그 느낌의 냉장고', '애플'),
					('GS103', '김치냉장고', 550000, '김치는 역시 김치냉장고가 최고', '엘지'),
					('GS104', '오디오', 440000, '오디오의 명가에서 만든 최고의 오디오', '불티나'),
					('GS105', '컴퓨터', 2200000, '세계에서 최고로 가벼운 컴퓨터 그램', '그램');
						

--[3] dbo.tbl_sales1

select * from dbo.tbl_sales1
select * from dbo.tbl_sales2


delete from dbo.tbl_sales1 where s1_num = '1006'
insert into dbo.tbl_sales1(s1_num, s1_date,m_id)
			values
					(1004,'2006-12-28 20:21:20', 'skyman'),
					(1005,'2006-12-29 22:11:10', 'brickman'),
					(1006,'2006-12-29', 'superman');

--다중입력
insert into dbo.tbl_sales2(s2_num, s2_orderitem, p_id, qty, otem_price)
			values
					(1001,1, 'GS101', 1, 990000),
					(1001,2, 'GS102', 1, 770000),
					(1001,3, 'GS103', 1, 550000);
					

--단일입력
insert into dbo.tbl_sales2 VALUES(1002,1,'GS101', 1, 990000);
insert into dbo.tbl_sales2 VALUES(1003,1,'GS103', 1, 550000);
insert into dbo.tbl_sales2 VALUES(1004,1,'GS102', 1, 770000);
insert into dbo.tbl_sales2 VALUES(1005,1,'GS104', 1, 440000);
insert into dbo.tbl_sales2 VALUES(1005,2,'GS105', 1, 2200000);


-- 결제한 회원에 대한 이름, 주소, 이메일을 보고자 한다면?

select * from dbo.tbl_members;
select * from dbo.tbl_sales1;


select m_name, m_address, m_email 
from tbl_members, tbl_sales1
where tbl_members.m_id = tbl_sales1.m_id


--위 쿼리를 별칭을 사용하여 출력해본다.
select m.m_id,m_name, m_address, m_email
from tbl_members m, tbl_sales1 s
where m.m_id = s.m_id

--위 쿼리를 별칭을 사용하여 출력해본다.
select m.m_id as '사람_ID',m_name, m_address, m_email
from tbl_members as m, tbl_sales1 as s1
where m.m_id = s1.m_id


----------------------------------------
-- JOIN (3개 이상 테이블 조인 실습)
----------------------------------------
--Q. 김치 냉장고(GS103)를 구매한 고객의 아이디와 이름을 알고 싶다면?


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
-- 하나 이상의 테이블에 들어 있는 데이터를 가상 테이블로 엮는다.
-- 여러개의 테이블을 묶어서 뷰를 만드는 경우 '복합뷰' 라고도 부른다.

SELECT * from dbo.tbl_members;

-----------------------------------------------------------------
-- [1] VIEW 생성 및 삭제 그리고 사용 : 기본


CREATE VIEW MembersSelectAll
	AS
		SELECT * from dbo.tbl_members;

select * from MembersSelectAll

--------------------------------------------------------------------
-- [2] VIEW 생성 및 삭제 그리고 사용 : 재활용
--------------------------------------------------------------------
--뷰 생성

DROP VIEW MembersSales1Sales2;
CREATE VIEW MembersSales1Sales2
	AS
		--뷰로 묶을 쿼리문 작성
		--뷰는 쿼리로 작성하는 가상 테이블
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



