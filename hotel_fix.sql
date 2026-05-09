SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO
USE AciPlatform;
GO
UPDATE Customers SET IsHotel=1, HotelType='HOTEL' WHERE Code IN ('CITITEL','SONAGA');
UPDATE Customers SET IsHotel=0 WHERE IsHotel IS NULL;
GO
SELECT Code, Name, IsHotel, HotelType FROM Customers ORDER BY Code;
GO
