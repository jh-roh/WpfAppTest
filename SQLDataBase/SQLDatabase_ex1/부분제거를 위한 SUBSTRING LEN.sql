-------------------------------------------
--[3] MONEY, SUBSTRING 사용한 변환
-------------------------------------------
-- 초보자에게는 REPLACE 함수를 사용하는것보다 조금 어렵다
-- SUBSTRING, CONVERT, LEN 함수등의 사용법을 숙지한다.


--SUBSTRING(expressing,start,length)

select (123456789);
select CONVERT(MONEY, '123456789');
select convert(varchar,CONVERT(MONEY, '123456789'), 1);
select substring(convert(varchar,CONVERT(MONEY, '123456789'), 1), 1, 11);


--len
select CONVERT(varchar,convert(money,123456789),1);
select len(CONVERT(varchar,convert(money,123456789),1));
select len(CONVERT(varchar,convert(money,123456789),1)) -3;


-- SUBSTRING + CONVERT + LEN
-- 가독성을 위해서 함수사용을 아래와 같이 한줄이 아닌 여러줄에 걸쳐 사용해도 된다.
select substring
(
	CONVERT(VARCHAR, CONVERT(MONEY, 123456789, 1)),		  --SUBSTRING 1번쨰 값
	1,													  --SUBSTRING 2번쨰 값
	LEN(CONVERT(VARCHAR, CONVERT(MONEY, 123456789, 1)))-3 --SUBSTRIGN 3번쨰 값
)
AS '.00 제거'