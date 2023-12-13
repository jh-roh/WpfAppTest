----------------------------------------------------------
--[1] NUMERICE, DECIMAL 데이터타입(수)
----------------------------------------------------------

--CONVERT 함수와 함꼐 사용하는 것에 대해서 숙지(문자 --> 숫자 변환)
--소숫점 이하에서 반올림 지정도 가능

select  CONVERT(INT,'1234')		           AS 정수,
	    CONVERT(FLOAT, '1234.56')          AS '실수(부동소수점)',
		CONVERT(NUMERIC, '1234.5678')	   AS 실수_NUMERIC,
		CONVERT(DECIMAL, '1234.5678')      AS 실수_DECIMAL,
		CONVERT(NUMERIC(8,3), '1234.5678') AS 실수,
		CONVERT(DECIMAL(8,3), '1234.5674') AS 실수
----------------------------------------------------------
--[2] MONEY, DECIMAL 데이터타입(수)
----------------------------------------------------------
--세자리마다 콤마를 찍어주기 위해서는 MONEY로 지정하거나 또는 MONEY로 컨버팅
--타입이 VARCHAR라도 숫자 포맷이면 변환이 가능

select (123456789)
select CONVERT(MONEY, 123456789)
select CONVERT(VARCHAR, CONVERT(MONEY, 123456789), 1)
select CONVERT(VARCHAR, CONVERT(decimal(12,2), 123456789), 1)
select REPLACE(CONVERT(VARCHAR, CONVERT(MONEY, 123456789), 1), '.00', '');

----------------------------------------------------------
--[3] MONEY, SUBSTRING
----------------------------------------------------------
--SUBSTRING, CONVERT,LEN 함수 등의 사용법을 숙지한다.

--SUBSTRING(expression, start, length)
SELECT SUBSTRING('abcdef', 2,3)
SELECT SUBSTRING('123456789', 1,2)
SELECT SUBSTRING('123456789', 3,4)













