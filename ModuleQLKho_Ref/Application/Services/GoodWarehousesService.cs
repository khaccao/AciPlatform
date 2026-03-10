using AutoMapper;
using Common.Constants;
using ManageEmployee.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ManageEmployee.Dal.DbContexts;
using ManageEmployee.Services.Interfaces.ChartOfAccounts;
using ManageEmployee.Services.Interfaces.Goods;
using ManageEmployee.DataTransferObject.WarehouseModel;
using ManageEmployee.Entities;
using ManageEmployee.Entities.LedgerEntities;
using ManageEmployee.Entities.GoodsEntities;
using ManageEmployee.Entities.ChartOfAccountEntities;
using ManageEmployee.DataTransferObject.BillModels;
using ManageEmployee.DataTransferObject.GoodsModels;
using ManageEmployee.DataTransferObject.SearchModels;
using ManageEmployee.DataTransferObject.PagingResultModels;
using ManageEmployee.Entities.BillEntities;

namespace ManageEmployee.Services;
public class GoodWarehousesService : IGoodWarehousesService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
    private readonly IChartOfAccountService _chartOfAccountService;
    public GoodWarehousesService(ApplicationDbContext context, IMapper mapper, IHubContext<BroadcastHub, IHubClient> hubContext, IChartOfAccountService chartOfAccountService)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
        _chartOfAccountService = chartOfAccountService;
    }

    public async Task<PagingResult<GoodWarehousesViewModel>> GetAll(SearchViewModel param)
    {
        try
        {
            if (param.PageSize <= 0)
                param.PageSize = 20;

            if (param.Page < 0)
                param.Page = 1;
            var results = (from p in _context.GoodWarehouses
                           where p.Quantity > 0
                           && (string.IsNullOrEmpty(param.GoodType) || p.GoodsType == param.GoodType)
                           && (string.IsNullOrEmpty(param.Account) || p.Account == param.Account)
                           && (string.IsNullOrEmpty(param.Detail1) || p.Detail1 == param.Detail1)
                           && (string.IsNullOrEmpty(param.PriceCode) || p.PriceList == param.PriceCode)
                           && (string.IsNullOrEmpty(param.MenuType) || p.MenuType == param.MenuType)
                           && ((string.IsNullOrEmpty(param.SearchText) || (!string.IsNullOrEmpty(p.Detail2) ? p.DetailName2 : (p.DetailName1 ?? p.AccountName)).Contains(param.SearchText))
                           || p.Detail2.Contains(param.SearchText))
                           && p.Status == param.Status
                           orderby p.IsPrinted
                           select new GoodWarehousesViewModel()
                           {
                               Id = p.Id,
                               MenuType = p.MenuType,
                               Account = p.Account,
                               AccountName = p.AccountName,
                               Warehouse = p.Warehouse,
                               WarehouseName = p.WarehouseName,
                               Detail1 = p.Detail1,
                               Detail2 = p.Detail2,
                               DetailName1 = p.DetailName1,
                               DetailName2 = p.DetailName2,
                               GoodsType = p.GoodsType,
                               Image1 = p.Image1,
                               Quantity = p.Quantity,
                               Status = p.Status,
                               PriceList = p.PriceList,
                               Order = p.Order,
                               OrginalVoucherNumber = p.OrginalVoucherNumber,
                               LedgerId=p.LedgerId,
                               QrCode = (!String.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account)) + " " + p.Order + "-" + p.Id,
                               GoodCode = !String.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account),
                               GoodName = !String.IsNullOrEmpty(p.DetailName2) ? p.DetailName2 : (p.DetailName1 ?? p.AccountName),
                               Note = p.Note,
                               DateExpiration = p.DateExpiration,
                               DateManufacture = p.DateManufacture,
                               IsPrinted = p.IsPrinted,
                               QuantityInput = p.QuantityInput
                           });
            if (!string.IsNullOrEmpty(param.Warehouse))
            {
                results = results.Where(x => x.Warehouse == param.Warehouse);
            }

            if (!string.IsNullOrEmpty(param.GoodCode))
            {
                results = results.Where(x => x.Detail1 == param.GoodCode || x.Detail2 == param.GoodCode);
            }
            List<GoodWarehousesViewModel> datas;
            if (param.Page == 0)
                datas = await results.ToListAsync();
            else
            {
                datas = await results.Skip((param.Page - 1) * param.PageSize).Take(param.PageSize).ToListAsync();
                var goodWarehouseIds = datas.Select(x => x.Id).ToList();
                var goodWarehousePositions = await _context.GoodWarehousesPositions.Where(x => goodWarehouseIds.Contains(x.GoodWarehousesId)).ToListAsync();
                var warehouses = await _context.Warehouses.Where(x => !x.IsDelete).ToListAsync();
                var shevels = await _context.WareHouseShelves.ToListAsync();
                var floors = await _context.WareHouseFloors.ToListAsync();
                var positions = await _context.WareHousePositions.ToListAsync();

                foreach (var data in datas)
                {
                    var goods = await _context.Goods.FirstOrDefaultAsync(x => (x.Detail1 == data.Detail1 || string.IsNullOrEmpty(data.Detail1))
                                                && (x.Detail2 == data.Detail2 || string.IsNullOrEmpty(data.Detail1))
                                                && x.Account == data.Account
                                                && x.PriceList == "BGC"
                                                && (x.Warehouse == data.Warehouse || string.IsNullOrEmpty(data.Warehouse)));
                    data.SalePrice = goods?.SalePrice ?? 0;
                    var goodWarehouseDetails = goodWarehousePositions.Where(x => x.GoodWarehousesId == data.Id);
                    data.Positions = new List<string>();
                    foreach (var goodWarehouseDetail in goodWarehouseDetails)
                    {
                        var warehouse = warehouses.Find(X => X.Code == goodWarehouseDetail.Warehouse);
                        var shevel = shevels.Find(X => X.Id == goodWarehouseDetail.WareHouseShelvesId);
                        var floor = floors.Find(X => X.Id == goodWarehouseDetail.WareHouseFloorId);
                        var position = positions.Find(X => X.Id == goodWarehouseDetail.WareHousePositionId);
                        data.Positions.Add("Số lượng " + goodWarehouseDetail.Quantity.ToString() + " " + warehouse?.Name + ", " + shevel?.Name + ", " + floor?.Name + ", " + position?.Name);
                    }

                }
            }


            return new PagingResult<GoodWarehousesViewModel>()
            {
                CurrentPage = param.Page,
                PageSize = param.PageSize,
                TotalItems = results.Count(),
                Data = datas
            };

        }
        catch
        {
            return new PagingResult<GoodWarehousesViewModel>()
            {
                CurrentPage = param.Page,
                PageSize = param.PageSize,
                TotalItems = 0,
                Data = new List<GoodWarehousesViewModel>()
            };
        }
    }

    public async Task<string> SyncChartOfAccount(int year)
    {
        var listAccount = _context.GetChartOfAccount(year)
               .Where(x => (x.Classification == 2 || x.Classification == 3)).ToList();
        var goodWarehouses = new List<GoodWarehouses>();
        var goodWarehouseUpdates = new List<GoodWarehouses>();
        var goodWarehouseChecks = await _context.GoodWarehouses.Where(x => x.Order == 0).ToListAsync();
        if (listAccount.Count > 0)
        {
            foreach (var account in listAccount)
            {
                if (string.IsNullOrEmpty(account.ParentRef) || account.HasDetails
                        || account.OpeningStockQuantityNB == null || account.OpeningStockQuantityNB == 0)
                    continue;
                GoodWarehouses goodWarehouse;

                if (account.Type == 6)
                {
                    var parentCodes = account.ParentRef.Split(":");

                    goodWarehouse = goodWarehouseChecks.Find(x => x.Detail2 == account.Code && x.Detail1 == parentCodes[1] && x.Account == parentCodes[0]);
                    if (goodWarehouse is null)
                        goodWarehouse = new();

                    goodWarehouse.Account = parentCodes[0];
                    goodWarehouse.Detail1 = parentCodes[1];
                    goodWarehouse.Detail2 = account.Code;
                    goodWarehouse.DetailName2 = account.Name;

                }
                else
                {
                    goodWarehouse = goodWarehouseChecks.Find(x => x.Detail1 == account.Code && x.Account == account.ParentRef);
                    if (goodWarehouse is null)
                        goodWarehouse = new();

                    goodWarehouse.Account = account.ParentRef;
                    goodWarehouse.Detail1 = account.Code;
                    goodWarehouse.DetailName1 = account.Name;
                }
                goodWarehouse.Warehouse = account.WarehouseCode;
                goodWarehouse.WarehouseName = account.WarehouseName;
                goodWarehouse.Order = 0;
                goodWarehouse.Status = 1;

                goodWarehouse.Quantity = account.OpeningStockQuantityNB ?? 0;
                goodWarehouse.QuantityInput = account.OpeningStockQuantityNB ?? 0;
                if (goodWarehouse.Id == 0)
                    goodWarehouses.Add(goodWarehouse);
                else