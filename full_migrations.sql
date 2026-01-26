IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260123083455_InitialCreate', N'9.0.1');

DROP INDEX [IX_Users_Email] ON [Users];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'CreatedAt');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] DROP COLUMN [CreatedAt];

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'FirstName');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] DROP COLUMN [FirstName];

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'LastName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] DROP COLUMN [LastName];

EXEC sp_rename N'[Users].[UpdatedAt]', N'UpdatedDate', 'COLUMN';

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(255) NULL;

ALTER TABLE [Users] ADD [Address] nvarchar(255) NULL;

ALTER TABLE [Users] ADD [Avatar] nvarchar(max) NULL;

ALTER TABLE [Users] ADD [Bank] nvarchar(36) NULL;

ALTER TABLE [Users] ADD [BankAccount] nvarchar(36) NULL;

ALTER TABLE [Users] ADD [BirthDay] datetime2 NULL;

ALTER TABLE [Users] ADD [BranchId] int NULL;

ALTER TABLE [Users] ADD [CreatedDate] datetime2 NULL;

ALTER TABLE [Users] ADD [DepartmentId] int NULL;

ALTER TABLE [Users] ADD [DistrictId] int NULL;

ALTER TABLE [Users] ADD [FullName] nvarchar(255) NULL;

ALTER TABLE [Users] ADD [Gender] int NULL;

ALTER TABLE [Users] ADD [Identify] nvarchar(36) NULL;

ALTER TABLE [Users] ADD [IdentifyCreatedDate] datetime2 NULL;

ALTER TABLE [Users] ADD [IdentifyExpiredDate] datetime2 NULL;

ALTER TABLE [Users] ADD [Images] nvarchar(max) NULL;

ALTER TABLE [Users] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Users] ADD [PasswordHash] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Users] ADD [PersonalTaxCode] nvarchar(36) NULL;

ALTER TABLE [Users] ADD [Phone] nvarchar(20) NULL;

ALTER TABLE [Users] ADD [PositionDetailId] int NULL;

ALTER TABLE [Users] ADD [ProvinceId] int NULL;

ALTER TABLE [Users] ADD [RequestPassword] bit NULL;

ALTER TABLE [Users] ADD [Salary] float NULL;

ALTER TABLE [Users] ADD [SocialInsuranceCode] nvarchar(36) NULL;

ALTER TABLE [Users] ADD [TargetId] int NULL;

ALTER TABLE [Users] ADD [Timekeeper] bit NULL;

ALTER TABLE [Users] ADD [UserCreated] int NULL;

ALTER TABLE [Users] ADD [UserRoleIds] nvarchar(max) NULL;

ALTER TABLE [Users] ADD [UserUpdated] int NULL;

ALTER TABLE [Users] ADD [Username] nvarchar(255) NOT NULL DEFAULT N'';

ALTER TABLE [Users] ADD [WardId] int NULL;

ALTER TABLE [Users] ADD [YearCurrent] int NULL;

CREATE TABLE [Menus] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(36) NOT NULL,
    [Name] nvarchar(255) NULL,
    [NameEN] nvarchar(255) NULL,
    [NameKO] nvarchar(255) NULL,
    [CodeParent] nvarchar(36) NULL,
    [IsParent] bit NULL,
    [Order] int NULL,
    [Note] nvarchar(255) NULL,
    CONSTRAINT [PK_Menus] PRIMARY KEY ([Id]),
    CONSTRAINT [AK_Menus_Code] UNIQUE ([Code]),
    CONSTRAINT [FK_Menus_Menus_CodeParent] FOREIGN KEY ([CodeParent]) REFERENCES [Menus] ([Code]) ON DELETE NO ACTION
);

CREATE TABLE [UserRoles] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(36) NOT NULL,
    [Title] nvarchar(36) NULL,
    [Note] nvarchar(255) NULL,
    [Order] int NULL,
    [UserCreated] int NULL,
    [IsNotAllowDelete] bit NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [MenuRoles] (
    [Id] int NOT NULL IDENTITY,
    [MenuId] int NULL,
    [UserRoleId] int NULL,
    [MenuCode] nvarchar(max) NULL,
    [View] bit NULL,
    [Add] bit NULL,
    [Edit] bit NULL,
    [Delete] bit NULL,
    CONSTRAINT [PK_MenuRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MenuRoles_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menus] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MenuRoles_UserRoles_UserRoleId] FOREIGN KEY ([UserRoleId]) REFERENCES [UserRoles] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);

CREATE INDEX [IX_MenuRoles_MenuId] ON [MenuRoles] ([MenuId]);

CREATE INDEX [IX_MenuRoles_UserRoleId] ON [MenuRoles] ([UserRoleId]);

CREATE UNIQUE INDEX [IX_Menus_Code] ON [Menus] ([Code]);

CREATE INDEX [IX_Menus_CodeParent] ON [Menus] ([CodeParent]);

CREATE UNIQUE INDEX [IX_UserRoles_Code] ON [UserRoles] ([Code]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260124030124_AddAuthTables', N'9.0.1');

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PasswordHash');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Users] DROP COLUMN [PasswordHash];

ALTER TABLE [Users] ADD [PasswordHash] varbinary(max) NOT NULL DEFAULT 0x;

ALTER TABLE [Users] ADD [Language] nvarchar(max) NULL;

ALTER TABLE [Users] ADD [LastLogin] datetime2 NULL;

ALTER TABLE [Users] ADD [PasswordSalt] varbinary(max) NOT NULL DEFAULT 0x;

ALTER TABLE [Users] ADD [Status] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260125052508_UpdateUserPasswordToBytes', N'9.0.1');

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(50) NULL,
    [Name] nvarchar(255) NULL,
    [Avatar] nvarchar(255) NULL,
    [Phone] nvarchar(20) NULL,
    [Email] nvarchar(255) NULL,
    [PasswordHash] varbinary(max) NULL,
    [PasswordSalt] varbinary(max) NULL,
    [Address] nvarchar(500) NULL,
    [ProvinceId] int NULL,
    [DistrictId] int NULL,
    [WardId] int NULL,
    [Gender] int NULL,
    [Provider] nvarchar(50) NULL,
    [ProviderId] nvarchar(255) NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Customers_Phone] ON [Customers] ([Phone]) WHERE [Phone] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260125055658_AddCustomerAndUpdateModels', N'9.0.1');

CREATE TABLE [Allowances] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Allowances] PRIMARY KEY ([Id])
);

CREATE TABLE [AllowanceUsers] (
    [Id] int NOT NULL IDENTITY,
    [AllowanceId] int NOT NULL,
    [UserId] int NOT NULL,
    [StartDate] datetime2 NULL,
    [EndDate] datetime2 NULL,
    [AmountOverride] decimal(18,2) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_AllowanceUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Certificates] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Issuer] nvarchar(150) NULL,
    [IssueDate] datetime2 NULL,
    [ExpiryDate] datetime2 NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Certificates] PRIMARY KEY ([Id])
);

CREATE TABLE [ContractFiles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [FileUrl] nvarchar(255) NULL,
    [ContractTypeId] int NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_ContractFiles] PRIMARY KEY ([Id])
);

CREATE TABLE [ContractTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [Description] nvarchar(255) NULL,
    [DurationMonths] int NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_ContractTypes] PRIMARY KEY ([Id])
);

CREATE TABLE [Decides] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [DecisionTypeId] int NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    [EffectiveDate] datetime2 NULL,
    [ExpiredDate] datetime2 NULL,
    [FileUrl] nvarchar(255) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Decides] PRIMARY KEY ([Id])
);

CREATE TABLE [DecisionTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_DecisionTypes] PRIMARY KEY ([Id])
);

CREATE TABLE [Degrees] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [School] nvarchar(255) NULL,
    [Description] nvarchar(255) NULL,
    [GraduationYear] int NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Degrees] PRIMARY KEY ([Id])
);

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [ParentId] int NULL,
    [Order] int NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);

CREATE TABLE [HistoryAchievements] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NULL,
    [AchievedDate] datetime2 NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_HistoryAchievements] PRIMARY KEY ([Id])
);

CREATE TABLE [Majors] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Majors] PRIMARY KEY ([Id])
);

CREATE TABLE [PositionDetails] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [DepartmentId] int NULL,
    [Note] nvarchar(255) NULL,
    [Order] int NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_PositionDetails] PRIMARY KEY ([Id])
);

CREATE TABLE [Relatives] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Relationship] nvarchar(50) NOT NULL,
    [Phone] nvarchar(20) NULL,
    [Address] nvarchar(255) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Relatives] PRIMARY KEY ([Id])
);

CREATE TABLE [SalaryTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Code] nvarchar(50) NULL,
    [BaseAmount] decimal(18,2) NOT NULL,
    [Formula] nvarchar(255) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_SalaryTypes] PRIMARY KEY ([Id])
);

CREATE TABLE [TimeKeepingEntries] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [WorkDate] datetime2 NOT NULL,
    [CheckIn] datetime2 NULL,
    [CheckOut] datetime2 NULL,
    [WorkingHours] float NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_TimeKeepingEntries] PRIMARY KEY ([Id])
);

CREATE TABLE [UserContractHistories] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ContractTypeId] int NOT NULL,
    [SignedDate] datetime2 NULL,
    [StartDate] datetime2 NULL,
    [EndDate] datetime2 NULL,
    [FileUrl] nvarchar(255) NULL,
    [Note] nvarchar(255) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_UserContractHistories] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260126141209_AddQLNSModule', N'9.0.1');

CREATE TABLE [UserCompanies] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [CompanyCode] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_UserCompanies] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_UserCompanies_UserId_CompanyCode] ON [UserCompanies] ([UserId], [CompanyCode]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260126142857_AddUserCompany', N'9.0.1');

COMMIT;
GO

