  DELETE FROM [Countries]
  GO
  DBCC CHECKIDENT ('nscreg.dbo.Countries',RESEED, 0)
  GO

  INSERT INTO [dbo].[Countries] (Code, IsoCode, IsDeleted, Name)
  SELECT 
	 [ALFA_3]
	,[K_STM]
	,0
	,[N_STM]
 FROM [statcom].[dbo].[SPRSTM]
 GO