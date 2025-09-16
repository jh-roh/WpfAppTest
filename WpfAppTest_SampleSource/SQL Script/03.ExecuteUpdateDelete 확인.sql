/*
ExecuteDelete, ExecuteUpdate
*/
SELECT TOP (100) *
  FROM [SalesSimple].[dbo].[DML_Master] WITH(NOLOCK)

SELECT TOP (100) *
  FROM [SalesSimple].[dbo].Orders WITH(NOLOCK)
  ORDER BY OrderKey

SELECT TOP (100) *
  FROM [SalesSimple].[dbo].LineItems WITH(NOLOCK)
  ORDER BY OrderKey

