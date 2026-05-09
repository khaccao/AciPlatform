-- ============================================================
-- SEED DATA: Khách sạn HOMEHG - Hà Giang
-- 3 tầng, 5 phòng khép kín + 6 phòng tập thể
-- ============================================================

-- BƯỚC 1: Thêm HOMEHG vào AciPlatform.Customers
-- ============================================================
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
USE AciPlatform;
GO

IF NOT EXISTS (SELECT 1 FROM Customers WHERE Code = 'HOMEHG')
BEGIN
    INSERT INTO Customers (
        Code, Name, Phone, Email, Address,
        IsDeleted, IsSupplier, IsHotel, HotelType, CreatedDate
    ) VALUES (
        'HOMEHG',
        N'Home HG - Nhà Nghỉ Hà Giang',
        '0912345678',
        'homehg@gmail.com',
        N'Hà Giang, Việt Nam',
        0, 0, 1, 'HOSTEL', GETDATE()
    );
    PRINT 'Seeded HOMEHG into Customers.';
END
ELSE
BEGIN
    UPDATE Customers
    SET IsHotel = 1, HotelType = 'HOSTEL', Name = N'Home HG - Nhà Nghỉ Hà Giang'
    WHERE Code = 'HOMEHG';
    PRINT 'Updated HOMEHG in Customers.';
END
GO

-- BƯỚC 2: Thêm vào AciPlatform_Hotel
-- ============================================================
USE AciPlatform_Hotel;
GO

-- Upsert HotelProperties
IF NOT EXISTS (SELECT 1 FROM HotelProperties WHERE Code = 'HOMEHG')
    INSERT INTO HotelProperties (Code, Name, ShortName, Address, Phone, Email, HotelType, IsActive, IsLinkedToAciCompany)
    VALUES ('HOMEHG', N'Home HG - Nhà Nghỉ Hà Giang', 'HomeHG', N'Hà Giang, Việt Nam', '0912345678', 'homehg@gmail.com', 'HOSTEL', 1, 1);
ELSE
    UPDATE HotelProperties SET HotelType = 'HOSTEL', IsActive = 1 WHERE Code = 'HOMEHG';

DECLARE @HotelGuid UNIQUEIDENTIFIER = (SELECT Guid FROM HotelProperties WHERE Code = 'HOMEHG');

PRINT 'Hotel HOMEHG ready. Guid = ' + CAST(@HotelGuid AS NVARCHAR(50));

-- ============================================================
-- BƯỚC 3: Tạo cấu trúc tầng (Areas)
-- ============================================================

-- Xóa cũ nếu có (để idempotent)
DELETE FROM HotelAreas WHERE HotelCode = 'HOMEHG';

-- Tầng 1
INSERT INTO HotelAreas (HotelCode, HotelGuid, AreaCode, AreaName, AreaType, Color)
VALUES ('HOMEHG', @HotelGuid, 'F1', N'Tầng 1', 'FLOOR', '#4A90D9');

DECLARE @Floor1Id INT = SCOPE_IDENTITY();
DECLARE @Floor1Guid UNIQUEIDENTIFIER = (SELECT Guid FROM HotelAreas WHERE Id = @Floor1Id);

-- Tầng 2
INSERT INTO HotelAreas (HotelCode, HotelGuid, AreaCode, AreaName, AreaType, Color)
VALUES ('HOMEHG', @HotelGuid, 'F2', N'Tầng 2', 'FLOOR', '#7B68EE');

DECLARE @Floor2Id INT = SCOPE_IDENTITY();
DECLARE @Floor2Guid UNIQUEIDENTIFIER = (SELECT Guid FROM HotelAreas WHERE Id = @Floor2Id);

-- Tầng 3
INSERT INTO HotelAreas (HotelCode, HotelGuid, AreaCode, AreaName, AreaType, Color)
VALUES ('HOMEHG', @HotelGuid, 'F3', N'Tầng 3', 'FLOOR', '#50C878');

DECLARE @Floor3Id INT = SCOPE_IDENTITY();
DECLARE @Floor3Guid UNIQUEIDENTIFIER = (SELECT Guid FROM HotelAreas WHERE Id = @Floor3Id);

PRINT 'Floors created: F1, F2, F3';

-- ============================================================
-- BƯỚC 4: Seed Phòng vào PMS_Rooms
-- ============================================================

DELETE FROM PMS_Rooms WHERE HotelCode = 'HOMEHG';

-- ── TẦNG 1: 5 phòng khép kín ─────────────────────────────
INSERT INTO PMS_Rooms (HotelCode, So, Ma, Ten, Floor, KhuVucCode, SachBan, CleanDirty, Inspected, TinhTrang, Status, IsActive)
VALUES
('HOMEHG', '101', 'KHEPKIN', N'Phòng Khép Kín', '1', 'F1', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '102', 'KHEPKIN', N'Phòng Khép Kín', '1', 'F1', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '103', 'KHEPKIN', N'Phòng Khép Kín', '1', 'F1', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '104', 'KHEPKIN', N'Phòng Khép Kín', '1', 'F1', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '105', 'KHEPKIN', N'Phòng Khép Kín', '1', 'F1', 1, 1, 0, 0, 'VACANT', 1);

-- ── TẦNG 2: 1 phòng tập thể lớn ─────────────────────────
INSERT INTO PMS_Rooms (HotelCode, So, Ma, Ten, Floor, KhuVucCode, SachBan, CleanDirty, Inspected, TinhTrang, Status, IsActive)
VALUES
('HOMEHG', '201', 'TAPTHE', N'Phòng Tập Thể (Lớn)', '2', 'F2', 1, 1, 0, 0, 'VACANT', 1);

-- ── TẦNG 3: 5 phòng tập thể ─────────────────────────────
INSERT INTO PMS_Rooms (HotelCode, So, Ma, Ten, Floor, KhuVucCode, SachBan, CleanDirty, Inspected, TinhTrang, Status, IsActive)
VALUES
('HOMEHG', '301', 'TAPTHE', N'Phòng Tập Thể', '3', 'F3', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '302', 'TAPTHE', N'Phòng Tập Thể', '3', 'F3', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '303', 'TAPTHE', N'Phòng Tập Thể', '3', 'F3', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '304', 'TAPTHE', N'Phòng Tập Thể', '3', 'F3', 1, 1, 0, 0, 'VACANT', 1),
('HOMEHG', '305', 'TAPTHE', N'Phòng Tập Thể', '3', 'F3', 1, 1, 0, 0, 'VACANT', 1);

PRINT 'Rooms seeded: 5 kh?p kín (101-105), 1 t?p th? l?n (201), 5 t?p th? (301-305)';

-- ============================================================
-- BƯỚC 5: Seed Room Types
-- ============================================================

DELETE FROM PMS_RoomTypes WHERE HotelCode = 'HOMEHG';

INSERT INTO PMS_RoomTypes (HotelCode, Ma, Ten, DonGia, MaxPerson, SoLuong, FlagType, IsActive)
VALUES
('HOMEHG', 'KHEPKIN',  N'Phòng Khép Kín (Private)', 250000, 2, 5, 1, 1),
('HOMEHG', 'TAPTHE',   N'Phòng Tập Thể (Dormitory) - Giường', 100000, 1, 6, 2, 1),
('HOMEHG', 'TAPTHE_L', N'Phòng Tập Thể Lớn (Group Room)', 80000, 20, 1, 2, 1);

PRINT 'Room types seeded.';

-- ============================================================
-- BƯỚC 6: Tạo bảng Services (chung chung, sẽ tách sau)
-- ============================================================

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HotelServices' AND xtype='U')
BEGIN
    CREATE TABLE HotelServices (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Guid            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        HotelCode       NVARCHAR(50) NOT NULL,
        ServiceCode     NVARCHAR(50) NOT NULL,
        ServiceName     NVARCHAR(200) NOT NULL,
        ServiceNameEN   NVARCHAR(200),
        Category        NVARCHAR(50) NOT NULL,   -- VEHICLE / TOUR / FOOD / OTHER
        SubCategory     NVARCHAR(50),
        Description     NVARCHAR(1000),
        Unit            NVARCHAR(50),            -- ngày, chuyến, người, cái
        UnitPrice       DECIMAL(18,2) NOT NULL DEFAULT 0,
        Currency        NVARCHAR(10) NOT NULL DEFAULT 'VND',
        TyLeSC          DECIMAL(5,2) DEFAULT 0,
        TyLeVAT         DECIMAL(5,2) DEFAULT 0,
        MaxQuantity     INT,                     -- SL tối đa (VD: xe tối đa thuê được)
        IsAvailable     BIT NOT NULL DEFAULT 1,
        ImageUrl        NVARCHAR(500),
        SortOrder       INT DEFAULT 0,
        IsDeleted       BIT NOT NULL DEFAULT 0,
        CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME,
        CONSTRAINT UQ_HotelServices UNIQUE (HotelCode, ServiceCode)
    );
    CREATE INDEX IX_HotelServices_Hotel ON HotelServices(HotelCode, Category);
    PRINT 'Table HotelServices created.';
END
GO

-- ============================================================
-- BƯỚC 7: Seed dịch vụ mẫu cho HOMEHG
-- ============================================================

DELETE FROM HotelServices WHERE HotelCode = 'HOMEHG';

INSERT INTO HotelServices (HotelCode, ServiceCode, ServiceName, ServiceNameEN, Category, SubCategory, Unit, UnitPrice, Description, SortOrder)
VALUES
-- Phương tiện
('HOMEHG', 'XE_SO',     N'Thuê Xe Số',           'Manual Motorbike Rental',  'VEHICLE', 'MOTORBIKE', N'ngày', 120000, N'Xe số phù hợp cho đường Hà Giang, bình xăng đầy', 1),
('HOMEHG', 'XE_TAY_GA', N'Thuê Xe Tay Ga',        'Scooter Rental',           'VEHICLE', 'MOTORBIKE', N'ngày', 150000, N'Xe tay ga phù hợp cho người mới lái', 2),
('HOMEHG', 'XE_WIN',    N'Thuê Xe Win/Zumer',     'Semi-Auto Motorbike',      'VEHICLE', 'MOTORBIKE', N'ngày', 130000, N'Xe bán tự động, phổ biến leo núi', 3),
('HOMEHG', 'XE_DAP',    N'Thuê Xe Đạp',           'Bicycle Rental',           'VEHICLE', 'BICYCLE',   N'ngày',  50000, N'Xe đạp khám phá nội thị Hà Giang', 4),
('HOMEHG', 'XE_OTO',    N'Thuê Ô tô có tài',      'Car with Driver',          'VEHICLE', 'CAR',       N'ngày', 1500000, N'Xe 7 chỗ có tài xế thông thổ đường Hà Giang', 5),
-- Tour
('HOMEHG', 'TOUR_LOOP', N'Tour Hà Giang Loop',    'Ha Giang Loop Tour',       'TOUR', 'LOOP',    N'người', 350000, N'Trekking qua Đồng Văn - Mèo Vạc - Mã Pì Lèng 3 ngày 2 đêm', 10),
('HOMEHG', 'TOUR_1D',   N'Tour 1 Ngày',           '1-Day Tour',               'TOUR', 'DAY_TRIP', N'người', 200000, N'Khám phá cao nguyên đá Đồng Văn trong ngày', 11),
('HOMEHG', 'TOUR_CUST', N'Tour Theo Yêu Cầu',     'Customized Tour',          'TOUR', 'CUSTOM',   N'đoàn',  0,      N'Thiết kế tour riêng theo yêu cầu của đoàn', 12),
-- Đồ dùng / Tiện ích
('HOMEHG', 'AO_MUA',    N'Thuê Áo Mưa',           'Raincoat Rental',          'OTHER', 'GEAR',    N'cái',   20000, N'Áo mưa phù hợp đi xe máy', 20),
('HOMEHG', 'MU_BAO',    N'Thuê Mũ Bảo Hiểm',      'Helmet Rental',            'OTHER', 'GEAR',    N'cái',   20000, N'Mũ bảo hiểm full-face', 21),
('HOMEHG', 'TUI_NGU',   N'Thuê Túi Ngủ',          'Sleeping Bag Rental',      'OTHER', 'GEAR',    N'cái',   50000, N'Túi ngủ cho đêm lạnh vùng cao', 22),
('HOMEHG', 'GIAT_DO',   N'Dịch Vụ Giặt Đồ',       'Laundry Service',          'OTHER', 'LAUNDRY', N'kg',    30000, N'Giặt sấy theo cân', 23),
-- F&B
('HOMEHG', 'BUA_SANG',  N'Bữa Sáng',              'Breakfast',                'FOOD', 'MEAL',    N'người', 50000, N'Bữa sáng cơm hoặc bánh mì', 30),
('HOMEHG', 'COM_TRUA',  N'Bữa Trưa',              'Lunch',                    'FOOD', 'MEAL',    N'người', 80000, N'Cơm trưa với đặc sản Hà Giang', 31),
('HOMEHG', 'COM_TOI',   N'Bữa Tối',               'Dinner',                   'FOOD', 'MEAL',    N'người', 80000, N'Cơm tối gia đình', 32);

PRINT 'Services seeded: 5 vehicle + 3 tour + 4 gear/other + 3 food = 15 services';
GO

-- Verify
SELECT 'ROOMS' AS DataType, HotelCode, So AS RoomNo, Ma AS TypeCode, Ten AS RoomName, Floor, Status
FROM PMS_Rooms WHERE HotelCode = 'HOMEHG' ORDER BY Floor, So;

SELECT 'SERVICES' AS DataType, ServiceCode, ServiceName, Category, UnitPrice, Unit
FROM HotelServices WHERE HotelCode = 'HOMEHG' ORDER BY Category, SortOrder;
GO

PRINT '=== HOMEHG Setup COMPLETED ===';
PRINT '11 rooms: 5 private (101-105) + 1 large dorm (201) + 5 dorm (301-305)';
PRINT '3 Room Types: KHEPKIN, TAPTHE, TAPTHE_L';
PRINT '15 Services: Vehicle / Tour / Gear / Food';
GO
