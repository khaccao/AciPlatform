-- ============================================================
-- KỊCH BẢN: Đồng nhất Company = Hotel trong AciPlatform
-- Nguyên tắc: CompanyCode == HotelCode
-- Khi user gán vào Company → tự động gán vào Hotel cùng Code
-- ============================================================

-- PHẦN 1: Cập nhật AciPlatform DB
-- Thêm cột hotel vào bảng Customers (Company = Hotel)
-- Thêm cột UserFO vào UserCompanies (PMS account mapping)
-- ============================================================

USE AciPlatform;
GO

-- 1.1 Thêm cột IsHotel vào Customers (đánh dấu company này là khách sạn)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Customers') AND name='IsHotel')
BEGIN
    ALTER TABLE Customers ADD IsHotel BIT NOT NULL DEFAULT 0;
    PRINT 'Column IsHotel added to Customers.';
END
GO

-- 1.2 Thêm cột HotelType (HOTEL / RESORT / APARTMENT...)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Customers') AND name='HotelType')
BEGIN
    ALTER TABLE Customers ADD HotelType NVARCHAR(50) NULL;
    PRINT 'Column HotelType added to Customers.';
END
GO

-- 1.3 Thêm PMS connection config vào Customers
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Customers') AND name='PmsConnectionString')
BEGIN
    ALTER TABLE Customers ADD PmsConnectionString NVARCHAR(1000) NULL;
    PRINT 'Column PmsConnectionString added to Customers.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('Customers') AND name='DmsAppId')
BEGIN
    ALTER TABLE Customers ADD DmsAppId NVARCHAR(50) NULL;
    ALTER TABLE Customers ADD DmsAppSecret NVARCHAR(100) NULL;
    PRINT 'Columns DmsAppId, DmsAppSecret added to Customers.';
END
GO

-- 1.4 Thêm UserFO vào UserCompanies (tài khoản PMS của user tại hotel này)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('UserCompanies') AND name='UserFO')
BEGIN
    ALTER TABLE UserCompanies ADD UserFO NVARCHAR(50) NULL;
    PRINT 'Column UserFO added to UserCompanies.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('UserCompanies') AND name='UserBO')
BEGIN
    ALTER TABLE UserCompanies ADD UserBO NVARCHAR(50) NULL;
    ALTER TABLE UserCompanies ADD UserPOS NVARCHAR(50) NULL;
    PRINT 'Columns UserBO, UserPOS added to UserCompanies.';
END
GO

-- ============================================================
-- PHẦN 2: Cập nhật AciPlatform_Hotel DB
-- Đồng bộ HotelProperties từ Customers của AciPlatform
-- ============================================================

USE AciPlatform_Hotel;
GO

-- 2.1 Thêm cột IsLinkedToCompany để tracking
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('HotelProperties') AND name='IsLinkedToAciCompany')
BEGIN
    ALTER TABLE HotelProperties ADD IsLinkedToAciCompany BIT NOT NULL DEFAULT 1;
    PRINT 'Column IsLinkedToAciCompany added to HotelProperties.';
END
GO

-- 2.2 Thêm HotelType
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('HotelProperties') AND name='HotelType')
BEGIN
    ALTER TABLE HotelProperties ADD HotelType NVARCHAR(50) NULL;
    PRINT 'Column HotelType added to HotelProperties.';
END
GO

-- 2.3 Tạo View để join Company+Hotel data
IF EXISTS (SELECT 1 FROM AciPlatform_Hotel.sys.views WHERE name = 'V_HotelWithAciCompany')
    DROP VIEW V_HotelWithAciCompany;
GO

CREATE VIEW V_HotelWithAciCompany AS
    SELECT
        h.Id,
        h.Code,
        h.Name,
        h.ShortName,
        h.Address,
        h.Phone,
        h.Email,
        h.HotelType,
        h.PmsConnectionString,
        h.DmsAppId,
        h.DmsAppSecret,
        h.IsActive,
        h.IsLinkedToAciCompany,
        h.CreatedDate
    FROM HotelProperties h
    WHERE h.IsDeleted = 0;
GO
PRINT 'View V_HotelWithAciCompany created.';

-- ============================================================
-- PHẦN 3: Seed khách sạn mẫu vào AciPlatform.Customers
-- Đồng thời sync vào AciPlatform_Hotel.HotelProperties
-- ============================================================

USE AciPlatform;
GO

-- Seed CITITEL vào Customers nếu chưa có
IF NOT EXISTS (SELECT 1 FROM Customers WHERE Code = 'CITITEL')
BEGIN
    INSERT INTO Customers (Code, Name, Phone, Email, Address, IsDeleted, IsSupplier, IsHotel, HotelType, CreatedDate)
    VALUES ('CITITEL', N'Cititel Hotel Hà Nội', '0241234567', 'info@cititel.vn', N'Hà Nội, Việt Nam', 0, 0, 1, 'HOTEL', GETDATE());
    PRINT 'Seeded CITITEL into Customers.';
END
ELSE
BEGIN
    UPDATE Customers SET IsHotel = 1, HotelType = 'HOTEL' WHERE Code = 'CITITEL';
    PRINT 'Updated CITITEL IsHotel flag.';
END
GO

-- Seed SONAGA vào Customers nếu chưa có
IF NOT EXISTS (SELECT 1 FROM Customers WHERE Code = 'SONAGA')
BEGIN
    INSERT INTO Customers (Code, Name, Phone, Email, Address, IsDeleted, IsSupplier, IsHotel, HotelType, CreatedDate)
    VALUES ('SONAGA', N'Sonaga Hotel TP.HCM', '0281234568', 'info@sonaga.vn', N'TP.HCM, Việt Nam', 0, 0, 1, 'HOTEL', GETDATE());
    PRINT 'Seeded SONAGA into Customers.';
END
ELSE
BEGIN
    UPDATE Customers SET IsHotel = 1, HotelType = 'HOTEL' WHERE Code = 'SONAGA';
    PRINT 'Updated SONAGA IsHotel flag.';
END
GO

-- Sync vào AciPlatform_Hotel.HotelProperties
USE AciPlatform_Hotel;
GO

MERGE HotelProperties AS target
USING (
    SELECT Code, Name, Phone, Email, Address, IsHotel, HotelType
    FROM AciPlatform.dbo.Customers
    WHERE IsHotel = 1 AND IsDeleted = 0
) AS source ON target.Code = source.Code
WHEN MATCHED THEN
    UPDATE SET
        Name        = source.Name,
        Phone       = source.Phone,
        Email       = source.Email,
        Address     = source.Address,
        HotelType   = source.HotelType,
        UpdatedDate = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (Code, Name, Phone, Email, Address, HotelType, IsActive, IsLinkedToAciCompany)
    VALUES (source.Code, source.Name, source.Phone, source.Email, source.Address, source.HotelType, 1, 1);
GO
PRINT 'HotelProperties synced from AciPlatform.Customers.';

-- ============================================================
-- PHẦN 4: Stored Procedure tiện ích
-- ============================================================

USE AciPlatform;
GO

-- SP: Gán user vào Hotel (qua UserCompanies)
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'SP_Hotel_AssignUser')
    DROP PROCEDURE SP_Hotel_AssignUser;
GO

CREATE PROCEDURE SP_Hotel_AssignUser
    @UserId     INT,
    @HotelCode  NVARCHAR(50),
    @UserFO     NVARCHAR(50) = NULL,
    @UserBO     NVARCHAR(50) = NULL,
    @UserPOS    NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra hotel có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM Customers WHERE Code = @HotelCode AND IsHotel = 1)
    BEGIN
        RAISERROR('Hotel/Company code not found or not a hotel.', 16, 1);
        RETURN;
    END

    -- Upsert vào UserCompanies
    IF EXISTS (SELECT 1 FROM UserCompanies WHERE UserId = @UserId AND CompanyCode = @HotelCode)
    BEGIN
        UPDATE UserCompanies
        SET UserFO  = ISNULL(@UserFO, UserFO),
            UserBO  = ISNULL(@UserBO, UserBO),
            UserPOS = ISNULL(@UserPOS, UserPOS)
        WHERE UserId = @UserId AND CompanyCode = @HotelCode;
    END
    ELSE
    BEGIN
        INSERT INTO UserCompanies (UserId, CompanyCode, UserFO, UserBO, UserPOS)
        VALUES (@UserId, @HotelCode, @UserFO, @UserBO, @UserPOS);
    END

    SELECT 'OK' AS Result, @UserId AS UserId, @HotelCode AS HotelCode;
END
GO
PRINT 'Stored Procedure SP_Hotel_AssignUser created.';

-- SP: Lấy danh sách Hotel mà user có quyền truy cập
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'SP_Hotel_GetByUser')
    DROP PROCEDURE SP_Hotel_GetByUser;
GO

CREATE PROCEDURE SP_Hotel_GetByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.Id,
        c.Code          AS HotelCode,
        c.Name          AS HotelName,
        c.HotelType,
        c.Phone,
        c.Email,
        c.Address,
        uc.UserFO,
        uc.UserBO,
        uc.UserPOS,
        c.IsHotel
    FROM UserCompanies uc
    INNER JOIN Customers c ON c.Code = uc.CompanyCode
    WHERE uc.UserId = @UserId
      AND c.IsHotel = 1
      AND c.IsDeleted = 0
    ORDER BY c.Name;
END
GO
PRINT 'Stored Procedure SP_Hotel_GetByUser created.';

USE AciPlatform;
GO
PRINT '=== Hotel-Company unification COMPLETED ===';
PRINT 'AciPlatform.Customers: Added IsHotel, HotelType, PmsConnectionString, DmsAppId, DmsAppSecret';
PRINT 'AciPlatform.UserCompanies: Added UserFO, UserBO, UserPOS';
PRINT 'AciPlatform_Hotel.HotelProperties: Synced from Customers';
PRINT 'SPs created: SP_Hotel_AssignUser, SP_Hotel_GetByUser';
GO
