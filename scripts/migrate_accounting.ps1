$srcDir = "c:\Users\nguye\Project\Isoft\isoft\src"
$destDir = "c:\Users\nguye\Expo\AciPlatform"

$keywords = @(
    "*ChartOfAccount*", "*Ledger*", "*AccountBalance*", "*InvoiceDeclaration*", 
    "*GeneralDiary*", "*VoucherReport*", "*TransactionList*", "*ReportTax*"
)

$files = Get-ChildItem -Path $srcDir -Recurse -Include $keywords | Where-Object { $_.Extension -eq ".cs" }

Write-Host "Found $($files.Count) files to migrate."

foreach ($file in $files) {
    $relativePath = $file.FullName.Substring($srcDir.Length + 1)
    $destPath = ""

    if ($relativePath -match "^ManageEmployee\\Controllers") {
        $destPath = Join-Path $destDir "AciPlatform.Api\Controllers\Ledger\$($file.Name)"
    }
    elseif ($relativePath -match "^ManageEmployee\\Services") {
        if ($file.Name -match "^I[A-Z]") {
            $destPath = Join-Path $destDir "AciPlatform.Application\Interfaces\Ledger\$($file.Name)"
        } else {
            $destPath = Join-Path $destDir "AciPlatform.Application\Services\Ledger\$($file.Name)"
        }
    }
    elseif ($relativePath -match "^ManageEmployee\.DataLayer\.Service") {
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
    else {
        # Other paths? Maybe constants, common
        Write-Host "Skipping $relativePath"
        continue
    }

    $destFolder = Split-Path $destPath -Parent
    if (-not (Test-Path $destFolder)) {
        New-Item -ItemType Directory -Path $destFolder | Out-Null
    }

    $content = Get-Content $file.FullName -Raw

    # Replace Namespaces
    $content = $content -replace "namespace ManageEmployee\.Controllers", "namespace AciPlatform.Api.Controllers.Ledger"
    $content = $content -replace "namespace ManageEmployee\.Services", "namespace AciPlatform.Application.Services.Ledger"
    $content = $content -replace "namespace ManageEmployee\.DataLayer\.Service", "namespace AciPlatform.Application.Services.Ledger"
    $content = $content -replace "namespace ManageEmployee\.DataTransferObject", "namespace AciPlatform.Application.DTOs.Ledger"
    $content = $content -replace "namespace ManageEmployee\.Entities", "namespace AciPlatform.Domain.Entities.Ledger"
    
    # Using replacements
    $content = $content -replace "using ManageEmployee\.DataTransferObject", "using AciPlatform.Application.DTOs.Ledger"
    $content = $content -replace "using ManageEmployee\.Entities", "using AciPlatform.Domain.Entities.Ledger"
    $content = $content -replace "using ManageEmployee\.Services", "using AciPlatform.Application.Services.Ledger"
    $content = $content -replace "using ManageEmployee\.DataLayer\.Service", "using AciPlatform.Application.Services.Ledger"

    Set-Content -Path $destPath -Value $content -Encoding UTF8
    Write-Host "Copied to $destPath"
}
Write-Host "Done!"
