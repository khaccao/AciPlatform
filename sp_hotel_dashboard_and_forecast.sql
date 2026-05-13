CREATE OR ALTER PROCEDURE SP_GetRoomStatusDashboard
    @HotelCode NVARCHAR(50),
    @TargetDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tạm bảng các chỉ số
    DECLARE @TotalRooms INT, @InHouseRooms INT, @OosRooms INT, @VacantRooms INT;
    DECLARE @RoomRevenue DECIMAL(18,2), @Adr DECIMAL(18,2), @Occupancy DECIMAL(5,2);

    -- Tổng số phòng và OOS
    SELECT 
        @TotalRooms = COUNT(*),
        @OosRooms = SUM(CASE WHEN Status IN ('OOS', 'OOO') THEN 1 ELSE 0 END)
    FROM HotelElements
    WHERE HotelCode = @HotelCode AND Type = 'ROOM' AND IsActive = 1;

    -- In-House (đang ở) hôm nay
    SELECT @InHouseRooms = COUNT(DISTINCT br.RoomNo)
    FROM HotelBookings b
    JOIN HotelBookingRooms br ON b.Id = br.BookingId
    WHERE b.HotelCode = @HotelCode 
      AND b.Status IN ('CHECKED_IN')
      AND CAST(b.CheckIn AS DATE) <= @TargetDate 
      AND CAST(b.CheckOut AS DATE) > @TargetDate;

    SET @VacantRooms = @TotalRooms - @InHouseRooms - @OosRooms;
    IF @VacantRooms < 0 SET @VacantRooms = 0;

    -- Doanh thu phòng (dự kiến/thực tế) của các phòng in-house đêm nay
    SELECT @RoomRevenue = ISNULL(SUM(br.PricePerNight), 0)
    FROM HotelBookings b
    JOIN HotelBookingRooms br ON b.Id = br.BookingId
    WHERE b.HotelCode = @HotelCode 
      AND b.Status IN ('CHECKED_IN', 'CONFIRMED')
      AND CAST(b.CheckIn AS DATE) <= @TargetDate 
      AND CAST(b.CheckOut AS DATE) > @TargetDate;

    -- ADR & Occupancy
    IF @InHouseRooms > 0
        SET @Adr = @RoomRevenue / @InHouseRooms;
    ELSE
        SET @Adr = 0;

    IF (@TotalRooms - @OosRooms) > 0
        SET @Occupancy = (CAST(@InHouseRooms AS DECIMAL(18,2)) / CAST((@TotalRooms - @OosRooms) AS DECIMAL(18,2))) * 100;
    ELSE
        SET @Occupancy = 0;

    -- Dọn phòng (Housekeeping)
    DECLARE @Vc INT, @Vd INT, @Inspected INT, @Uninspected INT;
    SELECT 
        @Vc = SUM(CASE WHEN Status = 'VC' THEN 1 ELSE 0 END),
        @Vd = SUM(CASE WHEN Status = 'VD' THEN 1 ELSE 0 END),
        @Inspected = 0, -- Tạm thời chưa có logic inspected
        @Uninspected = @TotalRooms
    FROM HotelElements
    WHERE HotelCode = @HotelCode AND Type = 'ROOM' AND IsActive = 1;

    -- Chuyển động (Movement)
    DECLARE @ExpectedArr INT = 0, @ExpectedArrPax INT = 0;
    DECLARE @ExpectedDep INT = 0, @ExpectedDepPax INT = 0;
    DECLARE @StayOver INT = @InHouseRooms, @StayOverPax INT = 0;

    -- Expected Arrivals
    SELECT @ExpectedArr = COUNT(DISTINCT br.RoomNo), @ExpectedArrPax = SUM(b.GroupSize)
    FROM HotelBookings b JOIN HotelBookingRooms br ON b.Id = br.BookingId
    WHERE b.HotelCode = @HotelCode AND b.Status = 'CONFIRMED' AND CAST(b.CheckIn AS DATE) = @TargetDate;

    -- Expected Departures
    SELECT @ExpectedDep = COUNT(DISTINCT br.RoomNo), @ExpectedDepPax = SUM(b.GroupSize)
    FROM HotelBookings b JOIN HotelBookingRooms br ON b.Id = br.BookingId
    WHERE b.HotelCode = @HotelCode AND b.Status = 'CHECKED_IN' AND CAST(b.CheckOut AS DATE) = @TargetDate;

    -- Nguồn khách
    DECLARE @FitCount INT = 0, @GitCount INT = 0, @CompanyCount INT = 0, @OtaCount INT = 0;
    SELECT 
        @FitCount = SUM(CASE WHEN Source = 'DIRECT' AND BookingType = 'FIT' THEN 1 ELSE 0 END),
        @GitCount = SUM(CASE WHEN Source = 'DIRECT' AND BookingType = 'GIT' THEN 1 ELSE 0 END),
        @OtaCount = SUM(CASE WHEN Source LIKE 'OTA%' THEN 1 ELSE 0 END),
        @CompanyCount = SUM(CASE WHEN Source = 'COMPANY' THEN 1 ELSE 0 END)
    FROM HotelBookings
    WHERE HotelCode = @HotelCode AND CAST(CheckIn AS DATE) <= @TargetDate AND CAST(CheckOut AS DATE) > @TargetDate AND Status IN ('CHECKED_IN');

    -- Trả kết quả
    SELECT 
        @RoomRevenue AS Revenue,
        @Adr AS Adr,
        @Occupancy AS Occupancy,
        @InHouseRooms AS InHouseRooms,
        @VacantRooms AS VacantTonight,
        @VacantRooms AS ReadyToCkin,
        @OosRooms AS Oos,
        @TotalRooms AS TotalRooms,
        (@TotalRooms - @OosRooms) AS AvailableToSell,
        @OosRooms AS Ooo,
        @Inspected AS HkInspected,
        @Uninspected AS HkUninspected,
        @Vc AS HkVc,
        @Vd AS HkVd,
        @ExpectedDep AS MovExpectedDepRooms, @ExpectedDepPax AS MovExpectedDepPax,
        0 AS MovActualDepRooms, 0 AS MovActualDepPax,
        @StayOver AS MovStayOverRooms, @StayOverPax AS MovStayOverPax,
        @ExpectedArr AS MovExpectedArrRooms, @ExpectedArrPax AS MovExpectedArrPax,
        0 AS MovExtendedRooms, 0 AS MovExtendedPax,
        0 AS MovWalkInRooms, 0 AS MovWalkInPax,
        0 AS MovSameDayResRooms, 0 AS MovSameDayResPax,
        @FitCount AS MixFit,
        @GitCount AS MixGit,
        @CompanyCount AS MixCompany,
        @OtaCount AS MixOta;
END
GO

CREATE OR ALTER PROCEDURE SP_GetRoomForecast
    @HotelCode NVARCHAR(50),
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Giả sử RoomType được lấy từ HotelElements (Alias hoặc Description)
    -- Lấy danh sách loại phòng và tổng số phòng
    SELECT 
        ISNULL(rt.Ten, e.Description) AS RoomTypeName,
        ISNULL(rt.Ma, e.Description) AS RoomType,
        COUNT(e.Id) AS TotalRooms
    INTO #RoomStats
    FROM HotelElements e
    LEFT JOIN PMS_RoomTypes rt ON e.Description = rt.Ma AND rt.HotelCode = @HotelCode
    WHERE e.HotelCode = @HotelCode AND e.Type = 'ROOM' AND e.IsActive = 1
    GROUP BY ISNULL(rt.Ten, e.Description), ISNULL(rt.Ma, e.Description);

    -- Lấy số lượng đặt phòng (CONFIRMED, CHECKED_IN) theo ngày
    -- Sử dụng CTE hoặc vòng lặp để lấy danh sách ngày
    ;WITH DateList AS (
        SELECT @FromDate AS TargetDate
        UNION ALL
        SELECT DATEADD(DAY, 1, TargetDate)
        FROM DateList
        WHERE TargetDate < @ToDate
    )
    SELECT 
        r.RoomType,
        r.RoomTypeName,
        r.TotalRooms,
        d.TargetDate AS [Date],
        r.TotalRooms - ISNULL((
            SELECT COUNT(DISTINCT br.RoomNo)
            FROM HotelBookings b
            JOIN HotelBookingRooms br ON b.Id = br.BookingId
            JOIN HotelElements he ON he.Name = br.RoomNo AND he.HotelCode = @HotelCode
            WHERE b.HotelCode = @HotelCode 
              AND b.Status IN ('CONFIRMED', 'CHECKED_IN')
              AND ISNULL(he.Description, '') = r.RoomType
              AND CAST(b.CheckIn AS DATE) <= d.TargetDate 
              AND CAST(b.CheckOut AS DATE) > d.TargetDate
        ), 0) AS AvailableCount
    FROM #RoomStats r
    CROSS JOIN DateList d
    ORDER BY r.RoomTypeName, d.TargetDate;

    DROP TABLE #RoomStats;
END
GO
