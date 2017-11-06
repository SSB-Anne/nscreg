  DELETE FROM [dbo].[DictionaryVersions]
  GO
  DBCC CHECKIDENT ('nscreg.dbo.DictionaryVersions',RESEED, 0)
  GO
  
  INSERT INTO [dbo].[DictionaryVersions] ([VersionId],[VersionName])
  VALUES (1,'Migration version')
  GO
    
  DELETE FROM [dbo].[ActivityCategories]
  GO
  DBCC CHECKIDENT ('nscreg.dbo.ActivityCategories',RESEED, 0)
  GO

  ALTER TABLE [dbo].[ActivityCategories] ADD OldParentId NVARCHAR(20) NULL
  GO

  INSERT INTO [dbo].[ActivityCategories]
    ([Code]
    ,[IsDeleted]
    ,[Name]
    ,[ParentId]
    ,[Section]
	,[OldParentId]
	,[VersionId]
	,[DicParentId])
  SELECT   
	 [KOD]
	,0
	,[NAME]
	,0
	,[KODA]
	,[PARENT_ID]
	,1
	,NULL
  FROM [statcom].[dbo].[OKED3]
  GO

  UPDATE aco
	SET [DicParentId] = aco.Id
  FROM [dbo].[ActivityCategories] aco
  GO

  UPDATE aco
  	SET [ParentId] = ac.Id
  FROM [dbo].[ActivityCategories] ac
  	INNER JOIN [dbo].[ActivityCategories] aco
  		ON ac.Code = aco.OldParentId
  GO
  
  ALTER TABLE [dbo].[ActivityCategories] DROP COLUMN OldParentId
  GO