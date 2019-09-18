UPDATE [dbo].[StatisticalUnits] SET [AddressId] = NULL
GO
UPDATE [dbo].[EnterpriseGroups] SET [AddressId] = NULL
GO

DELETE FROM [dbo].[Regions]
GO
DBCC CHECKIDENT ('dbo.Regions',RESEED, 1)
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
  ,[FullPath]
  ,[RegionLevel])
SELECT
  [NAM1]
  ,[K_TER]
  ,0
  ,[N_TER]
  ,0
  ,[K_TER_GROUP]
  ,NULL
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

UPDATE r
	SET r.FullPath = cte.FullPath
FROM Regions r
	INNER JOIN CTE2 cte
		ON cte.Id = r.Id
WHERE rn = 1
GO

-- Add a function that gets the level number, passing the ID
CREATE FUNCTION GetRegionLevel (@input_id INT)
	RETURNS INT
AS BEGIN
    DECLARE @in_id INT = @input_id;
	DECLARE @level INT = 1;

	WHILE @in_id > 0
	BEGIN
		SELECT top 1 @in_id = ParentId FROM Regions WHERE Id = @in_id
		IF @in_id > 0 SET @level = @level + 1;
	END

  RETURN @level
END  
GO

-- Update ActivityCategories with correct level number
UPDATE [dbo].[Regions]
SET [RegionLevel] = dbo.GetRegionLevel(Id)
GO

-- Remove unnecessary function
DROP FUNCTION [dbo].[GetRegionLevel]
GO