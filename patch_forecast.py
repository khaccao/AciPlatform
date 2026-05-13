import sys
import re

content = open('AciPlatform.Infrastructure/Services/HotelManagement/HotelRoomService.cs', 'r', encoding='utf-8').read()

old_func = """    public async Task<List<object>> GetRoomForecastAsync(string hotelCode, DateTime fromDate, DateTime toDate)
    {
        var from = DateOnly.FromDateTime(fromDate);
        var to = DateOnly.FromDateTime(toDate);
        var rooms = await _db.PmsRooms.Where(r => r.HotelCode == hotelCode && r.IsActive)
            .OrderBy(r => r.Floor).ThenBy(r => r.So).ToListAsync();
        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.ForecastDate >= from && f.ForecastDate <= to).ToListAsync();
        var bookings = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status != "CANCELLED"
                && b.CheckIn.Date <= toDate && b.CheckOut.Date >= fromDate)
            .Include(b => b.Rooms).ToListAsync();
        var days = (to.DayNumber - from.DayNumber) + 1;

        return rooms.Select(r => (object)new
        {
            RoomNo = r.So,
            RoomType = r.Ma,
            Floor = r.Floor,
            Blocks = Enumerable.Range(0, days).Select(d =>
            {
                var date = from.AddDays(d);
                var bk = bookings.FirstOrDefault(b =>
                    b.Rooms.Any(br => br.RoomNo == r.So && br.BedCode == null
                        && DateOnly.FromDateTime(br.CheckIn) <= date
                        && DateOnly.FromDateTime(br.CheckOut) > date));
                var fc = forecasts.FirstOrDefault(f => f.RoomNo == r.So && f.BedCode == null && f.ForecastDate == date);
                return new { Date = date, IsBlocked = bk != null || fc != null,
                    BlockType = bk != null ? "BOOKING" : fc?.BlockType, BookingCode = bk?.BookingCode, GuestName = bk?.GuestName };
            })
        }).ToList();
    }"""

new_func = """    public async Task<List<object>> GetRoomForecastAsync(string hotelCode, DateTime fromDate, DateTime toDate)
    {
        var result = new List<object>();
        using var cmd = _db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SP_GetRoomForecast";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        var p1 = cmd.CreateParameter(); p1.ParameterName = "@HotelCode"; p1.Value = hotelCode; cmd.Parameters.Add(p1);
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@FromDate"; p2.Value = fromDate; cmd.Parameters.Add(p2);
        var p3 = cmd.CreateParameter(); p3.ParameterName = "@ToDate"; p3.Value = toDate; cmd.Parameters.Add(p3);

        if (cmd.Connection!.State != System.Data.ConnectionState.Open)
            await cmd.Connection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        var rows = new List<dynamic>();
        while (await reader.ReadAsync())
        {
            rows.Add(new
            {
                RoomType = reader["RoomType"]?.ToString(),
                RoomTypeName = reader["RoomTypeName"]?.ToString(),
                TotalRooms = Convert.ToInt32(reader["TotalRooms"]),
                Date = Convert.ToDateTime(reader["Date"]),
                AvailableCount = Convert.ToInt32(reader["AvailableCount"])
            });
        }

        var types = rows.Select(r => new { r.RoomType, r.RoomTypeName, r.TotalRooms }).Distinct().ToList();
        foreach (var type in types)
        {
            var dates = rows.Where(r => r.RoomType == type.RoomType)
                .Select(r => new { Date = r.Date, AvailableCount = r.AvailableCount })
                .OrderBy(r => r.Date).ToList();
            
            result.Add(new
            {
                RoomType = type.RoomType,
                RoomTypeName = type.RoomTypeName,
                TotalRooms = type.TotalRooms,
                Dates = dates
            });
        }

        return result;
    }"""

old_regex = re.escape(old_func)
old_regex = old_regex.replace(r'\ ', r'\s*').replace(r'\n', r'\s*')

content = re.sub(old_regex, new_func, content, flags=re.DOTALL)
open('AciPlatform.Infrastructure/Services/HotelManagement/HotelRoomService.cs', 'w', encoding='utf-8').write(content)
