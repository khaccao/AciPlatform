-- Fix Migration History Issue
-- This script manually inserts the missing migration record

-- Check if RefreshTokens table exists
IF OBJECT_ID(N'[RefreshTokens]', N'U') IS NOT NULL
BEGIN
    PRINT 'RefreshTokens table exists. Adding migration record to history...'
    
    -- Check if migration record exists
    IF NOT EXISTS (
        SELECT 1 FROM [__EFMigrationsHistory] 
        WHERE [MigrationId] = N'20260126150817_AddRefreshToken'
    )
    BEGIN
        -- Insert the missing migration record
        INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES (N'20260126150817_AddRefreshToken', N'9.0.1');
        
        PRINT 'Migration record added successfully.'
    END
    ELSE
    BEGIN
        PRINT 'Migration record already exists.'
    END
END
ELSE
BEGIN
    PRINT 'RefreshTokens table does not exist. Run migration normally.'
END

-- Check if UserMenus table exists
IF OBJECT_ID(N'[UserMenus]', N'U') IS NOT NULL
BEGIN
    PRINT 'UserMenus table exists. Adding migration record to history...'
    
    -- Check if migration record exists
    IF NOT EXISTS (
        SELECT 1 FROM [__EFMigrationsHistory] 
        WHERE [MigrationId] = N'20260129101319_AddUserMenus'
    )
    BEGIN
        -- Insert the missing migration record
        INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES (N'20260129101319_AddUserMenus', N'9.0.1');
        
        PRINT 'UserMenus migration record added successfully.'
    END
    ELSE
    BEGIN
        PRINT 'UserMenus migration record already exists.'
    END
END

PRINT 'Migration history fixed. You can now run the application.'
GO
