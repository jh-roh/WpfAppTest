-------------------------------------------
--[3] MONEY, SUBSTRING ����� ��ȯ
-------------------------------------------
-- �ʺ��ڿ��Դ� REPLACE �Լ��� ����ϴ°ͺ��� ���� ��ƴ�
-- SUBSTRING, CONVERT, LEN �Լ����� ������ �����Ѵ�.


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
-- �������� ���ؼ� �Լ������ �Ʒ��� ���� ������ �ƴ� �����ٿ� ���� ����ص� �ȴ�.
select substring
(
	CONVERT(VARCHAR, CONVERT(MONEY, 123456789, 1)),		  --SUBSTRING 1���� ��
	1,													  --SUBSTRING 2���� ��
	LEN(CONVERT(VARCHAR, CONVERT(MONEY, 123456789, 1)))-3 --SUBSTRIGN 3���� ��
)
AS '.00 ����'