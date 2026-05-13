param()
Add-Type -AssemblyName 'System.Data'
$connStr = 'Server=103.200.22.167,1433;Database=AciPlatform_Hotel;User Id=cao_admin;Password=Cao@Admin123!;TrustServerCertificate=True;'
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()
Write-Host "Connected"

$checkCmd = $conn.CreateCommand()
$checkCmd.CommandText = "SELECT COUNT(*) FROM HotelTourGuides WHERE HotelCode='HOMEHG' AND GuideCode IS NOT NULL"
$count = [int]$checkCmd.ExecuteScalar()
Write-Host "Existing guides: $count"

if ($count -lt 4) {
    $guides = @(
        @{ Code='HDV001'; Name='Nguyễn Văn Hùng'; Phone='0912345001'; Lang='Tiếng Việt, Tiếng Anh'; Spec='Loop Tour, Trekking'; DR=350000; Salary=3500000; CT='FULLTIME'; Bio='HDV dày dạn kinh nghiệm 5 năm tại Hà Giang' },
        @{ Code='HDV002'; Name='Trần Thị Mai';     Phone='0912345002'; Lang='Tiếng Việt, Tiếng Pháp'; Spec='Cultural, Day Trip'; DR=300000; Salary=3000000; CT='FULLTIME'; Bio='HDV văn hóa, am hiểu phong tục tập quán Hà Giang' },
        @{ Code='HDV003'; Name='Vàng A Páo';        Phone='0912345003'; Lang="Tiếng Việt, Tiếng H'Mông"; Spec='Trekking, Homestay'; DR=250000; Salary=0; CT='FREELANCE'; Bio="HDV địa phương người H'Mông" },
        @{ Code='HDV004'; Name='Lê Minh Tuấn';     Phone='0912345004'; Lang='Tiếng Việt, Tiếng Anh, Tiếng Trung'; Spec='Car Tour, Loop Tour'; DR=400000; Salary=0; CT='PARTTIME'; Bio='HDV xe ô tô cao cấp, phục vụ đoàn khách quốc tế' }
    )
    
    foreach ($g in $guides) {
        $checkDup = $conn.CreateCommand()
        $checkDup.CommandText = "SELECT COUNT(*) FROM HotelTourGuides WHERE HotelCode='HOMEHG' AND GuideCode=@gc"
        $checkDup.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@gc', [System.Data.SqlDbType]::NVarChar, 20))).Value = $g.Code | Out-Null
        $dup = [int]$checkDup.ExecuteScalar()
        if ($dup -gt 0) { Write-Host "Skip (exists): $($g.Name)"; continue }

        $cmd = $conn.CreateCommand()
        $cmd.CommandText = 'INSERT INTO HotelTourGuides (HotelCode,GuideCode,Name,Phone,Languages,Speciality,IsFreelance,DailyRate,Bio,IsActive,ContractType,MonthlyBaseSalary,CreatedDate) VALUES (@hc,@gc,@n,@ph,@lang,@spec,@fl,@dr,@bio,1,@ct,@ms,GETDATE())'
        $isFreelance = if ($g.CT -eq 'FREELANCE') { $true } else { $false }
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@hc',  [System.Data.SqlDbType]::NVarChar,  50))).Value = 'HOMEHG'
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@gc',  [System.Data.SqlDbType]::NVarChar,  20))).Value = $g.Code
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@n',   [System.Data.SqlDbType]::NVarChar, 200))).Value = $g.Name
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@ph',  [System.Data.SqlDbType]::NVarChar,  20))).Value = $g.Phone
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@lang',[System.Data.SqlDbType]::NVarChar, 200))).Value = $g.Lang
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@spec',[System.Data.SqlDbType]::NVarChar, 200))).Value = $g.Spec
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@fl',  [System.Data.SqlDbType]::Bit))).Value = $isFreelance
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@dr',  [System.Data.SqlDbType]::Decimal))).Value = [decimal]$g.DR
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@bio', [System.Data.SqlDbType]::NVarChar,1000))).Value = $g.Bio
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@ct',  [System.Data.SqlDbType]::NVarChar,  20))).Value = $g.CT
        $cmd.Parameters.Add((New-Object System.Data.SqlClient.SqlParameter('@ms',  [System.Data.SqlDbType]::Decimal))).Value = [decimal]$g.Salary
        $cmd.ExecuteNonQuery() | Out-Null
        Write-Host "Inserted: $($g.Name)"
    }
} else {
    Write-Host "Guides already seeded."
}

$conn.Close()
Write-Host "All done!"
