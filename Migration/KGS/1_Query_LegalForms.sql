  DELETE FROM [dbo].[LegalForms]
  GO
  DBCC CHECKIDENT ('nscreg.dbo.LegalForms',RESEED, 0)
  GO

  INSERT INTO [dbo].[LegalForms]
           ([Code]
           ,[IsDeleted]
           ,[Name]
           ,[ParentId])
  SELECT 
	   [K_OPF]
	  ,0
      ,[N_OPF]
      ,NULL
  FROM [statcom].[dbo].[SPROPF]
  GO