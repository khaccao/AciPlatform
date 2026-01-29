-- Check current migration status
SELECT [MigrationId], [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];

-- Check if tables exist
SELECT 
    CASE WHEN OBJECT_ID(N'[RefreshTokens]', N'U') IS NOT NULL THEN 'EXISTS' ELSE 'NOT EXISTS' END AS RefreshTokensTable,
    CASE WHEN OBJECT_ID(N'[UserMenus]', N'U') IS NOT NULL THEN 'EXISTS' ELSE 'NOT EXISTS' END AS UserMenusTable;
