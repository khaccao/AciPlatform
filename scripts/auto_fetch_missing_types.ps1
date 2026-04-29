$srcDir = "c:\Users\nguye\Project\Isoft\isoft\src"
$destDir = "c:\Users\nguye\Expo\AciPlatform"
$missingTypesFile = "c:\Users\nguye\Expo\AciPlatform\missing_types.txt"

$missingTypes = Get-Content $missingTypesFile | Where-Object { $_.Trim() -ne "" }

foreach ($type in $missingTypes) {
    # Remove generic type parameters like <> from the name
    $cleanType = $type -replace "<.*>", ""
    
    # Skip generic namespaces that would match too much
    if ($cleanType -in @("Models", "ViewModels", "Extensions", "Infrastructure", "Dal", "Hubs", "Handlers", "Errors", "Common", "Entities")) {
        continue
    }

    Write-Host "Searching for $cleanType..."
    $files = Get-ChildItem -Path $srcDir -Recurse -Filter "$cleanType*.cs"
    
    foreach ($file in $files) {
        $relativePath = $file.FullName.Substring($srcDir.Length + 1)
        $destPath = ""

        # Map to Clean Architecture Layers
        if ($relativePath -match "^ManageEmployee\\Controllers") {
            $destPath = Join-Path $destDir "AciPlatform.Api\Controllers\Ledger\$($file.Name)"
        }
        elseif ($relativePath -match "^ManageEmployee\\Services" -or $relativePath -match "^ManageEmployee\.DataLayer\.Service") {
            if ($file.Name -match "^I[A-Z]") {
                $destPath = Join-Path $destDir "AciPlatform.Application\Interfaces\Ledger\$($file.Name)"
            } else {
                $destPath = Join-Path $destDir "AciPlatform.Application\Services\Ledger\$($file.Name)"
            }
        }
        elseif ($relativePath -match "^ManageEmployee\.DataTransferObject") {
            $destPath = Join-Path $destDir "AciPlatform.Application\DTOs\Ledger\$($file.Name)"
        }
        elseif ($relativePath -match "^ManageEmployee\.Entities") {
            $destPath = Join-Path $destDir "AciPlatform.Domain\Entities\Ledger\$($file.Name)"
        }
        elseif ($relativePath -match "^Common") {
             $destPath = Join-Path $destDir "AciPlatform.Application\Common\$($file.Name)"
        }
        else {
            continue # If it's not in a recognized folder, skip it
        }

        $destFolder = Split-Path $destPath -Parent
        if (-not (Test-Path $destFolder)) {
            New-Item -ItemType Directory -Path $destFolder -Force | Out-Null
        }

        # Don't overwrite if it already exists, unless we want to refresh
        if (-not (Test-Path $destPath)) {
            $content = Get-Content $file.FullName -Raw

            # Namespace Replacements
            $content = $content -replace "ManageEmployee\.Entities\.BaseEntities", "AciPlatform.Domain.Entities.BaseEntities"
            $content = $content -replace "ManageEmployee\.Entities\.ProcedureEntities", "AciPlatform.Domain.Entities.ProcedureEntities"
            $content = $content -replace "ManageEmployee\.Entities\.Constants", "AciPlatform.Domain.Entities.Ledger"
            $content = $content -replace "ManageEmployee\.Entities\.Enumerations", "AciPlatform.Domain.Entities.Ledger"
            $content = $content -replace "ManageEmployee\.Entities", "AciPlatform.Domain.Entities.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.PagingRequest", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.PagingResultModels", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.Reports", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.V2", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.V3", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.LedgerModels", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject\.LedgerWarehouseModels", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataTransferObject", "AciPlatform.Application.DTOs.Ledger"
            $content = $content -replace "ManageEmployee\.DataLayer\.Service", "AciPlatform.Application.Services.Ledger"
            $content = $content -replace "ManageEmployee\.Services", "AciPlatform.Application.Services.Ledger"
            $content = $content -replace "ManageEmployee\.Helpers", "AciPlatform.Application.Helpers"
            $content = $content -replace "ManageEmployee\.Dal\.DbContexts", "AciPlatform.Infrastructure.Persistence"
            
            # Auto replace DbContext
            $content = $content -replace "\bApplicationDbContext\b", "IApplicationDbContext"
            if ($content -notmatch "using AciPlatform\.Application\.Interfaces;") {
                $content = "using AciPlatform.Application.Interfaces;`r`n" + $content
            }
            # Ledger Collision
            $content = $content -replace "\bDomain\.Entities\.Ledger\.Ledger\b", "Domain.Entities.Ledger.LedgerEntry"
            $content = $content -replace "\bpublic class Ledger\b", "public class LedgerEntry"
            $content = $content -replace "public Ledger\(", "public LedgerEntry("
            $content = $content -replace "\bDbSet<Ledger>\b", "DbSet<LedgerEntry>"
            
            Set-Content -Path $destPath -Value $content -Encoding UTF8
            Write-Host "Copied and transformed: $destPath"
        }
    }
}
