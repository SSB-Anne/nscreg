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
     ,[OldParentId]
     ,[FullPath])
	 
  SELECT   
	 [NAM1]
	,[K_TER]
	,0
	,[N_TER]
	,0
	,[K_TER_GROUP]
	,NULL
  FROM [statcom].[dbo].[SPRTER]
  GO

  UPDATE ro
  	SET [ParentId] = r.Id
  FROM [dbo].[Regions] r
  	INNER JOIN [dbo].[Regions] ro
  		ON r.Code = ro.OldParentId
  
  ALTER TABLE [dbo].[Regions] DROP COLUMN OldParentId
  GO

 WITH CTE AS (
 SELECT Id, AdminstrativeCenter, Code, IsDeleted, Name, ParentId, Name AS FullPath, ParentId AS LastParentId, 1 AS num
 FROM Regions 

 UNION ALL

 SELECT CTE.Id, CTE.AdminstrativeCenter, CTE.Code, CTE.IsDeleted, CTE.Name, CTE.ParentId, r.Name + ', ' + CTE.FullPath, r.ParentId, num + 1
 FROM CTE 
 INNER JOIN Regions AS r
  ON CTE.LastParentId = r.Id

),
CTE2 AS (
 SELECT Id, AdminstrativeCenter, Code, IsDeleted, Name, ParentId, FullPath, ROW_NUMBER() OVER(PARTITION BY Id ORDER BY num DESC) AS rn
 FROM CTE
)


Update r
	set r.FullPath = cte.FullPath
FROM Regions r
	inner join CTE2 cte
		on cte.Id = r.Id
WHERE rn = 1
GO


