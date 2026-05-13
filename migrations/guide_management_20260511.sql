-- ================================================================
-- Migration: PMS Tour Guide Management (HR Integration)
-- Applied to: AciPlatform_Hotel database
-- Date: 2026-05-11
-- ================================================================

USE AciPlatform_Hotel;
GO

-- Step 1: Alter HotelTourGuides to add new columns
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'GuideCode')
    ALTER TABLE HotelTourGuides ADD GuideCode NVARCHAR(20) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'HrEmployeeId')
    ALTER TABLE HotelTourGuides ADD HrEmployeeId INT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'IdCard')
    ALTER TABLE HotelTourGuides ADD IdCard NVARCHAR(30) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'Address')
    ALTER TABLE HotelTourGuides ADD Address NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'BirthDate')
    ALTER TABLE HotelTourGuides ADD BirthDate DATE NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'ContractType')
    ALTER TABLE HotelTourGuides ADD ContractType NVARCHAR(20) NOT NULL DEFAULT 'FREELANCE';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('HotelTourGuides') AND name = 'MonthlyBaseSalary')
    ALTER TABLE HotelTourGuides ADD MonthlyBaseSalary DECIMAL(18,2) NOT NULL DEFAULT 0;

-- Step 2: Create PmsTourGuideContracts table
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PmsTourGuideContracts')
BEGIN
    CREATE TABLE PmsTourGuideContracts (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode       NVARCHAR(50)    NOT NULL,
        GuideId         INT             NOT NULL,
        ContractCode    NVARCHAR(30)    NOT NULL,
        ContractType    NVARCHAR(20)    NOT NULL DEFAULT 'FREELANCE',  -- FREELANCE|FULLTIME|PARTTIME
        StartDate       DATE            NOT NULL,
        EndDate         DATE            NULL,
        BasicSalary     DECIMAL(18,2)   NOT NULL DEFAULT 0,
        DailyRate       DECIMAL(18,2)   NOT NULL DEFAULT 0,
        Status          NVARCHAR(20)    NOT NULL DEFAULT 'ACTIVE',  -- ACTIVE|EXPIRED|TERMINATED
        Notes           NVARCHAR(500)   NULL,
        CreatedDate     DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME        NULL,
        CONSTRAINT FK_GuideContract_Guide FOREIGN KEY (GuideId) REFERENCES HotelTourGuides(Id)
    );
    CREATE INDEX IX_PmsTourGuideContracts_Hotel ON PmsTourGuideContracts(HotelCode, GuideId);
    PRINT 'Created PmsTourGuideContracts';
END
ELSE PRINT 'PmsTourGuideContracts already exists';

-- Step 3: Create PmsTourGuideSalaries table
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PmsTourGuideSalaries')
BEGIN
    CREATE TABLE PmsTourGuideSalaries (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        HotelCode       NVARCHAR(50)    NOT NULL,
        GuideId         INT             NOT NULL,
        Month           INT             NOT NULL,  -- 1-12
        Year            INT             NOT NULL,
        TourCount       INT             NOT NULL DEFAULT 0,
        DailyRate       DECIMAL(18,2)   NOT NULL DEFAULT 0,
        TourIncome      DECIMAL(18,2)   NOT NULL DEFAULT 0,
        BasicSalary     DECIMAL(18,2)   NOT NULL DEFAULT 0,
        Bonus           DECIMAL(18,2)   NOT NULL DEFAULT 0,
        Deductions      DECIMAL(18,2)   NOT NULL DEFAULT 0,
        TotalPay        DECIMAL(18,2)   NOT NULL DEFAULT 0,
        Status          NVARCHAR(20)    NOT NULL DEFAULT 'PENDING',  -- PENDING|APPROVED|PAID
        PaidAt          DATETIME        NULL,
        Notes           NVARCHAR(500)   NULL,
        CreatedDate     DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedDate     DATETIME        NULL,
        CONSTRAINT UQ_GuideSalary UNIQUE (HotelCode, GuideId, Month, Year),
        CONSTRAINT FK_GuideSalary_Guide FOREIGN KEY (GuideId) REFERENCES HotelTourGuides(Id)
    );
    CREATE INDEX IX_PmsTourGuideSalaries_Hotel ON PmsTourGuideSalaries(HotelCode, Year, Month);
    PRINT 'Created PmsTourGuideSalaries';
END
ELSE PRINT 'PmsTourGuideSalaries already exists';

-- Step 4: Auto-generate GuideCode for existing guides
UPDATE HotelTourGuides
SET GuideCode = CONCAT('HDV', FORMAT(Id, '000'))
WHERE GuideCode IS NULL;

-- Step 5: Seed test data for HOMEHG
-- Thêm hướng dẫn viên mẫu
IF NOT EXISTS (SELECT 1 FROM HotelTourGuides WHERE HotelCode = 'HOMEHG' AND Name = N'Nguyễn Văn Hùng')
BEGIN
    INSERT INTO HotelTourGuides (HotelCode, GuideCode, Name, Phone, Email, Languages, Speciality,
        IsFreelance, DailyRate, Bio, IsActive, ContractType, MonthlyBaseSalary, CreatedDate)
    VALUES
    ('HOMEHG','HDV001',N'Nguyễn Văn Hùng','0912345001','hung.hdv@homehg.vn',N'Tiếng Việt, Tiếng Anh',N'Loop Tour, Trekking',
        0, 350000, N'HDV dày dạn kinh nghiệm 5 năm tại Hà Giang, chuyên loop tour và cung đường Đồng Văn.', 1, 'FULLTIME', 3500000, GETDATE()),
    ('HOMEHG','HDV002',N'Trần Thị Mai','0912345002','mai.hdv@homehg.vn',N'Tiếng Việt, Tiếng Pháp',N'Cultural, Day Trip',
        0, 300000, N'HDV văn hóa, am hiểu phong tục tập quán Hà Giang, giao tiếp Pháp ngữ tốt.', 1, 'FULLTIME', 3000000, GETDATE()),
    ('HOMEHG','HDV003',N'Vàng A Páo','0912345003',NULL,N'Tiếng Việt, Tiếng H''Mông',N'Trekking, Homestay',
        1, 250000, N'HDV người địa phương, thông thạo ngôn ngữ và văn hóa người H''Mông.', 1, 'FREELANCE', 0, GETDATE()),
    ('HOMEHG','HDV004',N'Lê Minh Tuấn','0912345004','tuan.hdv@homehg.vn',N'Tiếng Việt, Tiếng Anh, Tiếng Trung',N'Car Tour, Loop Tour',
        0, 400000, N'HDV xe ô tô cao cấp, phục vụ đoàn khách nước ngoài.', 1, 'PARTTIME', 0, GETDATE());

    PRINT 'Seeded 4 guides for HOMEHG';
END

-- Step 6: Tạo hợp đồng mẫu cho HDV fulltime
IF NOT EXISTS (SELECT 1 FROM PmsTourGuideContracts WHERE HotelCode = 'HOMEHG')
BEGIN
    INSERT INTO PmsTourGuideContracts (HotelCode, GuideId, ContractCode, ContractType,
        StartDate, BasicSalary, DailyRate, Status, Notes, CreatedDate)
    SELECT
        'HOMEHG', Id,
        CONCAT('HDV-CTR-', FORMAT(Id, '0001')),
        ContractType,
        '2026-01-01',
        MonthlyBaseSalary,
        DailyRate,
        'ACTIVE',
        N'Hợp đồng tự động',
        GETDATE()
    FROM HotelTourGuides
    WHERE HotelCode = 'HOMEHG' AND ContractType != 'FREELANCE';

    PRINT 'Created contracts for fulltime/parttime guides';
END

GO
PRINT 'Migration completed successfully!';
