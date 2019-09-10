/* All EnterpriseUnit must have EntGroupIdDate defined */
UPDATE [dbo].[StatisticalUnits] SET [EntGroupIdDate]=GETDATE() 
  FROM [dbo].[StatisticalUnits] WHERE [Discriminator]='EnterpriseUnit' AND [EntGroupIdDate] IS NULL

 /* All LegalUnit must have Market defined */
UPDATE [dbo].[StatisticalUnits] SET [Market]=0
  FROM [dbo].[StatisticalUnits] WHERE [Discriminator]='LegalUnit' AND [Market] IS NULL

 /* Units that are not liquidated must not have LiqDate defined */
UPDATE [dbo].[StatisticalUnits] SET [dbo].[StatisticalUnits].[LiqDate]=NULL, [dbo].[StatisticalUnits].[LiqReason]=NULL
  FROM [dbo].[StatisticalUnits]
  JOIN [dbo].[Statuses] ON [dbo].[StatisticalUnits].[UnitStatusId]=[dbo].[Statuses].[Id]
  WHERE [dbo].[Statuses].[Code]<>'7' AND [LiqDate] IS NOT NULL
