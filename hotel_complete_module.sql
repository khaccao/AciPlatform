-- ============================================================
-- HOTEL COMPLETE MODULE: Remaining Tables
-- Tour, GroupMembers, Reports, PropertySettings
-- ============================================================
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON;
GO
USE AciPlatform_Hotel;
GO

-- Add missing columns to HotelProperties if needed
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'HotelType')
    ALTER TABLE HotelProperties ADD HotelType NVARCHAR(50) DEFAULT 'HOTEL';
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'IsLinkedToAciCompany')
    ALTER TABLE HotelProperties ADD IsLinkedToAciCompany BIT DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'StarRating')
    ALTER TABLE HotelProperties ADD StarRating INT DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'CheckInTime')
    ALTER TABLE HotelProperties ADD CheckInTime NVARCHAR(10) DEFAULT '14:00';
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'CheckOutTime')
    ALTER TABLE HotelProperties ADD CheckOutTime NVARCHAR(10) DEFAULT '12:00';
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'Currency')
    ALTER TABLE HotelProperties ADD Currency NVARCHAR(10) DEFAULT 'VND';
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'Website')
    ALTER TABLE HotelProperties ADD Website NVARCHAR(200) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelProperties') AND name = 'Description')
    ALTER TABLE HotelProperties ADD Description NVARCHAR(2000) NULL;
PRINT 'HotelProperties columns updated.';
GO

-- Add missing columns to PMS_Rooms
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'RoomTypeId')
    ALTER TABLE PMS_Rooms ADD RoomTypeId INT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'MaxPerson')
    ALTER TABLE PMS_Rooms ADD MaxPerson INT DEFAULT 2;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'BasePrice')
    ALTER TABLE PMS_Rooms ADD BasePrice DECIMAL(18,2) DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'AreaId')
    ALTER TABLE PMS_Rooms ADD AreaId INT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'Description')
    ALTER TABLE PMS_Rooms ADD Description NVARCHAR(500) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'ImageUrl')
    ALTER TABLE PMS_Rooms ADD ImageUrl NVARCHAR(500) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'IsDeleted')
    ALTER TABLE PMS_Rooms ADD IsDeleted BIT NOT NULL DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_Rooms') AND name = 'UpdatedDate')
    ALTER TABLE PMS_Rooms ADD UpdatedDate DATETIME NULL;
PRINT 'PMS_Rooms columns updated.';
GO

-- Add missing columns to PMS_RoomTypes
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'Description')
    ALTER TABLE PMS_RoomTypes ADD Description NVARCHAR(500) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'ImageUrl')
    ALTER TABLE PMS_RoomTypes ADD ImageUrl NVARCHAR(500) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'Amenities')
    ALTER TABLE PMS_RoomTypes ADD Amenities NVARCHAR(1000) NULL;  -- JSON array: WiFi, AC, ...
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'IsDeleted')
    ALTER TABLE PMS_RoomTypes ADD IsDeleted BIT NOT NULL DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'UpdatedDate')
    ALTER TABLE PMS_RoomTypes ADD UpdatedDate DATETIME NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('PMS_RoomTypes') AND name = 'CreatedDate')
    ALTER TABLE PMS_RoomTypes ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();
PRINT 'PMS_RoomTypes columns updated.';
GO

-- ── TOUR MANAGEMENT ─────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelTours' AND xtype='U')
BEGIN
    CREATE TABLE HotelTours (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        TourCode        NVARCHAR(50) NOT NULL,
        TourName        NVARCHAR(200) NOT NULL,
        TourNameEN      NVARCHAR(200),
        TourType        NVARCHAR(50) DEFAULT 'DAY_TRIP', -- DAY_TRIP/LOOP/TREKKING/CUSTOM/CAR
        DurationDays    INT DEFAULT 1,
        DurationNights  INT DEFAULT 0,
        MaxPerson       INT DEFAULT 10,
        MinPerson       INT DEFAULT 1,
        PricePerPerson  DECIMAL(18,2) DEFAULT 0,
        GroupPrice      DECIMAL(18,2) DEFAULT 0,     -- Fixed price for whole group
        GroupDiscountFrom INT DEFAULT 5,             -- Group discount from N people
        Highlights      NVARCHAR(2000),              -- JSON array
        Itinerary       NVARCHAR(MAX),               -- JSON day-by-day schedule
        Inclusions      NVARCHAR(1000),              -- JSON array: what's included
        Exclusions      NVARCHAR(1000),              -- JSON array: not included
        MeetingPoint    NVARCHAR(500),
        Difficulty      NVARCHAR(20) DEFAULT 'EASY', -- EASY/MODERATE/HARD
        ImageUrl        NVARCHAR(500),
        IsAvailable     BIT DEFAULT 1,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        SortOrder       INT DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelTours UNIQUE (HotelCode, TourCode)
    );
    PRINT 'Table HotelTours created.';
END
GO

-- Tour Guides
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelTourGuides' AND xtype='U')
BEGIN
    CREATE TABLE HotelTourGuides (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Guid        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode   NVARCHAR(50) NOT NULL,
        Name        NVARCHAR(200) NOT NULL,
        Phone       NVARCHAR(20),
        Email       NVARCHAR(200),
        Languages   NVARCHAR(200),        -- JSON: ["vi","en","fr"]
        Speciality  NVARCHAR(200),        -- Loop/Trek/Cultural/Car
        IsFreelance BIT DEFAULT 0,
        DailyRate   DECIMAL(18,2) DEFAULT 0,
        Bio         NVARCHAR(1000),
        ImageUrl    NVARCHAR(500),
        IsActive    BIT DEFAULT 1,
        IsDeleted   BIT NOT NULL DEFAULT 0,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate DATETIME
    );
    PRINT 'Table HotelTourGuides created.';
END
GO

-- Tour Schedules
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelTourSchedules' AND xtype='U')
BEGIN
    CREATE TABLE HotelTourSchedules (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        TourCode    NVARCHAR(50) NOT NULL,
        TourDate    DATE NOT NULL,
        GuideId     INT,
        MaxSlots    INT DEFAULT 10,
        BookedSlots INT DEFAULT 0,
        PriceOverride DECIMAL(18,2),     -- Override tour price for this schedule
        Status      NVARCHAR(20) DEFAULT 'OPEN', -- OPEN/FULL/CANCELLED
        Notes       NVARCHAR(500),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT UQ_TourSchedule UNIQUE (HotelCode, TourCode, TourDate)
    );
    CREATE INDEX IX_TourSchedules_Date ON HotelTourSchedules(HotelCode, TourDate);
    PRINT 'Table HotelTourSchedules created.';
END
GO

-- ── GROUP BOOKING MEMBERS ────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelGroupMembers' AND xtype='U')
BEGIN
    CREATE TABLE HotelGroupMembers (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        BookingId   INT NOT NULL,       -- FK to HotelBookings (GIT type)
        MemberNo    INT DEFAULT 1,       -- Member number in group
        GuestName   NVARCHAR(200) NOT NULL,
        GuestPhone  NVARCHAR(20),
        GuestIdCard NVARCHAR(50),
        Nationality NVARCHAR(100) DEFAULT N'Việt Nam',
        RoomNo      NVARCHAR(20),
        BedCode     NVARCHAR(20),
        PaidAmount  DECIMAL(18,2) DEFAULT 0,   -- What this member paid
        Status      NVARCHAR(20) DEFAULT 'PENDING', -- PENDING/CONFIRMED/CHECKED_IN
        Notes       NVARCHAR(500),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
    CREATE INDEX IX_GroupMembers_Booking ON HotelGroupMembers(BookingId);
    PRINT 'Table HotelGroupMembers created.';
END
GO

-- ── PROPERTY SETTINGS ────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelSettings' AND xtype='U')
BEGIN
    CREATE TABLE HotelSettings (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        SettingKey  NVARCHAR(100) NOT NULL,
        SettingValue NVARCHAR(2000),
        Description NVARCHAR(500),
        UpdatedDate DATETIME DEFAULT GETDATE(),
        CONSTRAINT UQ_HotelSettings UNIQUE (HotelCode, SettingKey)
    );
    PRINT 'Table HotelSettings created.';
END
GO

-- Seed default settings for HOMEHG
DELETE FROM HotelSettings WHERE HotelCode = 'HOMEHG';
INSERT INTO HotelSettings (HotelCode, SettingKey, SettingValue, Description) VALUES
('HOMEHG', 'CheckInTime',  '14:00', N'Giờ check-in mặc định'),
('HOMEHG', 'CheckOutTime', '12:00', N'Giờ check-out mặc định'),
('HOMEHG', 'Currency',     'VND',   N'Đơn vị tiền tệ'),
('HOMEHG', 'AutoBlockForecast', 'true', N'Tự động block forecast khi tạo booking'),
('HOMEHG', 'LateCheckoutFee', '100000', N'Phí check-out muộn (VND)'),
('HOMEHG', 'EarlyCheckInFee', '100000', N'Phí check-in sớm (VND)'),
('HOMEHG', 'DefaultDepositVehicle', '500000', N'Tiền cọc xe mặc định (VND)');

-- Seed sample tours for HOMEHG
IF NOT EXISTS (SELECT 1 FROM HotelTours WHERE HotelCode = 'HOMEHG' AND TourCode = 'LOOP_3D2N')
INSERT INTO HotelTours (HotelCode, TourCode, TourName, TourNameEN, TourType, DurationDays, DurationNights,
    MaxPerson, MinPerson, PricePerPerson, GroupPrice, GroupDiscountFrom, Difficulty, IsAvailable, SortOrder,
    Highlights, Inclusions, Exclusions, MeetingPoint)
VALUES
('HOMEHG','LOOP_3D2N', N'Hà Giang Loop 3N2Đ','Ha Giang Loop 3D2N','LOOP',3,2,
 15,2,350000,4500000,10,'HARD',1,1,
 N'["Đèo Mã Pì Lèng","Cao nguyên đá Đồng Văn","Sông Nho Quế","Cột cờ Lũng Cú"]',
 N'["Hướng dẫn viên","Bản đồ GPS Track","Áo mưa","Mũ bảo hiểm"]',
 N'["Xe máy (thuê riêng)","Ăn uống","Chỗ ngủ dọc đường"]',
 N'Sảnh HomeHG, Hà Giang'),
('HOMEHG','LOOP_1D', N'Hà Giang Loop 1 Ngày','Ha Giang 1-Day Loop','DAY_TRIP',1,0,
 10,1,200000,1500000,8,'MODERATE',1,2,
 N'["Đèo Bắc Sum","Làng Văn hóa","Đồng Lâm"]',
 N'["Hướng dẫn viên","Bản đồ"]',
 N'["Xe máy","Ăn uống"]',
 N'Sảnh HomeHG, Hà Giang'),
('HOMEHG','TREK_BAN', N'Trekking Bản Làng','Village Trekking','TREKKING',1,0,
 8,2,300000,2000000,6,'MODERATE',1,3,
 N'["Bản dân tộc Mông","Ruộng bậc thang","Chợ phiên Đồng Văn"]',
 N'["Hướng dẫn viên người địa phương","Ăn trưa tại bản"]',
 N'["Đi lại","Tối"]',
 N'Sảnh HomeHG'),
('HOMEHG','CAR_TOUR', N'Tour Xe Ô Tô Đồng Văn','Dong Van Car Tour','CAR',2,1,
 7,4,600000,3500000,6,'EASY',1,4,
 N'["Cao nguyên đá Đồng Văn","Phố cổ Đồng Văn","Lũng Cú"]',
 N'["Xe 7 chỗ","Tài xế","Hướng dẫn viên","Ăn sáng"]',
 N'["Ăn trưa/tối","Chỗ ngủ"]',
 N'Sảnh HomeHG');

-- Seed a guide
IF NOT EXISTS (SELECT 1 FROM HotelTourGuides WHERE HotelCode = 'HOMEHG' AND Phone = '0912111222')
INSERT INTO HotelTourGuides (HotelCode, Name, Phone, Languages, Speciality, IsFreelance, DailyRate, Bio)
VALUES ('HOMEHG', N'Vàng A Dúa', '0912111222', '["vi","en"]', 'Loop/Trek', 0, 300000,
    N'HDV người Mông, thông thổ đường Hà Giang, kinh nghiệm 8 năm');

PRINT '=== Complete Hotel Module Tables DONE ===';
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME;
GO
