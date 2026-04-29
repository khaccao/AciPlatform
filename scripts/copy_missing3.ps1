$srcDir = "c:\Users\nguye\Project\Isoft\isoft\src"
$destDir = "c:\Users\nguye\Expo\AciPlatform"

$types = @(
    "CarModels", "Errors", "Invoices", "BillReportModels", "InvoiceModel", "DeskFloors", 
    "Customers", "ConvertProductEntities", "InOutEntities", "LookupValues", "OrderProduceProducts",
    "MenuModels", "Menus", "TaxRates", "CustomActionResult", "AssetsType", "PagingRequestModel",
    "PagingResultLedger", "ProcedurePagingRequestModel", "PagingResult", "EditOrderRequestModel", 
    "SoChiTietViewModel", "CarDelivery", "VoucherInforItem", "PlanningProduceProductGetDetailModel",
    "ProcedureForCreatePlanningProduct", "PlanningProduceProductListGetterModel", "LedgerDetail",
    "CarGetterModel", "PlanningProduceProductPagingModel", "PositionBillDetail", "RequestFilterDateModel",
    "PlanningProduceProductDetailModel", "LedgerReportModel", "LedgerReportParamDetail", "ProcedureCheckButton",
    "PaymentProposal", "PaymentProposalPagingModel", "ProduceProductStatusTab", "ProcedureStatusModelResponse",
    "HSoKhaiThue", "CKyDTu", "PagingRequestFilterDateModel", "GoodForCustomeViewModel", "PaymentProposalDetailModel",
    "LedgerReportViewModel", "WinInvoiceConfig", "VoucherAccount", "UpdateMenuTypeForGoodModel", "OrderProduceProduct",
    "PlanningProduceProductDetail", "InventoryProductStockViewModel", "InventoryStockResponse", "PaymentProposalDetail",
    "AppSettings", "GoodsQuotaStep", "MenuCheckRole", "GoodsQuotaRecipe", "LedgerReportParam", "DangKyChungTuGhiSo",
    "TotalAmountDKCTGS", "SumSoChiTietViewModel", "TaxRateV2Service", "AccountPayService", "BillDetailService",
    "InvoiceCreator", "BillExporter", "BillService", "BillForSaleReporter", "BillHistoryCollectionService",
    "BillReporter", "BillTrackingForNotificationService", "BillTrackingService", "DeskFloorService",
    "ChartOfAccountValidator", "ChartOfAccountCaculatorQueue", "GoodSynchronizer", "AesEncryptionHelper",
    "VitaxOneClient", "GoodSetter", "GoodsDetailService", "GoodsService", "GoodsQuotaRecipeService",
    "GoodsPromotionService", "OrderProduceProductService", "ProcedurePlanningProduceProductHelper",
    "LookupValueService", "GoodsQuotaService", "GoodsQuotaStepService", "ProcedureService", "MenuService",
    "TaxRateService", "CustomerTaxInformationService"
)

foreach ($type in $types) {
    # Search for exactly TypeName.cs or TypeName*.cs
    $files = Get-ChildItem -Path $srcDir -Recurse -Filter "$type*.cs"
    
    foreach ($file in $files) {
        $relativePath = $file.FullName.Substring($srcDir.Length + 1)
        $destPath = ""

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
            continue
        }

        $destFolder = Split-Path $destPath -Parent
        if (-not (Test-Path $destFolder)) {
            New-Item -ItemType Directory -Path $destFolder | Out-Null
        }

        $content = Get-Content $file.FullName -Raw

        # Fix Namespaces
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

        Set-Content -Path $destPath -Value $content -Encoding UTF8
        Write-Host "Copied $destPath"
    }
}
