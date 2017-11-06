  UPDATE [dbo].[StatisticalUnits] SET [AddressId] = NULL
  GO
  UPDATE [dbo].[EnterpriseGroups] SET [AddressId] = NULL
  GO

  DELETE FROM [dbo].[Regions]
  GO
  DBCC CHECKIDENT ('nscreg.dbo.Regions',RESEED, 0)
  GO

  ALTER TABLE [dbo].[Regions] ADD OldParentId NVARCHAR(20) NULL
  GO

  INSERT INTO [dbo].[Regions]
    ([AdminstrativeCenter]
     ,[Code]
     ,[IsDeleted]
     ,[Name]
     ,[ParentId]
	 ,[OldParentId])
  SELECT   
	 [NAM1]
	,[K_TER]
	,0
	,[N_TER]
	,0
	,[K_TER_GROUP]
  FROM [statcom].[dbo].[SPRTER]
  GO

  UPDATE ro
  	SET [ParentId] = r.Id
  FROM [dbo].[Regions] r
  	INNER JOIN [dbo].[Regions] ro
  		ON r.Code = ro.OldParentId
  
  ALTER TABLE [dbo].[Regions] DROP COLUMN OldParentId
  GO