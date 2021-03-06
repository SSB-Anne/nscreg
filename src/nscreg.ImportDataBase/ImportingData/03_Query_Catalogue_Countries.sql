DELETE FROM [Countries]
GO
DBCC CHECKIDENT ('dbo.Countries',RESEED, 1)
GO

-- Strict insert, please edit insert values

INSERT INTO [dbo].[Countries]
  ([Code],[IsDeleted],[IsoCode],[Name],[NameLanguage1],[NameLanguage2])
VALUES
  ('JPN', 0, '392', 'Japan', 'NameLanguage1', 'NameLanguage2'),
  ('KOR', 0, '410', 'Korea', 'NameLanguage1', 'NameLanguage2')
GO

-- OR select from other database

-- INSERT INTO [dbo].[Countries]
--   ([Code],[IsDeleted],[IsoCode],[Name],[NameLanguage1],[NameLanguage2])
-- SELECT 
--   [ALFA_3]
--   ,0
--   ,[K_STM]
--   ,[N_STM]
--   ,NULL
--   ,NULL
-- FROM [statcom].[dbo].[SPRSTM]
-- GO