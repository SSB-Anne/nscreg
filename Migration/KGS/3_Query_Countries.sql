DELETE FROM [Countries]
GO
DBCC CHECKIDENT ('dbo.Countries',RESEED, 1)
GO

INSERT INTO [dbo].[Countries] 
  ([Code]
  ,[IsoCode]
  ,[IsDeleted]
  ,[Name])
SELECT 
  [ALFA_3]
  ,[K_STM]
  ,0
  ,[N_STM]
FROM [statcom].[dbo].[SPRSTM]
GO