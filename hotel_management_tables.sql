-- ============================================================
-- HOTEL MANAGEMENT FULL TABLES
-- AciPlatform_Hotel DB - Phase 1 Complete
-- ============================================================
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON;
GO
USE AciPlatform_Hotel;
GO

-- ── 1. GIƯỜNG TRONG PHÒNG TẬP THỂ ───────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelBeds' AND xtype='U')
BEGIN
    CREATE TABLE HotelBeds (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Guid        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode   NVARCHAR(50) NOT NULL,
        RoomNo      NVARCHAR(20) NOT NULL,
        BedCode     NVARCHAR(20) NOT NULL,       -- B01, B02...
        BedName     NVARCHAR(100),               -- Giường 1 Tầng Dưới
        BedType     NVARCHAR(20) NOT NULL DEFAULT 'SINGLE', -- BOTTOM/TOP/SINGLE
        Status      NVARCHAR(20) NOT NULL DEFAULT 'VACANT',
        SortOrder   INT DEFAULT 0,
        IsActive    BIT NOT NULL DEFAULT 1,
        CONSTRAINT UQ_HotelBeds UNIQUE (HotelCode, RoomNo, BedCode)
    );
    PRINT 'Table HotelBeds created.';
END
GO

-- ── 2. HỒ SƠ KHÁCH ────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelGuests' AND xtype='U')
BEGIN
    CREATE TABLE HotelGuests (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        GuestCode       NVARCHAR(50),
        FullName        NVARCHAR(200) NOT NULL,
        Phone           NVARCHAR(20),
        Email           NVARCHAR(200),
        IdCard          NVARCHAR(50),            -- CMND/CCCD/Passport
        IdType          NVARCHAR(20) DEFAULT 'CCCD', -- CCCD/PASSPORT/OTHER
        Nationality     NVARCHAR(100) DEFAULT N'Việt Nam',
        DateOfBirth     DATE,
        Gender          NVARCHAR(10),            -- MALE/FEMALE/OTHER
        Address         NVARCHAR(500),
        -- Preference
        PreferRoomType  NVARCHAR(50),            -- KHEPKIN/TAPTHE
        PreferVehicle   NVARCHAR(50),            -- XE_SO/XE_TAY_GA
        Notes           NVARCHAR(1000),
        -- Stats
        TotalVisits     INT DEFAULT 0,
        TotalSpend      DECIMAL(18,2) DEFAULT 0,
        LastVisitDate   DATE,
        -- Source
        Source          NVARCHAR(50) DEFAULT 'DIRECT', -- DIRECT/ZALO/BOOKING_COM
        IsVIP           BIT DEFAULT 0,
        IsBlacklisted   BIT DEFAULT 0,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelGuests_Phone UNIQUE (HotelCode, Phone)
    );
    CREATE INDEX IX_HotelGuests_IdCard ON HotelGuests(HotelCode, IdCard);
    PRINT 'Table HotelGuests created.';
END
GO

-- ── 3. BOOKING CHÍNH (FIT + GIT) ─────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelBookings' AND xtype='U')
BEGIN
    CREATE TABLE HotelBookings (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        BookingCode     NVARCHAR(50) NOT NULL,   -- BK-20260510-001
        BookingType     NVARCHAR(10) NOT NULL DEFAULT 'FIT', -- FIT / GIT
        -- Khách đại diện
        GuestId         INT,                     -- FK -> HotelGuests (optional)
        GuestName       NVARCHAR(200) NOT NULL,
        GuestPhone      NVARCHAR(20),
        GuestIdCard     NVARCHAR(50),
        Nationality     NVARCHAR(100) DEFAULT N'Việt Nam',
        -- Thời gian
        CheckIn         DATETIME NOT NULL,
        CheckOut        DATETIME NOT NULL,
        NightCount      INT NOT NULL DEFAULT 1,
        -- Tài chính (lưu giá ngay khi booking)
        RoomPrice       DECIMAL(18,2) DEFAULT 0, -- Tổng tiền phòng
        ServicePrice    DECIMAL(18,2) DEFAULT 0, -- Tổng tiền dịch vụ
        VehiclePrice    DECIMAL(18,2) DEFAULT 0, -- Tổng tiền xe
        DiscountAmount  DECIMAL(18,2) DEFAULT 0,
        TotalAmount     DECIMAL(18,2) DEFAULT 0, -- = Room+Service+Vehicle-Discount
        PaidAmount      DECIMAL(18,2) DEFAULT 0,
        DepositAmount   DECIMAL(18,2) DEFAULT 0,
        -- Trạng thái
        Status          NVARCHAR(20) NOT NULL DEFAULT 'CONFIRMED', -- PENDING/CONFIRMED/CHECKED_IN/CHECKED_OUT/CANCELLED/NO_SHOW
        -- Nguồn đặt
        Source          NVARCHAR(50) DEFAULT 'DIRECT', -- DIRECT/ZALO/BOOKING_COM/AIRBNB/PHONE
        -- GIT group
        GroupName       NVARCHAR(200),
        GroupSize       INT DEFAULT 1,
        -- Thông tin thêm
        Notes           NVARCHAR(1000),
        SpecialRequests NVARCHAR(500),
        CreatedBy       INT,                     -- UserId của nhân viên tạo
        -- Timestamps
        CheckInActual   DATETIME,
        CheckOutActual  DATETIME,
        CancelledAt     DATETIME,
        CancelReason    NVARCHAR(500),
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelBookings_Code UNIQUE (HotelCode, BookingCode)
    );
    CREATE INDEX IX_HotelBookings_Hotel ON HotelBookings(HotelCode, Status);
    CREATE INDEX IX_HotelBookings_CheckIn ON HotelBookings(HotelCode, CheckIn, CheckOut);
    PRINT 'Table HotelBookings created.';
END
GO

-- ── 4. PHÒNG / GIƯỜNG TRONG BOOKING ──────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelBookingRooms' AND xtype='U')
BEGIN
    CREATE TABLE HotelBookingRooms (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        BookingId   INT NOT NULL,
        RoomNo      NVARCHAR(20) NOT NULL,
        BedCode     NVARCHAR(20),               -- NULL nếu đặt cả phòng
        RoomType    NVARCHAR(50),               -- KHEPKIN / TAPTHE
        GuestName   NVARCHAR(200),              -- Tên khách ở phòng này (cho GIT)
        CheckIn     DATETIME NOT NULL,
        CheckOut    DATETIME NOT NULL,
        NightCount  INT NOT NULL DEFAULT 1,
        PricePerNight DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalPrice  DECIMAL(18,2) NOT NULL DEFAULT 0,
        Status      NVARCHAR(20) DEFAULT 'BOOKED', -- BOOKED/CHECKED_IN/CHECKED_OUT/CANCELLED
        Notes       NVARCHAR(500),
        CONSTRAINT FK_BookingRoom_Booking FOREIGN KEY (BookingId) REFERENCES HotelBookings(Id)
    );
    CREATE INDEX IX_HotelBookingRooms_Booking ON HotelBookingRooms(BookingId);
    CREATE INDEX IX_HotelBookingRooms_Room ON HotelBookingRooms(HotelCode, RoomNo, CheckIn, CheckOut);
    PRINT 'Table HotelBookingRooms created.';
END
GO

-- ── 5. DỊCH VỤ TRONG BOOKING ─────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelBookingServices' AND xtype='U')
BEGIN
    CREATE TABLE HotelBookingServices (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode       NVARCHAR(50) NOT NULL,
        BookingId       INT NOT NULL,
        ServiceCode     NVARCHAR(50) NOT NULL,  -- FK -> HotelServices.ServiceCode
        ServiceName     NVARCHAR(200),          -- Denormalized cho lưu tại thời điểm booking
        Category        NVARCHAR(50),
        Quantity        DECIMAL(10,2) NOT NULL DEFAULT 1,
        Unit            NVARCHAR(50),
        UnitPrice       DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalPrice      DECIMAL(18,2) NOT NULL DEFAULT 0,
        ServiceDate     DATE,
        Notes           NVARCHAR(500),
        CONSTRAINT FK_BookingService_Booking FOREIGN KEY (BookingId) REFERENCES HotelBookings(Id)
    );
    CREATE INDEX IX_HotelBookingServices_Booking ON HotelBookingServices(BookingId);
    PRINT 'Table HotelBookingServices created.';
END
GO

-- ── 6. ROOM FORECAST (Block Calendar) ───────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelRoomForecast' AND xtype='U')
BEGIN
    CREATE TABLE HotelRoomForecast (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        RoomNo      NVARCHAR(20) NOT NULL,
        BedCode     NVARCHAR(20),               -- NULL nếu block cả phòng
        ForecastDate DATE NOT NULL,
        BookingId   INT,                         -- Booking đang chiếm slot này
        BlockType   NVARCHAR(20) NOT NULL DEFAULT 'BOOKING', -- BOOKING/MAINTENANCE/HOLD/BLOCK
        BlockNote   NVARCHAR(200),
        CreatedBy   INT,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT UQ_RoomForecast UNIQUE (HotelCode, RoomNo, BedCode, ForecastDate)
    );
    CREATE INDEX IX_HotelRoomForecast_Date ON HotelRoomForecast(HotelCode, ForecastDate);
    CREATE INDEX IX_HotelRoomForecast_Room ON HotelRoomForecast(HotelCode, RoomNo, ForecastDate);
    PRINT 'Table HotelRoomForecast created.';
END
GO

-- ── 7. KHO XE ────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelVehicles' AND xtype='U')
BEGIN
    CREATE TABLE HotelVehicles (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        VehicleCode     NVARCHAR(50) NOT NULL,
        BienSo          NVARCHAR(20),            -- Biển số xe
        TenXe           NVARCHAR(200) NOT NULL,
        VehicleType     NVARCHAR(50) NOT NULL,   -- MOTORBIKE_MANUAL/MOTORBIKE_AUTO/BICYCLE/CAR
        ServiceCode     NVARCHAR(50),            -- Link to HotelServices
        Brand           NVARCHAR(100),           -- Honda, Yamaha...
        Model           NVARCHAR(100),           -- Wave, Sirius, Exciter...
        Color           NVARCHAR(50),
        YearMade        INT,
        PricePerDay     DECIMAL(18,2) NOT NULL DEFAULT 0,
        DepositRequired DECIMAL(18,2) DEFAULT 0,
        FuelLevel       INT DEFAULT 100,         -- 0-100%
        Condition       NVARCHAR(20) DEFAULT 'GOOD', -- GOOD/FAIR/POOR
        Status          NVARCHAR(20) NOT NULL DEFAULT 'AVAILABLE', -- AVAILABLE/RENTED/MAINTENANCE/UNAVAILABLE
        ImageUrl        NVARCHAR(500),
        Notes           NVARCHAR(500),
        IsActive        BIT NOT NULL DEFAULT 1,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelVehicles UNIQUE (HotelCode, VehicleCode)
    );
    PRINT 'Table HotelVehicles created.';
END
GO

-- ── 8. GIAO DỊCH THUÊ XE ─────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelVehicleRentals' AND xtype='U')
BEGIN
    CREATE TABLE HotelVehicleRentals (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        RentalCode      NVARCHAR(50) NOT NULL,  -- VR-20260510-001
        BookingId       INT,                    -- Optional link to Booking
        VehicleCode     NVARCHAR(50) NOT NULL,
        -- Khách
        GuestName       NVARCHAR(200) NOT NULL,
        GuestPhone      NVARCHAR(20),
        GuestIdCard     NVARCHAR(50),
        -- Thời gian
        RentFrom        DATETIME NOT NULL,
        RentTo          DATETIME NOT NULL,
        ActualReturnDate DATETIME,
        TotalDays       DECIMAL(5,1) NOT NULL DEFAULT 1,
        -- Tài chính
        PricePerDay     DECIMAL(18,2) NOT NULL DEFAULT 0,
        TotalAmount     DECIMAL(18,2) NOT NULL DEFAULT 0,
        DepositAmount   DECIMAL(18,2) DEFAULT 0,
        DepositReturned DECIMAL(18,2) DEFAULT 0,
        DamageFee       DECIMAL(18,2) DEFAULT 0,
        -- Tình trạng
        FuelLevelOut    INT DEFAULT 100,         -- Xăng lúc cho thuê
        FuelLevelIn     INT,                     -- Xăng lúc trả
        ConditionOut    NVARCHAR(20) DEFAULT 'GOOD',
        ConditionIn     NVARCHAR(20),
        DamageNotes     NVARCHAR(500),
        -- Trạng thái
        Status          NVARCHAR(20) NOT NULL DEFAULT 'ACTIVE', -- ACTIVE/RETURNED/OVERDUE/CANCELLED
        Notes           NVARCHAR(500),
        CreatedBy       INT,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelVehicleRentals_Code UNIQUE (HotelCode, RentalCode)
    );
    CREATE INDEX IX_HotelVehicleRentals_Vehicle ON HotelVehicleRentals(HotelCode, VehicleCode, Status);
    PRINT 'Table HotelVehicleRentals created.';
END
GO

-- ── 9. HÓA ĐƠN ───────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelInvoices' AND xtype='U')
BEGIN
    CREATE TABLE HotelInvoices (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        InvoiceCode     NVARCHAR(50) NOT NULL,  -- INV-20260510-001
        BookingId       INT,
        GuestName       NVARCHAR(200),
        RoomAmount      DECIMAL(18,2) DEFAULT 0,
        ServiceAmount   DECIMAL(18,2) DEFAULT 0,
        VehicleAmount   DECIMAL(18,2) DEFAULT 0,
        DiscountAmount  DECIMAL(18,2) DEFAULT 0,
        TotalAmount     DECIMAL(18,2) DEFAULT 0,
        PaidAmount      DECIMAL(18,2) DEFAULT 0,
        PaymentMethod   NVARCHAR(50) DEFAULT 'CASH', -- CASH/TRANSFER/CARD
        Status          NVARCHAR(20) DEFAULT 'UNPAID', -- UNPAID/PARTIAL/PAID
        Notes           NVARCHAR(500),
        IssuedBy        INT,
        IssuedDate      DATETIME DEFAULT GETDATE(),
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT UQ_HotelInvoices_Code UNIQUE (HotelCode, InvoiceCode)
    );
    PRINT 'Table HotelInvoices created.';
END
GO

-- ── 10. GIÁ THEO MÙA ─────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelSeasonalPricing' AND xtype='U')
BEGIN
    CREATE TABLE HotelSeasonalPricing (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode   NVARCHAR(50) NOT NULL,
        SeasonName  NVARCHAR(100) NOT NULL,     -- Mùa Tam Giác Mạch
        SeasonType  NVARCHAR(20) NOT NULL,      -- HIGH/MID/LOW
        StartDate   DATE NOT NULL,
        EndDate     DATE NOT NULL,
        PriceMultiplier DECIMAL(5,2) DEFAULT 1.0, -- 1.5 = giá x1.5
        IsActive    BIT NOT NULL DEFAULT 1,
        Notes       NVARCHAR(500),
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Table HotelSeasonalPricing created.';
END
GO

-- ── SEED BEDS CHO HOMEHG ─────────────────────────────────────
DELETE FROM HotelBeds WHERE HotelCode = 'HOMEHG';

-- Phòng 201 - Tập thể lớn: 12 giường tầng (6 bộ x 2 tầng)
DECLARE @i INT = 1;
WHILE @i <= 6
BEGIN
    INSERT INTO HotelBeds (HotelCode, RoomNo, BedCode, BedName, BedType, SortOrder)
    VALUES
    ('HOMEHG', '201', 'B' + FORMAT(@i*2-1, '00'), N'Giường ' + CAST(@i*2-1 AS NVARCHAR) + N' (Tầng Dưới)', 'BOTTOM', @i*2-1),
    ('HOMEHG', '201', 'B' + FORMAT(@i*2, '00'),   N'Giường ' + CAST(@i*2 AS NVARCHAR) + N' (Tầng Trên)',  'TOP',    @i*2);
    SET @i = @i + 1;
END

-- Phòng 301-305: Mỗi phòng 4 giường
DECLARE @room NVARCHAR(5);
DECLARE @rooms TABLE (rno NVARCHAR(5));
INSERT INTO @rooms VALUES ('301'),('302'),('303'),('304'),('305');
DECLARE room_cur CURSOR FOR SELECT rno FROM @rooms;
OPEN room_cur; FETCH NEXT FROM room_cur INTO @room;
WHILE @@FETCH_STATUS = 0
BEGIN
    INSERT INTO HotelBeds (HotelCode, RoomNo, BedCode, BedName, BedType, SortOrder) VALUES
    ('HOMEHG', @room, 'B01', N'Giường 1 (Tầng Dưới)', 'BOTTOM', 1),
    ('HOMEHG', @room, 'B02', N'Giường 2 (Tầng Trên)',  'TOP',    2),
    ('HOMEHG', @room, 'B03', N'Giường 3 (Tầng Dưới)', 'BOTTOM', 3),
    ('HOMEHG', @room, 'B04', N'Giường 4 (Tầng Trên)',  'TOP',    4);
    FETCH NEXT FROM room_cur INTO @room;
END
CLOSE room_cur; DEALLOCATE room_cur;

PRINT 'Beds seeded: 12 beds in 201, 4 beds each in 301-305 = 32 total dorm beds';

-- Seed Seasonal Pricing
DELETE FROM HotelSeasonalPricing WHERE HotelCode = 'HOMEHG';
INSERT INTO HotelSeasonalPricing (HotelCode, SeasonName, SeasonType, StartDate, EndDate, PriceMultiplier)
VALUES
('HOMEHG', N'Mùa Tam Giác Mạch', 'HIGH', '2026-09-01', '2026-11-30', 1.8),
('HOMEHG', N'Tết Nguyên Đán',    'HIGH', '2026-01-25', '2026-02-05', 2.0),
('HOMEHG', N'Mùa Xuân',          'MID',  '2026-03-01', '2026-05-31', 1.2),
('HOMEHG', N'Mùa Mưa',           'LOW',  '2026-06-01', '2026-08-31', 0.8);

PRINT '=== Hotel Management Tables COMPLETE ===';
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME;
GO
