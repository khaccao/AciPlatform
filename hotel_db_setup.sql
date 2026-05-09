USE master;
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AciPlatform_Hotel')
BEGIN
    CREATE DATABASE AciPlatform_Hotel COLLATE Vietnamese_CI_AS;
    PRINT 'Database AciPlatform_Hotel created.';
END
GO
USE AciPlatform_Hotel;
GO

-- HotelProperties
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelProperties' AND xtype='U')
BEGIN
    CREATE TABLE HotelProperties (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Code            NVARCHAR(50)  NOT NULL,
        Name            NVARCHAR(200) NOT NULL,
        ShortName       NVARCHAR(50),
        LogoUrl         NVARCHAR(500),
        Address         NVARCHAR(500),
        Phone           NVARCHAR(50),
        Email           NVARCHAR(200),
        PmsConnectionString NVARCHAR(1000),
        PmsDbName       NVARCHAR(100),
        PmsIpAddress    NVARCHAR(200),
        DmsAppId        NVARCHAR(50),
        DmsAppSecret    NVARCHAR(100),
        IsAutoUpdateOTA INT DEFAULT 0,
        OtaTimesAuto    NVARCHAR(20),
        IsActive        BIT NOT NULL DEFAULT 1,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelProperties_Code UNIQUE (Code),
        CONSTRAINT UQ_HotelProperties_Guid UNIQUE (Guid)
    );
    PRINT 'Table HotelProperties created.';
END
GO

-- HotelUserMappings
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelUserMappings' AND xtype='U')
BEGIN
    CREATE TABLE HotelUserMappings (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Guid        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        AciUserId   INT NOT NULL,
        HotelCode   NVARCHAR(50) NOT NULL,
        HotelGuid   UNIQUEIDENTIFIER NOT NULL,
        UserFO      NVARCHAR(50),
        UserBO      NVARCHAR(50),
        UserPOS     NVARCHAR(50),
        IsDefault   BIT NOT NULL DEFAULT 0,
        ValidDate   DATETIME,
        Status      INT NOT NULL DEFAULT 1,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CONSTRAINT UQ_HotelUserMappings UNIQUE (AciUserId, HotelCode)
    );
    PRINT 'Table HotelUserMappings created.';
END
GO

-- HotelAreas
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelAreas' AND xtype='U')
BEGIN
    CREATE TABLE HotelAreas (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        ParentId        INT,
        ParentGuid      UNIQUEIDENTIFIER,
        HotelCode       NVARCHAR(50) NOT NULL,
        HotelGuid       UNIQUEIDENTIFIER NOT NULL,
        AreaCode        NVARCHAR(50),
        AreaName        NVARCHAR(200) NOT NULL,
        AreaType        NVARCHAR(50),
        AreaTypeGuid    UNIQUEIDENTIFIER,
        AreaAlias       NVARCHAR(500),
        AreaDescription NVARCHAR(500),
        AreaAvatar      NVARCHAR(500),
        Color           NVARCHAR(20),
        PositionX       INT,
        PositionY       INT,
        Width           INT,
        Height          INT,
        DmsLockId       BIGINT,
        DmsHardwareId   NVARCHAR(50),
        IsActive        BIT NOT NULL DEFAULT 1,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT FK_HotelAreas_Parent FOREIGN KEY (ParentId) REFERENCES HotelAreas(Id)
    );
    PRINT 'Table HotelAreas created.';
END
GO

-- HotelAreaTypes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelAreaTypes' AND xtype='U')
BEGIN
    CREATE TABLE HotelAreaTypes (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Guid        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode   NVARCHAR(50) NOT NULL,
        HotelGuid   UNIQUEIDENTIFIER NOT NULL,
        GroupCode   NVARCHAR(50),
        Code        NVARCHAR(50) NOT NULL,
        Name        NVARCHAR(200) NOT NULL,
        Descriptions NVARCHAR(500),
        CONSTRAINT UQ_HotelAreaTypes UNIQUE (Code, HotelGuid)
    );
    PRINT 'Table HotelAreaTypes created.';
END
GO

-- HotelElements
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelElements' AND xtype='U')
BEGIN
    CREATE TABLE HotelElements (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Guid        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode   NVARCHAR(50) NOT NULL,
        HotelGuid   UNIQUEIDENTIFIER NOT NULL,
        AreaId      INT NOT NULL,
        AreaGuid    UNIQUEIDENTIFIER NOT NULL,
        Name        NVARCHAR(200) NOT NULL,
        Alias       NVARCHAR(200),
        Type        NVARCHAR(50) NOT NULL,
        Capacity    INT,
        Description NVARCHAR(500),
        PositionX   INT NOT NULL DEFAULT 0,
        PositionY   INT NOT NULL DEFAULT 0,
        Width       INT,
        Height      INT,
        Rotation    INT NOT NULL DEFAULT 0,
        Color       NVARCHAR(20),
        Icon        NVARCHAR(10),
        Settings    NVARCHAR(MAX),
        IsActive    BIT NOT NULL DEFAULT 1,
        IsOccupied  BIT NOT NULL DEFAULT 0,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        CONSTRAINT FK_HotelElements_Area FOREIGN KEY (AreaId) REFERENCES HotelAreas(Id)
    );
    PRINT 'Table HotelElements created.';
END
GO

-- PMS_RoomTypes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_RoomTypes' AND xtype='U')
BEGIN
    CREATE TABLE PMS_RoomTypes (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        PmsItemId   INT,
        Ma          NVARCHAR(50),
        Ten         NVARCHAR(200),
        DonGia      DECIMAL(18,2),
        MaxPerson   INT,
        SoLuong     INT,
        FlagType    INT,
        IsActive    BIT NOT NULL DEFAULT 1,
        SyncDate    DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PMS_RoomTypes created.';
END
GO

-- PMS_Rooms
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_Rooms' AND xtype='U')
BEGIN
    CREATE TABLE PMS_Rooms (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        PmsRoomId   INT,
        So          NVARCHAR(20),
        Loai        INT,
        Ma          NVARCHAR(50),
        Ten         NVARCHAR(200),
        Floor       NVARCHAR(10),
        KhuVucCode  NVARCHAR(50),
        BuildingID  NVARCHAR(50),
        SachBan     INT,
        CleanDirty  INT,
        Inspected   INT,
        TinhTrang   INT,
        Status      NVARCHAR(20),
        IsActive    BIT NOT NULL DEFAULT 1,
        SyncDate    DATETIME DEFAULT GETDATE()
    );
    CREATE INDEX IX_PMS_Rooms_HotelCode ON PMS_Rooms(HotelCode);
    PRINT 'Table PMS_Rooms created.';
END
GO

-- PMS_GuestsInHouse
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_GuestsInHouse' AND xtype='U')
BEGIN
    CREATE TABLE PMS_GuestsInHouse (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode       NVARCHAR(50) NOT NULL,
        TSRoomID        INT,
        RSVN            NVARCHAR(50),
        RoomID          INT,
        RoomNo          NVARCHAR(20),
        GuestID         INT,
        ArrivalDate     DATETIME,
        DepartureDate   DATETIME,
        Rate            DECIMAL(18,2),
        Ten             NVARCHAR(200),
        GuestName       NVARCHAR(200),
        GuestFullName   NVARCHAR(400),
        FirstName       NVARCHAR(200),
        Company         NVARCHAR(200),
        CompanyName     NVARCHAR(200),
        HouseUse        INT,
        NoPost          INT,
        Comp            INT,
        Status          INT,
        GroupFolio      INT,
        SyncDate        DATETIME DEFAULT GETDATE()
    );
    CREATE INDEX IX_PMS_Guests ON PMS_GuestsInHouse(HotelCode, RoomNo);
    PRINT 'Table PMS_GuestsInHouse created.';
END
GO

-- PMS_HousekeepingLogs
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_HousekeepingLogs' AND xtype='U')
BEGIN
    CREATE TABLE PMS_HousekeepingLogs (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        RoomId      INT,
        RoomNo      NVARCHAR(20),
        Floor       NVARCHAR(10),
        ActionType  NVARCHAR(20) NOT NULL,
        OldStatus   INT,
        NewStatus   INT,
        UserFO      NVARCHAR(50),
        AciUserId   INT,
        ActionDate  DATETIME NOT NULL DEFAULT GETDATE(),
        Notes       NVARCHAR(500)
    );
    PRINT 'Table PMS_HousekeepingLogs created.';
END
GO

-- PMS_MinibarItems
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_MinibarItems' AND xtype='U')
BEGIN
    CREATE TABLE PMS_MinibarItems (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        PmsItemId   INT,
        Parent      INT,
        Ma          NVARCHAR(50),
        Ten         NVARCHAR(200),
        TenNN       NVARCHAR(200),
        QuyCach     NVARCHAR(100),
        DonViTinh   NVARCHAR(50),
        DonGia      DECIMAL(18,2),
        GhiChu1     NVARCHAR(500),
        PhanLoai    NVARCHAR(100),
        TyLeSC      DECIMAL(5,2),
        TyLeThueVAT DECIMAL(5,2),
        StatusRec   INT DEFAULT 1,
        SyncDate    DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PMS_MinibarItems created.';
END
GO

-- PMS_LaundryItems
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_LaundryItems' AND xtype='U')
BEGIN
    CREATE TABLE PMS_LaundryItems (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        PmsItemId   INT,
        Ma          NVARCHAR(50),
        Ten         NVARCHAR(200),
        TenNN       NVARCHAR(200),
        Laundry     DECIMAL(18,2),
        Dry         DECIMAL(18,2),
        PressingOnly DECIMAL(18,2),
        TyLeSC      DECIMAL(5,2),
        TyLeThueVAT DECIMAL(5,2),
        GhiChu      NVARCHAR(500),
        Status      INT DEFAULT 1,
        SyncDate    DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PMS_LaundryItems created.';
END
GO

-- PMS_MinibarOrders
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_MinibarOrders' AND xtype='U')
BEGIN
    CREATE TABLE PMS_MinibarOrders (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        Parent          INT,
        So              NVARCHAR(50),
        NgayThang       DATETIME NOT NULL DEFAULT GETDATE(),
        ThanhTien       DECIMAL(18,2),
        TyLeGiamTru     DECIMAL(5,2),
        SoTienGiamTru   DECIMAL(18,2),
        TongSoTien      DECIMAL(18,2),
        ServicesCharge  DECIMAL(18,2),
        VATCharge       DECIMAL(18,2),
        GhiChu          NVARCHAR(500),
        NguoiDung       SMALLINT,
        Guest           INT,
        FreeCharge      BIT DEFAULT 0,
        Status          SMALLINT DEFAULT 1,
        NightAuditorRun SMALLINT DEFAULT 0,
        CreatedDate     DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PMS_MinibarOrders created.';
END
GO

-- PMS_MinibarOrderDetails
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_MinibarOrderDetails' AND xtype='U')
BEGIN
    CREATE TABLE PMS_MinibarOrderDetails (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        OrderId     INT NOT NULL,
        HangHoa     INT,
        SoLuong     DECIMAL(18,2),
        DonGia      DECIMAL(18,2),
        ThanhTien   DECIMAL(18,2),
        TyLeSC      DECIMAL(5,2),
        TyLeVAT     DECIMAL(5,2),
        Status      SMALLINT,
        CONSTRAINT FK_MinibarDetail_Order FOREIGN KEY (OrderId) REFERENCES PMS_MinibarOrders(Id)
    );
    PRINT 'Table PMS_MinibarOrderDetails created.';
END
GO

-- PMS_LaundryOrders
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_LaundryOrders' AND xtype='U')
BEGIN
    CREATE TABLE PMS_LaundryOrders (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        Parent          INT,
        So              NVARCHAR(50),
        NgayThang       DATETIME NOT NULL DEFAULT GETDATE(),
        ThanhTien       DECIMAL(18,2),
        TyLeGiamTru     DECIMAL(5,2),
        SoTienGiamTru   DECIMAL(18,2),
        TongSoTien      DECIMAL(18,2),
        TongVND         DECIMAL(18,2),
        ServicesCharge  DECIMAL(18,2),
        VATCharge       DECIMAL(18,2),
        GhiChu          NVARCHAR(500),
        NguoiDung       SMALLINT,
        Guest           INT,
        FreeCharge      BIT DEFAULT 0,
        IsExpress       BIT DEFAULT 0,
        Status          SMALLINT DEFAULT 1,
        NightAuditorRun SMALLINT DEFAULT 0,
        CreatedDate     DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PMS_LaundryOrders created.';
END
GO

-- PMS_LaundryOrderDetails
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PMS_LaundryOrderDetails' AND xtype='U')
BEGIN
    CREATE TABLE PMS_LaundryOrderDetails (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        OrderId     INT NOT NULL,
        HangHoa     INT,
        SoLuong     DECIMAL(18,2),
        DonGia      DECIMAL(18,2),
        ThanhTien   DECIMAL(18,2),
        TyLeSC      DECIMAL(5,2),
        TyLeVAT     DECIMAL(5,2),
        FlagType    SMALLINT,
        Status      SMALLINT,
        CONSTRAINT FK_LaundryDetail_Order FOREIGN KEY (OrderId) REFERENCES PMS_LaundryOrders(Id)
    );
    PRINT 'Table PMS_LaundryOrderDetails created.';
END
GO

-- SEED DATA
IF NOT EXISTS (SELECT 1 FROM HotelProperties WHERE Code = 'CITITEL')
    INSERT INTO HotelProperties (Code, Name, ShortName, Address, IsActive)
    VALUES ('CITITEL', 'Cititel Hotel', 'Cititel', 'Ha Noi, Viet Nam', 1);

IF NOT EXISTS (SELECT 1 FROM HotelProperties WHERE Code = 'SONAGA')
    INSERT INTO HotelProperties (Code, Name, ShortName, Address, IsActive)
    VALUES ('SONAGA', 'Sonaga Hotel', 'Sonaga', 'TP.HCM, Viet Nam', 1);
GO

PRINT '=== AciPlatform_Hotel setup COMPLETED (12 tables) ===';
GO
