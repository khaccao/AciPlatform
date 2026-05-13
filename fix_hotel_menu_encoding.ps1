param()
Add-Type -AssemblyName "System.Data"

$connStr = "Server=103.200.22.167,1433;Database=AciPlatform;User Id=cao_admin;Password=Cao@Admin123!;TrustServerCertificate=True;"

$menus = @(
    @{ Code = "hotel";           Name = "Quản Lý Khách Sạn"; NameEN = "Hotel Management" },
    @{ Code = "hotel/dashboard"; Name = "Dashboard Hôm Nay";  NameEN = "Hotel Dashboard" },
    @{ Code = "hotel/room-map";  Name = "Sơ Đồ Phòng";        NameEN = "Room Map" },
    @{ Code = "hotel/bookings";  Name = "Đặt Phòng";           NameEN = "Bookings" },
    @{ Code = "hotel/vehicles";  Name = "Cho Thuê Xe";         NameEN = "Vehicle Rental" },
    @{ Code = "hotel/tours";     Name = "Quản Lý Tour";        NameEN = "Tours" },
    @{ Code = "hotel/guests";    Name = "Hồ Sơ Khách";        NameEN = "Guests" },
    @{ Code = "hotel/reports";   Name = "Báo Cáo";             NameEN = "Reports" }
)

$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()
Write-Host "Connected to SQL Server"

foreach ($m in $menus) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "UPDATE Menus SET Name = @name, NameEN = @nameEN WHERE Code = @code"
    $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter("@name",   [System.Data.SqlDbType]::NVarChar, 500))).Value = $m.Name
    $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter("@nameEN", [System.Data.SqlDbType]::NVarChar, 500))).Value = $m.NameEN
    $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter("@code",   [System.Data.SqlDbType]::NVarChar, 200))).Value = $m.Code
    $rows = $cmd.ExecuteNonQuery()
    Write-Host "Updated: $($m.Code) -> $($m.Name) [$rows row(s)]"
}

$conn.Close()
Write-Host "Done!"
