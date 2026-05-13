import sys
import re

content = open('AciPlatform.Infrastructure/Services/HotelManagement/HotelBookingService.cs', 'r', encoding='utf-8').read()

old_func = """    public async Task<object> GetTodayDashboardAsync(string hotelCode)
    {
        var today = DateTime.Today;
        // Expected or Actual Check-ins today
        var checkIns = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.CheckIn.Date == today && (b.Status == "CONFIRMED" || b.Status == "CHECKED_IN" || b.Status == "CHECKED_OUT")).CountAsync();
        
        // Expected or Actual Check-outs today
        var checkOuts = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.CheckOut.Date == today && (b.Status == "CHECKED_IN" || b.Status == "CHECKED_OUT")).CountAsync();
            
        var inHouse = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_IN").CountAsync();
            
        var todayRevenue = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_OUT" && b.CheckOutActual.HasValue && b.CheckOutActual.Value.Date == today)
            .SumAsync(b => b.TotalAmount);
            
        return new { CheckInsToday = checkIns, CheckOutsToday = checkOuts, InHouse = inHouse, TodayRevenue = (double)todayRevenue };
    }"""

new_func = """    public async Task<object> GetTodayDashboardAsync(string hotelCode)
    {
        var targetDate = DateTime.Today;
        using var cmd = _db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SP_GetRoomStatusDashboard";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        var p1 = cmd.CreateParameter(); p1.ParameterName = "@HotelCode"; p1.Value = hotelCode; cmd.Parameters.Add(p1);
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@TargetDate"; p2.Value = targetDate; cmd.Parameters.Add(p2);

        if (cmd.Connection!.State != System.Data.ConnectionState.Open)
            await cmd.Connection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new
            {
                Revenue = reader["Revenue"], Adr = reader["Adr"], Occupancy = reader["Occupancy"],
                InHouseRooms = reader["InHouseRooms"], VacantTonight = reader["VacantTonight"],
                ReadyToCkin = reader["ReadyToCkin"], Oos = reader["Oos"],
                TotalRooms = reader["TotalRooms"], AvailableToSell = reader["AvailableToSell"],
                Ooo = reader["Ooo"], HkInspected = reader["HkInspected"], HkUninspected = reader["HkUninspected"],
                HkVc = reader["HkVc"], HkVd = reader["HkVd"],
                MovExpectedDepRooms = reader["MovExpectedDepRooms"], MovExpectedDepPax = reader["MovExpectedDepPax"],
                MovActualDepRooms = reader["MovActualDepRooms"], MovActualDepPax = reader["MovActualDepPax"],
                MovStayOverRooms = reader["MovStayOverRooms"], MovStayOverPax = reader["MovStayOverPax"],
                MovExpectedArrRooms = reader["MovExpectedArrRooms"], MovExpectedArrPax = reader["MovExpectedArrPax"],
                MovExtendedRooms = reader["MovExtendedRooms"], MovExtendedPax = reader["MovExtendedPax"],
                MovWalkInRooms = reader["MovWalkInRooms"], MovWalkInPax = reader["MovWalkInPax"],
                MovSameDayResRooms = reader["MovSameDayResRooms"], MovSameDayResPax = reader["MovSameDayResPax"],
                MixFit = reader["MixFit"], MixGit = reader["MixGit"], MixCompany = reader["MixCompany"], MixOta = reader["MixOta"]
            };
        }

        return new { };
    }"""

old_regex = re.escape(old_func)
old_regex = old_regex.replace(r'\ ', r'\s*').replace(r'\n', r'\s*')

content = re.sub(old_regex, new_func, content, flags=re.DOTALL)
open('AciPlatform.Infrastructure/Services/HotelManagement/HotelBookingService.cs', 'w', encoding='utf-8').write(content)
