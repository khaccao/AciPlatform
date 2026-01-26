param(
    [Parameter(Position=0)]
    [ValidateSet("setup", "start", "stop", "logs", "clean", "build", "test", "migrate", "help")]
    [string]$Command = "help"
)

function Show-Banner {
    Write-Host "==================================================" -ForegroundColor Cyan
    Write-Host "  AciPlatform API - Development Helper" -ForegroundColor Cyan
    Write-Host "  Domain: sit-backend-nguyenbinh.info.vn" -ForegroundColor Cyan
    Write-Host "==================================================" -ForegroundColor Cyan
}

function Show-Help {
    Write-Host ""
    Write-Host "Usage: .\dev.ps1 [command]" -ForegroundColor Green
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor Green
    Write-Host "  setup     Setup local environment (first time)"
    Write-Host "  start     Start database container"
    Write-Host "  stop      Stop all containers"
    Write-Host "  logs      View Docker logs"
    Write-Host "  clean     Remove all containers and volumes"
    Write-Host "  build     Build .NET application"
    Write-Host "  test      Run tests"
    Write-Host "  migrate   Run database migrations"
    Write-Host "  help      Show this help"
    Write-Host ""
}

function Test-Requirements {
    Write-Host ""
    Write-Host "Checking requirements..." -ForegroundColor Yellow

    $missing = @()

    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        $missing += "Docker"
    } else {
        # Test if Docker daemon is running
        try {
            docker version | Out-Null
        } catch {
            Write-Host ""
            Write-Host "❌ Docker is installed but NOT RUNNING!" -ForegroundColor Red
            Write-Host ""
            Write-Host "FIX: Start Docker Desktop" -ForegroundColor Yellow
            Write-Host "  1. Open Windows Start Menu" -ForegroundColor Cyan
            Write-Host "  2. Search for 'Docker Desktop'" -ForegroundColor Cyan
            Write-Host "  3. Click to launch Docker Desktop" -ForegroundColor Cyan
            Write-Host "  4. Wait for it to fully start (icon in taskbar)" -ForegroundColor Cyan
            Write-Host "  5. Run this script again" -ForegroundColor Cyan
            Write-Host ""
            exit 1
        }
    }

    if (-not (Get-Command docker-compose -ErrorAction SilentlyContinue)) {
        $missing += "Docker Compose"
    }

    if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
        $missing += "Git"
    }

    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        $missing += ".NET SDK"
    }

    if ($missing.Count -gt 0) {
        Write-Host ""
        Write-Host "Missing requirements:" -ForegroundColor Red
        $missing | ForEach-Object { Write-Host " - $_" -ForegroundColor Red }
        exit 1
    }

    Write-Host "✅ All requirements met." -ForegroundColor Green
}

function Setup-Environment {
    Show-Banner
    Test-Requirements

    Write-Host ""
    Write-Host "Setting up environment..." -ForegroundColor Yellow

    Write-Host ""
    Write-Host "✅ Development environment configured!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Database connection details:" -ForegroundColor Green
    Write-Host "  Server: 14.225.212.145,1433" -ForegroundColor Cyan
    Write-Host "  Database: AciPlatform" -ForegroundColor Cyan
    Write-Host "  User: dev_user1" -ForegroundColor Cyan
    Write-Host "  Password: Dev@123456" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Green
    Write-Host "  1. Run: .\dev.ps1 start" -ForegroundColor Cyan
    Write-Host "  2. This will build and run the API" -ForegroundColor Cyan
    Write-Host "  3. Open: http://localhost:5000/swagger" -ForegroundColor Cyan
}

function Start-Services {
    Write-Host ""
    Write-Host "Starting .NET API..." -ForegroundColor Cyan
    Write-Host ""
    Write-Host "API will be available at: http://localhost:5000" -ForegroundColor Green
    Write-Host "Swagger docs at: http://localhost:5000/swagger" -ForegroundColor Green
    Write-Host ""
    Write-Host "Database: 14.225.212.145:1433 (dev_user1/Dev@123456)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
    Write-Host ""
    
    Set-Location "AciPlatform.Api"
    dotnet watch run --urls "http://localhost:5000"
    Set-Location ".."
}

function Stop-Services {
    Write-Host "Stopping services..." -ForegroundColor Yellow
    docker-compose -f docker-compose.dev.yml down
}

function Show-Logs {
    param($ServiceName = "")

    if ($ServiceName) {
        docker-compose -f docker-compose.dev.yml logs -f $ServiceName
    } else {
        docker-compose -f docker-compose.dev.yml logs -f
    }
}

function Clean-Environment {
    Write-Host "Cleaning environment..." -ForegroundColor Yellow
    $confirm = Read-Host "This will delete all containers and volumes. Continue? (y/n)"
    if ($confirm -ne "y") {
        Write-Host "Aborted." -ForegroundColor Red
        return
    }
    docker-compose -f docker-compose.dev.yml down -v
}

function Build-Application {
    Write-Host "Building application..." -ForegroundColor Cyan
    Set-Location AciPlatform.Api
    dotnet build
    Set-Location ..
}

function Run-Tests {
    Write-Host "Running tests..." -ForegroundColor Cyan
    dotnet test
}

function Run-Migrations {
    Write-Host "Running database migrations..." -ForegroundColor Cyan
    Set-Location AciPlatform.Api
    dotnet ef database update
    Set-Location ..
}

# MAIN
Show-Banner

switch ($Command) {
    "setup"   { Setup-Environment }
    "start"   { Start-Services }
    "stop"    { Stop-Services }
    "logs"    { Show-Logs $args[0] }
    "clean"   { Clean-Environment }
    "build"   { Build-Application }
    "test"    { Run-Tests }
    "migrate" { Run-Migrations }
    "help"    { Show-Help }
    default {
        Write-Host "Unknown command: $Command" -ForegroundColor Red
        Show-Help
        exit 1
    }
}
