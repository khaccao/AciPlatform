SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
USE AciPlatform;
GO
IF DB_ID(N'AciPlatform_Hotel') IS NOT NULL
    AND OBJECT_ID(N'AciPlatform_Hotel.dbo.HotelVehicleRentals', N'U') IS NOT NULL
    AND COL_LENGTH(N'AciPlatform_Hotel.dbo.HotelVehicleRentals', N'PaidAmount') IS NULL
BEGIN
    ALTER TABLE AciPlatform_Hotel.dbo.HotelVehicleRentals
        ADD PaidAmount DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_HotelVehicleRentals_PaidAmount DEFAULT 0;
END
GO
IF DB_ID(N'AciPlatform_Hotel') IS NOT NULL
    AND OBJECT_ID(N'AciPlatform_Hotel.dbo.HotelElements', N'U') IS NOT NULL
    AND COL_LENGTH(N'AciPlatform_Hotel.dbo.HotelElements', N'Status') IS NULL
BEGIN
    ALTER TABLE AciPlatform_Hotel.dbo.HotelElements
        ADD Status NVARCHAR(20) NOT NULL
            CONSTRAINT DF_HotelElements_Status DEFAULT N'VC';
END
GO
IF NOT EXISTS (SELECT 1 FROM Customers WHERE Code = 'HOMEHG')
BEGIN
    INSERT INTO Customers (Code, Name, Phone, Email, Address, IsHotel, HotelType, IsDeleted, CreatedDate)
    VALUES ('HOMEHG', N'Home HG - Nha Nghi Ha Giang', '', '', N'Ha Giang', 1, 'HOTEL', 0, GETDATE());
END
ELSE
BEGIN
    UPDATE Customers
    SET Name = COALESCE(NULLIF(Name, ''), N'Home HG - Nha Nghi Ha Giang'),
        IsHotel = 1,
        HotelType = 'HOTEL',
        IsDeleted = 0,
        UpdatedDate = GETDATE()
    WHERE Code = 'HOMEHG';
END
GO
INSERT INTO UserCompanies (UserId, CompanyCode)
SELECT u.Id, 'HOMEHG'
FROM Users u
WHERE u.Username = 'admin'
  AND NOT EXISTS (
      SELECT 1 FROM UserCompanies uc
      WHERE uc.UserId = u.Id AND uc.CompanyCode = 'HOMEHG'
  );
GO
UPDATE Customers SET IsHotel=1, HotelType='HOTEL' WHERE Code IN ('CITITEL','SONAGA','HOMEHG');
UPDATE Customers SET IsHotel=0 WHERE IsHotel IS NULL;
GO
SELECT Code, Name, IsHotel, HotelType FROM Customers ORDER BY Code;
GO
