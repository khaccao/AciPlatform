using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.DTOs;

// Warehouse
public class WarehouseSetterModel
{
    public int Id { get; set; }
    public int? BranchId { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? ManagerName { get; set; }
    public bool IsSyncChartOfAccount { get; set; }
    public List<int>? ShelveIds { get; set; }
}

public class WarehousePaging
{
    public int Id { get; set; }
    public int? BranchId { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? ManagerName { get; set; }
    public bool IsSyncChartOfAccount { get; set; }
    public string? Shevles { get; set; }
}

public class WarehouseFloorPaging
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public string? Positions { get; set; }
}

public class WarehouseFloorSetterModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public List<int>? PositionIds { get; set; }
}

public class WareHouseFloorGetAllModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int WareHouseShelveId { get; set; }
}

public class WarehouseShelvesPaging
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public string? Floors { get; set; }
}

public class WarehouseShelvesSetterModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Note { get; set; }
    public int OrderHorizontal { get; set; }
    public int OrderVertical { get; set; }
    public List<int>? FloorIds { get; set; }
}

public class WareHouseShelvesGetAllModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int WareHouseId { get; set; }
}

public class WareHousePositionGetAllModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int WareHouseFloorId { get; set; }
}

// Good Warehouse
public class FilterParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchText { get; set; }
}

public class SearchViewModel : FilterParams
{
    public string? GoodType { get; set; }
    public string? Account { get; set; }
    public string? Detail1 { get; set; }
    public string? PriceCode { get; set; }
    public string? MenuType { get; set; }
    public int Status { get; set; }
    public string? Warehouse { get; set; }
    public string? GoodCode { get; set; }
}

public class GoodWarehousesViewModel
{
    public int Id { get; set; }
    public string? MenuType { get; set; }
    public string? Account { get; set; }
    public string? AccountName { get; set; }
    public string? Warehouse { get; set; }
    public string? WarehouseName { get; set; }
    public string? Detail1 { get; set; }
    public string? Detail2 { get; set; }
    public string? DetailName1 { get; set; }
    public string? DetailName2 { get; set; }
    public string? GoodsType { get; set; }
    public string? Image1 { get; set; }
    public double Quantity { get; set; }
    public double QuantityInput { get; set; }
    public int Status { get; set; }
    public string? PriceList { get; set; }
    public int Order { get; set; }
    public string? OrginalVoucherNumber { get; set; }
    public int? LedgerId { get; set; }
    public string? QrCode { get; set; }
    public string? GoodCode { get; set; }
    public string? GoodName { get; set; }
    public string? Note { get; set; }
    public DateTime? DateExpiration { get; set; }
    public DateTime? DateManufacture { get; set; }
    public bool IsPrinted { get; set; }
    public double SalePrice { get; set; }
    public List<string>? Positions { get; set; }
}

// Export
public class GoodWarehouseExportRequestModel : FilterParams
{
    public DateTime? Fromdt { get; set; }
    public DateTime? Todt { get; set; }
}

public class GoodWarehouseExportsViewModel
{
    public int Id { get; set; }
    public string? Warehouse { get; set; }
    public string? WarehouseName { get; set; }
    public double Quantity { get; set; }
    public DateTime? DateExpiration { get; set; }
    public int Order { get; set; }
    public string? OrginalVoucherNumber { get; set; }
    public string? QrCode { get; set; }
    public string? GoodCode { get; set; }
    public string? GoodName { get; set; }
}

// Ledger Warehouse
public class LedgerWarehouseCreate
{
    public int GoodsId { get; set; }
    public double Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class LedgerWarehousesRequestPaging : FilterParams
{
    // Add properties as needed
}

// Base Results
public class PagingResult<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public List<T> Data { get; set; } = new List<T>();
}
