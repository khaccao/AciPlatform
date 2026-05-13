using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Persistence;

public static class HotelSchemaPatcher
{
    public static Task ApplyAsync(HotelDbContext context)
    {
        return context.Database.ExecuteSqlRawAsync("""
IF OBJECT_ID(N'dbo.HotelVehicleRentals', N'U') IS NOT NULL
    AND COL_LENGTH(N'dbo.HotelVehicleRentals', N'PaidAmount') IS NULL
BEGIN
    ALTER TABLE dbo.HotelVehicleRentals
        ADD PaidAmount DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_HotelVehicleRentals_PaidAmount DEFAULT 0;
END

IF OBJECT_ID(N'dbo.HotelElements', N'U') IS NOT NULL
    AND COL_LENGTH(N'dbo.HotelElements', N'Status') IS NULL
BEGIN
    ALTER TABLE dbo.HotelElements
        ADD Status NVARCHAR(20) NOT NULL
            CONSTRAINT DF_HotelElements_Status DEFAULT N'VC';
END
""");
    }
}
