--뷰 확인
--뷰를 만들면 뷰의 이름이 sysobjects 테이블에 저장된다.
--뷰 열에 관한 정보는 syscolums 테이블에 추가
--뷰 종속 관계에 관한 정보는 sysdepends테이블에 추가
--CREATE VIEW 문의 텍스트는 syscomments 테이블에 추가

select * from sysobjects

select * from sysobjects where xtype = 'U'
 
select * from sysobjects where name = 'MembersSales1Sales2';

select * from sys.objects where name = 'MembersSales1Sales2'

--시스템 뷰 활용
--이러한 뷰를 활용하면 특정 객체를 찾는데 유용하다..
--고급 과정을 공부하는 과정에서는 이러한 뷰를 많이 활용하게 된다.
--Q특정 컬럼이 포함된 테이블을 찾고 싶다면 어떻게 할까

select * from sys.columns where name = 'm_email'




select * 
from sys.objects SO
inner join sys.columns SC on SC.object_id = SO.object_id
where SO.type = 'U'


select * from INFORMATION_SCHEMA.COLUMNS where COLUMN_NAME = 's1_date'

--트랜잭션 카운트
BEGIN TRAN
	SELECT @@TRANCOUNT --1
	UPDATE dbo.tbl_sales2 SET qty=10 where s2_num =1004
	select * from dbo.tbl_sales2
ROLLBACK

commit
select @@TRANCOUNT --0



--TRANSACTION : TRY ~ CATCH


BEGIN TRY
	BEGIN TRANSACTION;
		INSERT INTO dbo.tbl_sales2 values(1006,1, 'GS104', 1, 220000);
		INSERT INTO dbo.tbl_sales2 values(1006,2, 'GS105', 1, 330000);
		select * from dbo.tbl_sales2;
		select 1/0;  -- 오류하나를 발생
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
END CATCH

SELECT @@TRANCOUNT