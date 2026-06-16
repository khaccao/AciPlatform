using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.RestaurantErp;
using AciPlatform.Domain.Entities.Ledger;
using AciPlatform.Domain.Entities.QLKho;
using AciPlatform.Domain.Entities.RestaurantErp;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.RestaurantErp;

public class RestaurantErpService : IRestaurantErpService
{
    private readonly IApplicationDbContext _context;

    public RestaurantErpService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RestaurantDashboardDto> GetDashboardAsync(string? companyCode)
    {
        var funds = Company(_context.RestaurantFunds.AsQueryable(), companyCode);
        var capital = Company(_context.RestaurantCapitalContributions.AsQueryable(), companyCode);
        var setup = Company(_context.RestaurantSetupExpenses.AsQueryable(), companyCode);
        var disbursements = Company(_context.RestaurantDisbursements.AsQueryable(), companyCode);
        var supplierDebts = Company(_context.RestaurantSupplierDebts.AsQueryable(), companyCode);
        var customerDebts = Company(_context.RestaurantCustomerDebts.AsQueryable(), companyCode);
        var purchaseRequests = Company(_context.RestaurantPurchaseRequests.AsQueryable(), companyCode);
        var paymentRequests = Company(_context.RestaurantPaymentRequests.AsQueryable(), companyCode);
        var stockBalances = Company(_context.RestaurantStockBalances.AsQueryable(), companyCode);
        var materials = Company(_context.RestaurantMaterials.AsQueryable(), companyCode);

        var cash = await funds.Where(x => x.FundType == "Cash").SumAsync(x => x.CurrentBalance);
        var bank = await funds.Where(x => x.FundType == "Bank").SumAsync(x => x.CurrentBalance);
        var totalCommitted = await capital.SumAsync(x => x.CommittedAmount);
        var totalContributed = await capital.SumAsync(x => x.ContributedAmount);
        var balances = await stockBalances.ToListAsync();
        var materialMap = await materials.ToDictionaryAsync(x => x.Id, x => x.MinStock);

        return new RestaurantDashboardDto
        {
            TotalCommittedCapital = totalCommitted,
            TotalContributedCapital = totalContributed,
            RemainingCapitalToContribute = totalCommitted - totalContributed,
            TotalSetupExpense = await setup.SumAsync(x => x.Amount),
            TotalDisbursed = await disbursements.SumAsync(x => x.Amount),
            CashBalance = cash,
            BankBalance = bank,
            TotalFundBalance = cash + bank + await funds.Where(x => x.FundType != "Cash" && x.FundType != "Bank").SumAsync(x => x.CurrentBalance),
            SupplierDebt = await supplierDebts.SumAsync(x => x.RemainingAmount),
            CustomerDebt = await customerDebts.SumAsync(x => x.RemainingAmount),
            PurchaseRequestsPending = await purchaseRequests.CountAsync(x => x.Status == RestaurantErpStatus.Submitted || x.Status == RestaurantErpStatus.Pending),
            PaymentRequestsPending = await paymentRequests.CountAsync(x => x.Status == RestaurantErpStatus.Submitted || x.Status == RestaurantErpStatus.Pending),
            ApprovedNotDisbursed = await paymentRequests
                .Where(x => x.Status == RestaurantErpStatus.Approved || x.Status == RestaurantErpStatus.PendingDisbursement)
                .SumAsync(x => x.RequestedAmount - x.DisbursedAmount),
            InventoryValue = balances.Sum(x => x.InventoryValue),
            LowStockMaterials = balances.Count(x => materialMap.TryGetValue(x.MaterialId, out var min) && min > 0 && x.Quantity <= min)
        };
    }

    public async Task<IEnumerable<RestaurantFund>> GetFundsAsync(string? companyCode)
    {
        return await Company(_context.RestaurantFunds.AsQueryable(), companyCode)
            .Where(x => !x.IsDelete)
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task<RestaurantFund> CreateFundAsync(RestaurantFundRequest request)
    {
        var fund = new RestaurantFund
        {
            Code = await NextCodeAsync<RestaurantFund>("Q", request.CompanyCode),
            Name = request.Name,
            FundType = request.FundType,
            AccountCode = string.IsNullOrWhiteSpace(request.AccountCode) ? AccountByFundType(request.FundType) : request.AccountCode,
            OpeningBalance = request.OpeningBalance,
            CurrentBalance = request.OpeningBalance,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantFunds.Add(fund);
        await _context.SaveChangesAsync();
        return fund;
    }

    public async Task<RestaurantFund> UpdateFundAsync(int id, RestaurantFundRequest request)
    {
        var fund = await Required(_context.RestaurantFunds, id, "Fund not found");
        var oldOpening = fund.OpeningBalance;
        fund.Name = request.Name;
        fund.FundType = request.FundType;
        fund.AccountCode = string.IsNullOrWhiteSpace(request.AccountCode) ? AccountByFundType(request.FundType) : request.AccountCode;
        fund.OpeningBalance = request.OpeningBalance;
        fund.CurrentBalance += request.OpeningBalance - oldOpening;
        fund.CompanyCode = request.CompanyCode ?? fund.CompanyCode;
        fund.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return fund;
    }

    public Task DeleteFundAsync(int id)
        => SoftDeleteAsync(_context.RestaurantFunds, id, "Fund not found");

    public async Task<IEnumerable<RestaurantCapitalContribution>> GetCapitalContributionsAsync(string? companyCode)
    {
        return await Company(_context.RestaurantCapitalContributions.AsQueryable(), companyCode)
            .Where(x => !x.IsDelete)
            .OrderByDescending(x => x.ContributionDate)
            .ToListAsync();
    }

    public async Task<RestaurantCapitalContribution> CreateCapitalContributionAsync(CapitalContributionRequest request)
    {
        var contribution = new RestaurantCapitalContribution
        {
            Code = await NextCodeAsync<RestaurantCapitalContribution>("GV", request.CompanyCode),
            ContributorName = request.ContributorName,
            CommittedAmount = request.CommittedAmount,
            ContributedAmount = request.ContributedAmount,
            ContributionDate = request.ContributionDate,
            PaymentMethod = request.PaymentMethod,
            FundId = request.FundId,
            Note = request.Note,
            CompanyCode = request.CompanyCode
        };

        RestaurantFund? fund = null;
        if (request.FundId.HasValue)
        {
            fund = await _context.RestaurantFunds.FindAsync(request.FundId.Value);
            if (fund == null) throw new InvalidOperationException("Fund not found");
            fund.CurrentBalance += request.ContributedAmount;
        }

        var ledger = CreateLedger(
            "RestaurantCapital",
            request.ContributionDate,
            contribution.Code,
            $"Capital contribution from {request.ContributorName}",
            fund?.AccountCode ?? AccountByFundType(request.PaymentMethod),
            "411",
            request.ContributedAmount,
            request.CompanyCode);
        _context.LedgerEntries.Add(ledger);
        _context.RestaurantCapitalContributions.Add(contribution);
        await _context.SaveChangesAsync();

        contribution.LedgerEntryId = ledger.Id;
        await _context.SaveChangesAsync();
        return contribution;
    }

    public async Task<RestaurantCapitalContribution> UpdateCapitalContributionAsync(int id, CapitalContributionRequest request)
    {
        var contribution = await Required(_context.RestaurantCapitalContributions, id, "Capital contribution not found");
        contribution.ContributorName = request.ContributorName;
        contribution.CommittedAmount = request.CommittedAmount;
        contribution.ContributionDate = request.ContributionDate;
        contribution.PaymentMethod = request.PaymentMethod;
        contribution.Note = request.Note;
        contribution.CompanyCode = request.CompanyCode ?? contribution.CompanyCode;
        contribution.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return contribution;
    }

    public Task DeleteCapitalContributionAsync(int id)
        => SoftDeleteAsync(_context.RestaurantCapitalContributions, id, "Capital contribution not found");

    public async Task<IEnumerable<RestaurantSetupExpense>> GetSetupExpensesAsync(string? companyCode)
    {
        return await Company(_context.RestaurantSetupExpenses.AsQueryable(), companyCode)
            .Where(x => !x.IsDelete)
            .OrderByDescending(x => x.ExpenseDate)
            .ToListAsync();
    }

    public async Task<RestaurantSetupExpense> CreateSetupExpenseAsync(SetupExpenseRequest request)
    {
        var expense = new RestaurantSetupExpense
        {
            Code = await NextCodeAsync<RestaurantSetupExpense>("CP", request.CompanyCode),
            Name = request.Name,
            ExpenseGroup = request.ExpenseGroup,
            Amount = request.Amount,
            ExpenseDate = request.ExpenseDate,
            PaymentRequestId = request.PaymentRequestId,
            PurchaseRequestId = request.PurchaseRequestId,
            Note = request.Note,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantSetupExpenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<RestaurantSetupExpense> UpdateSetupExpenseAsync(int id, SetupExpenseRequest request)
    {
        var expense = await Required(_context.RestaurantSetupExpenses, id, "Setup expense not found");
        expense.Name = request.Name;
        expense.ExpenseGroup = request.ExpenseGroup;
        expense.Amount = request.Amount;
        expense.ExpenseDate = request.ExpenseDate;
        expense.PaymentRequestId = request.PaymentRequestId;
        expense.PurchaseRequestId = request.PurchaseRequestId;
        expense.Note = request.Note;
        expense.CompanyCode = request.CompanyCode ?? expense.CompanyCode;
        expense.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return expense;
    }

    public Task DeleteSetupExpenseAsync(int id)
        => SoftDeleteAsync(_context.RestaurantSetupExpenses, id, "Setup expense not found");

    public async Task<IEnumerable<RestaurantMaterialGroup>> GetMaterialGroupsAsync(string? companyCode)
    {
        return await Company(_context.RestaurantMaterialGroups.AsQueryable(), companyCode)
            .Where(x => !x.IsDelete)
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task<RestaurantMaterialGroup> CreateMaterialGroupAsync(MaterialGroupRequest request)
    {
        var group = new RestaurantMaterialGroup
        {
            Code = await NextCodeAsync<RestaurantMaterialGroup>("NVT", request.CompanyCode),
            Name = request.Name,
            Note = request.Note,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantMaterialGroups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<RestaurantMaterialGroup> UpdateMaterialGroupAsync(int id, MaterialGroupRequest request)
    {
        var group = await Required(_context.RestaurantMaterialGroups, id, "Material group not found");
        group.Name = request.Name;
        group.Note = request.Note;
        group.CompanyCode = request.CompanyCode ?? group.CompanyCode;
        group.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return group;
    }

    public Task DeleteMaterialGroupAsync(int id)
        => SoftDeleteAsync(_context.RestaurantMaterialGroups, id, "Material group not found");

    public async Task<IEnumerable<RestaurantMaterial>> GetMaterialsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantMaterials.AsQueryable(), filter.CompanyCode).Where(x => !x.IsDelete);
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(x => x.Code.Contains(filter.SearchText) || x.Name.Contains(filter.SearchText));

        return await query.OrderBy(x => x.Code)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<RestaurantMaterial> CreateMaterialAsync(MaterialRequest request)
    {
        var material = new RestaurantMaterial
        {
            Code = await NextCodeAsync<RestaurantMaterial>("VT", request.CompanyCode),
            Name = request.Name,
            MaterialGroupId = request.MaterialGroupId,
            Unit = request.Unit,
            PurchaseUnit = request.PurchaseUnit,
            ConversionRate = request.ConversionRate <= 0 ? 1 : request.ConversionRate,
            MinStock = request.MinStock,
            MaxStock = request.MaxStock,
            LastPurchasePrice = request.LastPurchasePrice,
            DefaultSupplierId = request.DefaultSupplierId,
            HasExpiryTracking = request.HasExpiryTracking,
            InventoryAccountCode = string.IsNullOrWhiteSpace(request.InventoryAccountCode) ? "152" : request.InventoryAccountCode,
            ExpenseAccountCode = string.IsNullOrWhiteSpace(request.ExpenseAccountCode) ? "642" : request.ExpenseAccountCode,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantMaterials.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task<RestaurantMaterial> UpdateMaterialAsync(int id, MaterialRequest request)
    {
        var material = await Required(_context.RestaurantMaterials, id, "Material not found");
        material.Name = request.Name;
        material.MaterialGroupId = request.MaterialGroupId;
        material.Unit = request.Unit;
        material.PurchaseUnit = request.PurchaseUnit;
        material.ConversionRate = request.ConversionRate <= 0 ? 1 : request.ConversionRate;
        material.MinStock = request.MinStock;
        material.MaxStock = request.MaxStock;
        material.LastPurchasePrice = request.LastPurchasePrice;
        material.DefaultSupplierId = request.DefaultSupplierId;
        material.HasExpiryTracking = request.HasExpiryTracking;
        material.InventoryAccountCode = string.IsNullOrWhiteSpace(request.InventoryAccountCode) ? "152" : request.InventoryAccountCode;
        material.ExpenseAccountCode = string.IsNullOrWhiteSpace(request.ExpenseAccountCode) ? "642" : request.ExpenseAccountCode;
        material.CompanyCode = request.CompanyCode ?? material.CompanyCode;
        material.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return material;
    }

    public Task DeleteMaterialAsync(int id)
        => SoftDeleteAsync(_context.RestaurantMaterials, id, "Material not found");

    public async Task<IEnumerable<RestaurantPurchaseRequest>> GetPurchaseRequestsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantPurchaseRequests.AsQueryable(), filter.CompanyCode).Where(x => !x.IsDelete);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(x => x.Code.Contains(filter.SearchText) || (x.Reason != null && x.Reason.Contains(filter.SearchText)));

        return await query.OrderByDescending(x => x.RequestDate)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<RestaurantPurchaseRequest> CreatePurchaseRequestAsync(PurchaseRequestRequest request)
    {
        var pr = new RestaurantPurchaseRequest
        {
            Code = await NextCodeAsync<RestaurantPurchaseRequest>("DN", request.CompanyCode),
            RequestDepartment = request.RequestDepartment,
            RequestedBy = request.RequestedBy,
            RequestDate = request.RequestDate,
            NeededDate = request.NeededDate,
            Reason = request.Reason,
            Status = RestaurantErpStatus.Draft,
            CompanyCode = request.CompanyCode,
            TotalEstimatedAmount = request.Items.Sum(x => x.Quantity * x.EstimatedUnitPrice)
        };
        _context.RestaurantPurchaseRequests.Add(pr);
        await _context.SaveChangesAsync();

        _context.RestaurantPurchaseRequestDetails.AddRange(request.Items.Select(x => new RestaurantPurchaseRequestDetail
        {
            PurchaseRequestId = pr.Id,
            MaterialId = x.MaterialId,
            Quantity = x.Quantity,
            EstimatedUnitPrice = x.EstimatedUnitPrice,
            EstimatedAmount = x.Quantity * x.EstimatedUnitPrice,
            Reason = x.Reason,
            CompanyCode = request.CompanyCode
        }));
        await _context.SaveChangesAsync();
        return pr;
    }

    public async Task<RestaurantPurchaseRequest> UpdatePurchaseRequestAsync(int id, PurchaseRequestRequest request)
    {
        var pr = await Required(_context.RestaurantPurchaseRequests, id, "Purchase request not found");
        if (pr.Status != RestaurantErpStatus.Draft)
            throw new InvalidOperationException("Only draft purchase requests can be edited");

        pr.RequestDepartment = request.RequestDepartment;
        pr.RequestedBy = request.RequestedBy;
        pr.RequestDate = request.RequestDate;
        pr.NeededDate = request.NeededDate;
        pr.Reason = request.Reason;
        pr.TotalEstimatedAmount = request.Items.Sum(x => x.Quantity * x.EstimatedUnitPrice);
        pr.CompanyCode = request.CompanyCode ?? pr.CompanyCode;
        pr.UpdatedAt = DateTime.Now;

        var oldDetails = await _context.RestaurantPurchaseRequestDetails.Where(x => x.PurchaseRequestId == id).ToListAsync();
        _context.RestaurantPurchaseRequestDetails.RemoveRange(oldDetails);
        _context.RestaurantPurchaseRequestDetails.AddRange(request.Items.Select(x => new RestaurantPurchaseRequestDetail
        {
            PurchaseRequestId = pr.Id,
            MaterialId = x.MaterialId,
            Quantity = x.Quantity,
            EstimatedUnitPrice = x.EstimatedUnitPrice,
            EstimatedAmount = x.Quantity * x.EstimatedUnitPrice,
            Reason = x.Reason,
            CompanyCode = pr.CompanyCode
        }));

        await _context.SaveChangesAsync();
        return pr;
    }

    public Task DeletePurchaseRequestAsync(int id)
        => SoftDeleteAsync(_context.RestaurantPurchaseRequests, id, "Purchase request not found");

    public async Task SubmitPurchaseRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var pr = await Required(_context.RestaurantPurchaseRequests, id, "Purchase request not found");
        ChangeStatus(pr, RestaurantErpStatus.Submitted, "Submit", request);
        await _context.SaveChangesAsync();
    }

    public async Task ApprovePurchaseRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var pr = await Required(_context.RestaurantPurchaseRequests, id, "Purchase request not found");
        if (pr.Status is RestaurantErpStatus.Approved or RestaurantErpStatus.Completed)
            throw new InvalidOperationException("Purchase request is already approved or completed");

        ChangeStatus(pr, RestaurantErpStatus.Approved, "Approve", request);
        await _context.SaveChangesAsync();
    }

    public async Task RejectPurchaseRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var pr = await Required(_context.RestaurantPurchaseRequests, id, "Purchase request not found");
        ChangeStatus(pr, RestaurantErpStatus.Rejected, "Reject", request);
        await _context.SaveChangesAsync();
    }

    public async Task<RestaurantPurchaseOrder> CreatePurchaseOrderAsync(PurchaseOrderRequest request)
    {
        var po = new RestaurantPurchaseOrder
        {
            Code = await NextCodeAsync<RestaurantPurchaseOrder>("PO", request.CompanyCode),
            PurchaseRequestId = request.PurchaseRequestId,
            SupplierId = request.SupplierId,
            OrderDate = request.OrderDate,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            Status = RestaurantErpStatus.New,
            Note = request.Note,
            CompanyCode = request.CompanyCode
        };
        ApplyPoTotals(po, request.Items);
        _context.RestaurantPurchaseOrders.Add(po);
        await _context.SaveChangesAsync();

        _context.RestaurantPurchaseOrderDetails.AddRange(request.Items.Select(x => new RestaurantPurchaseOrderDetail
        {
            PurchaseOrderId = po.Id,
            MaterialId = x.MaterialId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            VatRate = x.VatRate,
            VatAmount = x.Quantity * x.UnitPrice * x.VatRate / 100,
            LineAmount = x.Quantity * x.UnitPrice * (1 + x.VatRate / 100),
            CompanyCode = request.CompanyCode
        }));

        if (request.PurchaseRequestId.HasValue)
        {
            var pr = await _context.RestaurantPurchaseRequests.FindAsync(request.PurchaseRequestId.Value);
            if (pr != null)
            {
                pr.CreatedPurchaseOrderId = po.Id;
                ChangeStatus(pr, RestaurantErpStatus.Converted, "CreatePO", new ApprovalDecisionRequest(null, null, $"Created PO {po.Code}"));
            }
        }

        await _context.SaveChangesAsync();
        return po;
    }

    public async Task<RestaurantPurchaseOrder> CreatePurchaseOrderFromRequestAsync(int purchaseRequestId, PurchaseOrderRequest request)
    {
        var pr = await Required(_context.RestaurantPurchaseRequests, purchaseRequestId, "Purchase request not found");
        if (pr.Status != RestaurantErpStatus.Approved)
            throw new InvalidOperationException("Only approved purchase requests can be converted to PO");

        var lines = request.Items.Any()
            ? request.Items
            : await _context.RestaurantPurchaseRequestDetails
                .Where(x => x.PurchaseRequestId == purchaseRequestId)
                .Select(x => new PurchaseOrderLineRequest(x.MaterialId, x.Quantity, x.EstimatedUnitPrice, 0))
                .ToListAsync();

        var poRequest = request with { PurchaseRequestId = purchaseRequestId, Items = lines };
        return await CreatePurchaseOrderAsync(poRequest);
    }

    public async Task<IEnumerable<RestaurantPurchaseOrder>> GetPurchaseOrdersAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantPurchaseOrders.AsQueryable(), filter.CompanyCode).Where(x => !x.IsDelete);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SearchText)) query = query.Where(x => x.Code.Contains(filter.SearchText));
        return await query.OrderByDescending(x => x.OrderDate).Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
    }

    public async Task<RestaurantPurchaseOrder> UpdatePurchaseOrderAsync(int id, PurchaseOrderRequest request)
    {
        var po = await Required(_context.RestaurantPurchaseOrders, id, "Purchase order not found");
        if (po.ReceivedAmount > 0)
            throw new InvalidOperationException("Purchase orders with receipts cannot be edited");

        po.PurchaseRequestId = request.PurchaseRequestId;
        po.SupplierId = request.SupplierId;
        po.OrderDate = request.OrderDate;
        po.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
        po.Note = request.Note;
        po.CompanyCode = request.CompanyCode ?? po.CompanyCode;
        po.UpdatedAt = DateTime.Now;
        ApplyPoTotals(po, request.Items);

        var oldDetails = await _context.RestaurantPurchaseOrderDetails.Where(x => x.PurchaseOrderId == id).ToListAsync();
        _context.RestaurantPurchaseOrderDetails.RemoveRange(oldDetails);
        _context.RestaurantPurchaseOrderDetails.AddRange(request.Items.Select(x => new RestaurantPurchaseOrderDetail
        {
            PurchaseOrderId = po.Id,
            MaterialId = x.MaterialId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
            VatRate = x.VatRate,
            VatAmount = x.Quantity * x.UnitPrice * x.VatRate / 100,
            LineAmount = x.Quantity * x.UnitPrice * (1 + x.VatRate / 100),
            CompanyCode = po.CompanyCode
        }));

        await _context.SaveChangesAsync();
        return po;
    }

    public Task DeletePurchaseOrderAsync(int id)
        => SoftDeleteAsync(_context.RestaurantPurchaseOrders, id, "Purchase order not found");

    public async Task SetPurchaseOrderStatusAsync(int id, string status)
    {
        var po = await Required(_context.RestaurantPurchaseOrders, id, "Purchase order not found");
        po.Status = status;
        po.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RestaurantGoodsReceipt>> GetGoodsReceiptsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantGoodsReceipts.AsQueryable(), filter.CompanyCode).Where(x => !x.IsDelete);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SearchText)) query = query.Where(x => x.Code.Contains(filter.SearchText));
        return await query.OrderByDescending(x => x.ReceiptDate).Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
    }

    public async Task<RestaurantGoodsReceipt> CreateGoodsReceiptAsync(GoodsReceiptRequest request)
    {
        if (!request.Items.Any()) throw new InvalidOperationException("Goods receipt must contain items");
        var receipt = new RestaurantGoodsReceipt
        {
            Code = await NextCodeAsync<RestaurantGoodsReceipt>("NK", request.CompanyCode),
            PurchaseOrderId = request.PurchaseOrderId,
            SupplierId = request.SupplierId,
            WarehouseCode = string.IsNullOrWhiteSpace(request.WarehouseCode) ? "MAIN" : request.WarehouseCode,
            WarehouseName = request.WarehouseName,
            ReceiptDate = request.ReceiptDate,
            Status = request.Status == RestaurantErpStatus.Draft ? RestaurantErpStatus.Draft : RestaurantErpStatus.Received,
            Note = request.Note,
            CompanyCode = request.CompanyCode,
            TotalAmount = request.Items.Sum(x => x.ReceivedQuantity * x.UnitPrice),
            DamagedAmount = request.Items.Sum(x => x.DamagedQuantity * x.UnitPrice)
        };
        _context.RestaurantGoodsReceipts.Add(receipt);
        await _context.SaveChangesAsync();

        _context.RestaurantGoodsReceiptDetails.AddRange(request.Items.Select(x => new RestaurantGoodsReceiptDetail
        {
            GoodsReceiptId = receipt.Id,
            MaterialId = x.MaterialId,
            OrderedQuantity = x.OrderedQuantity,
            ReceivedQuantity = x.ReceivedQuantity,
            DamagedQuantity = x.DamagedQuantity,
            UnitPrice = x.UnitPrice,
            LineAmount = x.ReceivedQuantity * x.UnitPrice,
            ManufactureDate = x.ManufactureDate,
            ExpiryDate = x.ExpiryDate,
            LotNumber = x.LotNumber,
            CompanyCode = request.CompanyCode
        }));

        if (receipt.Status != RestaurantErpStatus.Draft)
        {
            await ApplyGoodsReceiptEffects(receipt, request.Items);
        }

        await _context.SaveChangesAsync();
        return receipt;
    }

    public async Task<RestaurantGoodsReceipt> UpdateGoodsReceiptAsync(int id, GoodsReceiptRequest request)
    {
        var receipt = await Required(_context.RestaurantGoodsReceipts, id, "Goods receipt not found");
        if (receipt.Status != RestaurantErpStatus.Draft)
            throw new InvalidOperationException("Only draft goods receipts can be edited");

        receipt.PurchaseOrderId = request.PurchaseOrderId;
        receipt.SupplierId = request.SupplierId;
        receipt.WarehouseCode = string.IsNullOrWhiteSpace(request.WarehouseCode) ? "MAIN" : request.WarehouseCode;
        receipt.WarehouseName = request.WarehouseName;
        receipt.ReceiptDate = request.ReceiptDate;
        receipt.Note = request.Note;
        receipt.CompanyCode = request.CompanyCode ?? receipt.CompanyCode;
        receipt.TotalAmount = request.Items.Sum(x => x.ReceivedQuantity * x.UnitPrice);
        receipt.DamagedAmount = request.Items.Sum(x => x.DamagedQuantity * x.UnitPrice);
        receipt.UpdatedAt = DateTime.Now;

        var oldDetails = await _context.RestaurantGoodsReceiptDetails.Where(x => x.GoodsReceiptId == id).ToListAsync();
        _context.RestaurantGoodsReceiptDetails.RemoveRange(oldDetails);
        _context.RestaurantGoodsReceiptDetails.AddRange(request.Items.Select(x => new RestaurantGoodsReceiptDetail
        {
            GoodsReceiptId = receipt.Id,
            MaterialId = x.MaterialId,
            OrderedQuantity = x.OrderedQuantity,
            ReceivedQuantity = x.ReceivedQuantity,
            DamagedQuantity = x.DamagedQuantity,
            UnitPrice = x.UnitPrice,
            LineAmount = x.ReceivedQuantity * x.UnitPrice,
            ManufactureDate = x.ManufactureDate,
            ExpiryDate = x.ExpiryDate,
            LotNumber = x.LotNumber,
            CompanyCode = receipt.CompanyCode
        }));

        await _context.SaveChangesAsync();
        return receipt;
    }

    public Task DeleteGoodsReceiptAsync(int id)
        => SoftDeleteAsync(_context.RestaurantGoodsReceipts, id, "Goods receipt not found");

    public async Task<IEnumerable<RestaurantStockBalance>> GetStockBalancesAsync(string? companyCode)
    {
        return await Company(_context.RestaurantStockBalances.AsQueryable(), companyCode)
            .OrderBy(x => x.WarehouseCode)
            .ThenBy(x => x.MaterialId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RestaurantPaymentRequest>> GetPaymentRequestsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantPaymentRequests.AsQueryable(), filter.CompanyCode).Where(x => !x.IsDelete);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(x => x.Code.Contains(filter.SearchText) || (x.ReceiverName != null && x.ReceiverName.Contains(filter.SearchText)));
        return await query.OrderByDescending(x => x.RequestDate).Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
    }

    public async Task<RestaurantPaymentRequest> CreatePaymentRequestAsync(PaymentRequestRequest request)
    {
        var amount = request.RequestedAmount > 0 ? request.RequestedAmount : request.Items.Sum(x => x.Amount);
        if (request.SupplierDebtId.HasValue)
        {
            var debt = await _context.RestaurantSupplierDebts.FindAsync(request.SupplierDebtId.Value);
            if (debt != null && amount > debt.RemainingAmount)
                throw new InvalidOperationException("Requested amount exceeds remaining supplier debt");
        }

        var payment = new RestaurantPaymentRequest
        {
            Code = await NextCodeAsync<RestaurantPaymentRequest>("DC", request.CompanyCode),
            PaymentType = request.PaymentType,
            SupplierId = request.SupplierId,
            PurchaseOrderId = request.PurchaseOrderId,
            GoodsReceiptId = request.GoodsReceiptId,
            SupplierDebtId = request.SupplierDebtId,
            SetupExpenseId = request.SetupExpenseId,
            ReceiverName = request.ReceiverName,
            RequestDate = request.RequestDate,
            RequestedAmount = amount,
            DebitAccountCode = string.IsNullOrWhiteSpace(request.DebitAccountCode) ? "642" : request.DebitAccountCode,
            Reason = request.Reason,
            Status = RestaurantErpStatus.Draft,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantPaymentRequests.Add(payment);
        await _context.SaveChangesAsync();

        _context.RestaurantPaymentRequestDetails.AddRange(request.Items.Select(x => new RestaurantPaymentRequestDetail
        {
            PaymentRequestId = payment.Id,
            Content = x.Content,
            Amount = x.Amount,
            ReferenceType = x.ReferenceType,
            ReferenceId = x.ReferenceId,
            CompanyCode = request.CompanyCode
        }));

        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<RestaurantPaymentRequest> UpdatePaymentRequestAsync(int id, PaymentRequestRequest request)
    {
        var payment = await Required(_context.RestaurantPaymentRequests, id, "Payment request not found");
        if (payment.Status != RestaurantErpStatus.Draft)
            throw new InvalidOperationException("Only draft payment requests can be edited");

        var amount = request.RequestedAmount > 0 ? request.RequestedAmount : request.Items.Sum(x => x.Amount);
        payment.PaymentType = request.PaymentType;
        payment.SupplierId = request.SupplierId;
        payment.PurchaseOrderId = request.PurchaseOrderId;
        payment.GoodsReceiptId = request.GoodsReceiptId;
        payment.SupplierDebtId = request.SupplierDebtId;
        payment.SetupExpenseId = request.SetupExpenseId;
        payment.ReceiverName = request.ReceiverName;
        payment.RequestDate = request.RequestDate;
        payment.RequestedAmount = amount;
        payment.DebitAccountCode = string.IsNullOrWhiteSpace(request.DebitAccountCode) ? "642" : request.DebitAccountCode;
        payment.Reason = request.Reason;
        payment.CompanyCode = request.CompanyCode ?? payment.CompanyCode;
        payment.UpdatedAt = DateTime.Now;

        var oldDetails = await _context.RestaurantPaymentRequestDetails.Where(x => x.PaymentRequestId == id).ToListAsync();
        _context.RestaurantPaymentRequestDetails.RemoveRange(oldDetails);
        _context.RestaurantPaymentRequestDetails.AddRange(request.Items.Select(x => new RestaurantPaymentRequestDetail
        {
            PaymentRequestId = payment.Id,
            Content = x.Content,
            Amount = x.Amount,
            ReferenceType = x.ReferenceType,
            ReferenceId = x.ReferenceId,
            CompanyCode = payment.CompanyCode
        }));

        await _context.SaveChangesAsync();
        return payment;
    }

    public Task DeletePaymentRequestAsync(int id)
        => SoftDeleteAsync(_context.RestaurantPaymentRequests, id, "Payment request not found");

    public async Task SubmitPaymentRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var payment = await Required(_context.RestaurantPaymentRequests, id, "Payment request not found");
        ChangeStatus(payment, RestaurantErpStatus.Submitted, "Submit", request);
        await _context.SaveChangesAsync();
    }

    public async Task ApprovePaymentRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var payment = await Required(_context.RestaurantPaymentRequests, id, "Payment request not found");
        ChangeStatus(payment, RestaurantErpStatus.PendingDisbursement, "Approve", request);
        await _context.SaveChangesAsync();
    }

    public async Task RejectPaymentRequestAsync(int id, ApprovalDecisionRequest request)
    {
        var payment = await Required(_context.RestaurantPaymentRequests, id, "Payment request not found");
        ChangeStatus(payment, RestaurantErpStatus.Rejected, "Reject", request);
        await _context.SaveChangesAsync();
    }

    public async Task<RestaurantDisbursement> DisburseAsync(int paymentRequestId, DisbursementRequest request)
    {
        var payment = await Required(_context.RestaurantPaymentRequests, paymentRequestId, "Payment request not found");
        if (payment.Status is not (RestaurantErpStatus.Approved or RestaurantErpStatus.PendingDisbursement))
            throw new InvalidOperationException("Payment request must be approved before disbursement");
        if (payment.DisbursedAmount + request.Amount > payment.RequestedAmount)
            throw new InvalidOperationException("Disbursement amount exceeds approved amount");

        var fund = await Required(_context.RestaurantFunds, request.FundId, "Fund not found");
        if (fund.CurrentBalance < request.Amount)
            throw new InvalidOperationException("Fund balance is insufficient");

        fund.CurrentBalance -= request.Amount;
        payment.DisbursedAmount += request.Amount;
        payment.Status = payment.DisbursedAmount >= payment.RequestedAmount
            ? RestaurantErpStatus.Completed
            : RestaurantErpStatus.PendingDisbursement;

        var companyCode = request.CompanyCode ?? payment.CompanyCode;
        var disbursementCode = await NextCodeAsync<RestaurantDisbursement>("GN", companyCode);
        var debit = payment.SupplierDebtId.HasValue ? ResolveSupplierDebtAccount(payment.SupplierId) : payment.DebitAccountCode;
        var ledger = CreateLedger(
            "RestaurantDisbursement",
            request.DisbursementDate,
            disbursementCode,
            $"Disbursement for {payment.Code} - {payment.Reason}",
            debit,
            fund.AccountCode,
            request.Amount,
            companyCode);

        var disbursement = new RestaurantDisbursement
        {
            Code = disbursementCode,
            PaymentRequestId = paymentRequestId,
            FundId = request.FundId,
            DisbursementDate = request.DisbursementDate,
            Amount = request.Amount,
            ReceiverName = request.ReceiverName ?? payment.ReceiverName,
            PaidBy = request.PaidBy,
            Note = request.Note,
            CompanyCode = companyCode
        };

        _context.LedgerEntries.Add(ledger);
        _context.RestaurantDisbursements.Add(disbursement);
        await _context.SaveChangesAsync();
        disbursement.LedgerEntryId = ledger.Id;

        if (payment.SupplierDebtId.HasValue)
        {
            var debt = await Required(_context.RestaurantSupplierDebts, payment.SupplierDebtId.Value, "Supplier debt not found");
            debt.PaidAmount += request.Amount;
            debt.RemainingAmount = Math.Max(0, debt.Amount - debt.PaidAmount);
            debt.Status = debt.RemainingAmount <= 0 ? RestaurantErpStatus.Closed : RestaurantErpStatus.Partial;
            _context.RestaurantSupplierDebtPayments.Add(new RestaurantSupplierDebtPayment
            {
                SupplierDebtId = debt.Id,
                DisbursementId = disbursement.Id,
                PaymentDate = request.DisbursementDate,
                Amount = request.Amount,
                CompanyCode = debt.CompanyCode
            });
        }

        await _context.SaveChangesAsync();
        return disbursement;
    }

    public async Task<IEnumerable<RestaurantDisbursement>> GetDisbursementsAsync(string? companyCode)
    {
        return await Company(_context.RestaurantDisbursements.AsQueryable(), companyCode)
            .OrderByDescending(x => x.DisbursementDate)
            .ToListAsync();
    }

    public async Task<RestaurantDisbursement> UpdateDisbursementAsync(int id, DisbursementRequest request)
    {
        var disbursement = await Required(_context.RestaurantDisbursements, id, "Disbursement not found");
        disbursement.DisbursementDate = request.DisbursementDate;
        disbursement.ReceiverName = request.ReceiverName;
        disbursement.PaidBy = request.PaidBy;
        disbursement.Note = request.Note;
        disbursement.CompanyCode = request.CompanyCode ?? disbursement.CompanyCode;
        disbursement.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return disbursement;
    }

    public Task DeleteDisbursementAsync(int id)
        => SoftDeleteAsync(_context.RestaurantDisbursements, id, "Disbursement not found");

    public async Task<IEnumerable<RestaurantSupplierDebt>> GetSupplierDebtsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantSupplierDebts.AsQueryable(), filter.CompanyCode);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        return await query.OrderByDescending(x => x.DebtDate).Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
    }

    public async Task<RestaurantSupplierDebt> CreateSupplierDebtAsync(SupplierDebtRequest request)
    {
        var debt = new RestaurantSupplierDebt
        {
            Code = await NextCodeAsync<RestaurantSupplierDebt>("CN", request.CompanyCode),
            SupplierId = request.SupplierId,
            PurchaseOrderId = request.PurchaseOrderId,
            GoodsReceiptId = request.GoodsReceiptId,
            DebtDate = request.DebtDate,
            DueDate = request.DueDate,
            Amount = request.Amount,
            RemainingAmount = request.Amount,
            Status = RestaurantErpStatus.Open,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantSupplierDebts.Add(debt);
        await _context.SaveChangesAsync();
        return debt;
    }

    public async Task<RestaurantSupplierDebt> UpdateSupplierDebtAsync(int id, SupplierDebtRequest request)
    {
        var debt = await Required(_context.RestaurantSupplierDebts, id, "Supplier debt not found");
        debt.SupplierId = request.SupplierId;
        debt.PurchaseOrderId = request.PurchaseOrderId;
        debt.GoodsReceiptId = request.GoodsReceiptId;
        debt.DebtDate = request.DebtDate;
        debt.DueDate = request.DueDate;
        debt.Amount = request.Amount;
        debt.RemainingAmount = Math.Max(0, request.Amount - debt.PaidAmount);
        debt.Status = debt.RemainingAmount <= 0 ? RestaurantErpStatus.Closed : debt.PaidAmount > 0 ? RestaurantErpStatus.Partial : RestaurantErpStatus.Open;
        debt.CompanyCode = request.CompanyCode ?? debt.CompanyCode;
        debt.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return debt;
    }

    public Task DeleteSupplierDebtAsync(int id)
        => SoftDeleteAsync(_context.RestaurantSupplierDebts, id, "Supplier debt not found");

    public async Task<IEnumerable<RestaurantCustomerDebt>> GetCustomerDebtsAsync(RestaurantErpFilter filter)
    {
        var query = Company(_context.RestaurantCustomerDebts.AsQueryable(), filter.CompanyCode);
        if (!string.IsNullOrWhiteSpace(filter.Status)) query = query.Where(x => x.Status == filter.Status);
        return await query.OrderByDescending(x => x.DebtDate).Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
    }

    public async Task<RestaurantCustomerDebt> CreateCustomerDebtAsync(CustomerDebtRequest request)
    {
        var customer = await _context.Customers.FindAsync(request.CustomerId);
        var debt = new RestaurantCustomerDebt
        {
            Code = await NextCodeAsync<RestaurantCustomerDebt>("PT", request.CompanyCode),
            CustomerId = request.CustomerId,
            DebtDate = request.DebtDate,
            DueDate = request.DueDate,
            Amount = request.Amount,
            RemainingAmount = request.Amount,
            Status = RestaurantErpStatus.Open,
            Description = request.Description,
            CompanyCode = request.CompanyCode
        };

        var ledger = CreateLedger(
            "RestaurantCustomerDebt",
            request.DebtDate,
            debt.Code,
            $"Customer debt {customer?.Name}: {request.Description}",
            ResolveCustomerDebtAccount(request.CustomerId),
            "511",
            request.Amount,
            request.CompanyCode);
        _context.LedgerEntries.Add(ledger);
        _context.RestaurantCustomerDebts.Add(debt);
        await _context.SaveChangesAsync();
        debt.LedgerEntryId = ledger.Id;
        await _context.SaveChangesAsync();
        return debt;
    }

    public async Task<RestaurantCustomerDebt> UpdateCustomerDebtAsync(int id, CustomerDebtRequest request)
    {
        var debt = await Required(_context.RestaurantCustomerDebts, id, "Customer debt not found");
        debt.CustomerId = request.CustomerId;
        debt.DebtDate = request.DebtDate;
        debt.DueDate = request.DueDate;
        debt.Amount = request.Amount;
        debt.RemainingAmount = Math.Max(0, request.Amount - debt.ReceivedAmount);
        debt.Status = debt.RemainingAmount <= 0 ? RestaurantErpStatus.Closed : debt.ReceivedAmount > 0 ? RestaurantErpStatus.Partial : RestaurantErpStatus.Open;
        debt.Description = request.Description;
        debt.CompanyCode = request.CompanyCode ?? debt.CompanyCode;
        debt.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return debt;
    }

    public Task DeleteCustomerDebtAsync(int id)
        => SoftDeleteAsync(_context.RestaurantCustomerDebts, id, "Customer debt not found");

    public async Task<RestaurantCustomerDebtReceipt> ReceiveCustomerDebtAsync(int customerDebtId, CustomerDebtReceiptRequest request)
    {
        var debt = await Required(_context.RestaurantCustomerDebts, customerDebtId, "Customer debt not found");
        if (debt.ReceivedAmount + request.Amount > debt.Amount)
            throw new InvalidOperationException("Receipt amount exceeds remaining customer debt");

        RestaurantFund? fund = null;
        if (request.FundId.HasValue)
        {
            fund = await Required(_context.RestaurantFunds, request.FundId.Value, "Fund not found");
            fund.CurrentBalance += request.Amount;
        }

        debt.ReceivedAmount += request.Amount;
        debt.RemainingAmount = Math.Max(0, debt.Amount - debt.ReceivedAmount);
        debt.Status = debt.RemainingAmount <= 0 ? RestaurantErpStatus.Closed : RestaurantErpStatus.Partial;

        var ledger = CreateLedger(
            "RestaurantCustomerReceipt",
            request.ReceiptDate,
            debt.Code,
            $"Receipt for customer debt {debt.Code}",
            fund?.AccountCode ?? "111",
            ResolveCustomerDebtAccount(debt.CustomerId),
            request.Amount,
            request.CompanyCode ?? debt.CompanyCode);
        var receipt = new RestaurantCustomerDebtReceipt
        {
            CustomerDebtId = customerDebtId,
            FundId = request.FundId,
            ReceiptDate = request.ReceiptDate,
            Amount = request.Amount,
            ReceivedBy = request.ReceivedBy,
            CompanyCode = request.CompanyCode ?? debt.CompanyCode
        };

        _context.LedgerEntries.Add(ledger);
        _context.RestaurantCustomerDebtReceipts.Add(receipt);
        await _context.SaveChangesAsync();
        receipt.LedgerEntryId = ledger.Id;
        await _context.SaveChangesAsync();
        return receipt;
    }

    public async Task<IEnumerable<RestaurantApprovalHistory>> GetApprovalHistoriesAsync(string documentType, int documentId)
    {
        return await _context.RestaurantApprovalHistories
            .Where(x => x.DocumentType == documentType && x.DocumentId == documentId)
            .OrderByDescending(x => x.ActionDate)
            .ToListAsync();
    }

    public async Task<RestaurantAttachment> AddAttachmentAsync(AttachmentRequest request)
    {
        var attachment = new RestaurantAttachment
        {
            DocumentType = request.DocumentType,
            DocumentId = request.DocumentId,
            FileName = request.FileName,
            FileUrl = request.FileUrl,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            CompanyCode = request.CompanyCode
        };
        _context.RestaurantAttachments.Add(attachment);
        await _context.SaveChangesAsync();
        return attachment;
    }

    public async Task<IEnumerable<RestaurantAttachment>> GetAttachmentsAsync(string documentType, int documentId)
    {
        return await _context.RestaurantAttachments
            .Where(x => x.DocumentType == documentType && x.DocumentId == documentId && !x.IsDelete)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    private async Task ApplyGoodsReceiptEffects(RestaurantGoodsReceipt receipt, List<GoodsReceiptLineRequest> items)
    {
        var materials = await _context.RestaurantMaterials
            .Where(x => items.Select(i => i.MaterialId).Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);

        foreach (var item in items)
        {
            var balance = await _context.RestaurantStockBalances.FirstOrDefaultAsync(x =>
                x.CompanyCode == receipt.CompanyCode &&
                x.MaterialId == item.MaterialId &&
                x.WarehouseCode == receipt.WarehouseCode);

            var oldQty = balance?.Quantity ?? 0;
            var oldValue = balance?.InventoryValue ?? 0;
            var qtyIn = item.ReceivedQuantity;
            var lineValue = qtyIn * item.UnitPrice;
            var newQty = oldQty + qtyIn;
            var newValue = oldValue + lineValue;

            if (balance == null)
            {
                balance = new RestaurantStockBalance
                {
                    CompanyCode = receipt.CompanyCode,
                    MaterialId = item.MaterialId,
                    WarehouseCode = receipt.WarehouseCode
                };
                _context.RestaurantStockBalances.Add(balance);
            }

            balance.Quantity = newQty;
            balance.InventoryValue = newValue;
            balance.AverageUnitPrice = newQty == 0 ? 0 : newValue / newQty;
            balance.LastTransactionDate = receipt.ReceiptDate;
            balance.StockStatus = materials.TryGetValue(item.MaterialId, out var material) && newQty <= material.MinStock ? "LowStock" : "Normal";

            _context.RestaurantStockTransactions.Add(new RestaurantStockTransaction
            {
                TransactionType = "Receipt",
                DocumentType = "GoodsReceipt",
                DocumentId = receipt.Id,
                MaterialId = item.MaterialId,
                WarehouseCode = receipt.WarehouseCode,
                QuantityIn = qtyIn,
                UnitPrice = item.UnitPrice,
                BalanceAfter = newQty,
                TransactionDate = receipt.ReceiptDate,
                LotNumber = item.LotNumber,
                ExpiryDate = item.ExpiryDate,
                CompanyCode = receipt.CompanyCode
            });

            if (materials.TryGetValue(item.MaterialId, out var mat))
            {
                mat.LastPurchasePrice = item.UnitPrice;
            }

            await UpsertLegacyGoodWarehouse(receipt, item, materials.GetValueOrDefault(item.MaterialId));
        }

        if (receipt.PurchaseOrderId.HasValue)
        {
            var po = await _context.RestaurantPurchaseOrders.FindAsync(receipt.PurchaseOrderId.Value);
            if (po != null)
            {
                po.ReceivedAmount += receipt.TotalAmount;
                var poLines = await _context.RestaurantPurchaseOrderDetails.Where(x => x.PurchaseOrderId == po.Id).ToListAsync();
                foreach (var line in poLines)
                {
                    line.ReceivedQuantity += items.Where(x => x.MaterialId == line.MaterialId).Sum(x => x.ReceivedQuantity);
                }

                po.Status = poLines.All(x => x.ReceivedQuantity >= x.Quantity)
                    ? RestaurantErpStatus.FullyReceived
                    : RestaurantErpStatus.PartiallyReceived;
            }
        }

        if (receipt.SupplierId.HasValue && receipt.TotalAmount > 0)
        {
            var debt = new RestaurantSupplierDebt
            {
                Code = await NextCodeAsync<RestaurantSupplierDebt>("CN", receipt.CompanyCode),
                SupplierId = receipt.SupplierId.Value,
                PurchaseOrderId = receipt.PurchaseOrderId,
                GoodsReceiptId = receipt.Id,
                DebtDate = receipt.ReceiptDate,
                Amount = receipt.TotalAmount,
                RemainingAmount = receipt.TotalAmount,
                Status = RestaurantErpStatus.Open,
                CompanyCode = receipt.CompanyCode
            };
            _context.RestaurantSupplierDebts.Add(debt);
            await _context.SaveChangesAsync();
            receipt.SupplierDebtId = debt.Id;
        }

        var debitAccount = items.Select(x => materials.GetValueOrDefault(x.MaterialId)?.InventoryAccountCode).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "152";
        var ledger = CreateLedger(
            "RestaurantGoodsReceipt",
            receipt.ReceiptDate,
            receipt.Code,
            $"Goods receipt {receipt.Code}",
            debitAccount,
            ResolveSupplierDebtAccount(receipt.SupplierId),
            receipt.TotalAmount,
            receipt.CompanyCode);
        _context.LedgerEntries.Add(ledger);
        await _context.SaveChangesAsync();
        receipt.LedgerEntryId = ledger.Id;
    }

    private async Task UpsertLegacyGoodWarehouse(RestaurantGoodsReceipt receipt, GoodsReceiptLineRequest item, RestaurantMaterial? material)
    {
        if (material == null) return;

        var legacy = await _context.GoodWarehouses.FirstOrDefaultAsync(x =>
            x.Detail1 == material.Code &&
            x.Warehouse == receipt.WarehouseCode &&
            !x.IsDeleted);

        if (legacy == null)
        {
            legacy = new GoodWarehouses
            {
                MenuType = "RestaurantMaterial",
                Account = material.InventoryAccountCode,
                AccountName = "Inventory",
                Warehouse = receipt.WarehouseCode,
                WarehouseName = receipt.WarehouseName,
                Detail1 = material.Code,
                DetailName1 = material.Name,
                GoodsType = "material",
                Quantity = (double)item.ReceivedQuantity,
                QuantityInput = (double)item.ReceivedQuantity,
                Status = 1,
                OrginalVoucherNumber = receipt.Code,
                DateExpiration = item.ExpiryDate,
                CreatedDate = receipt.ReceiptDate
            };
            _context.GoodWarehouses.Add(legacy);
        }
        else
        {
            legacy.Quantity += (double)item.ReceivedQuantity;
            legacy.QuantityInput += (double)item.ReceivedQuantity;
            legacy.UpdatedDate = DateTime.Now;
        }
    }

    private void ApplyPoTotals(RestaurantPurchaseOrder po, List<PurchaseOrderLineRequest> items)
    {
        po.SubTotal = items.Sum(x => x.Quantity * x.UnitPrice);
        po.VatAmount = items.Sum(x => x.Quantity * x.UnitPrice * x.VatRate / 100);
        po.TotalAmount = po.SubTotal + po.VatAmount;
    }

    private void ChangeStatus(RestaurantEntity document, string toStatus, string action, ApprovalDecisionRequest request)
    {
        var fromStatus = document switch
        {
            RestaurantPurchaseRequest pr => pr.Status,
            RestaurantPaymentRequest payment => payment.Status,
            _ => null
        };

        switch (document)
        {
            case RestaurantPurchaseRequest pr:
                pr.Status = toStatus;
                break;
            case RestaurantPaymentRequest payment:
                payment.Status = toStatus;
                break;
        }

        document.UpdatedAt = DateTime.Now;
        _context.RestaurantApprovalHistories.Add(new RestaurantApprovalHistory
        {
            DocumentType = document.GetType().Name.Replace("Restaurant", ""),
            DocumentId = document.Id,
            Action = action,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            ApproverId = request.ApproverId,
            ApproverName = request.ApproverName,
            Note = request.Note,
            CompanyCode = document.CompanyCode
        });
    }

    private static IQueryable<T> Company<T>(IQueryable<T> query, string? companyCode) where T : RestaurantEntity
    {
        return string.IsNullOrWhiteSpace(companyCode)
            ? query
            : query.Where(x => x.CompanyCode == companyCode || x.CompanyCode == null);
    }

    private async Task<string> NextCodeAsync<T>(string prefix, string? companyCode) where T : RestaurantEntity
    {
        var codes = await Company(_context.Set<T>().AsQueryable(), companyCode)
            .Select(x => EF.Property<string>(x, "Code"))
            .Where(code => code != null && code.StartsWith(prefix))
            .ToListAsync();

        var max = 0;
        foreach (var code in codes)
        {
            var suffix = code[prefix.Length..].TrimStart('-', '_');
            if (int.TryParse(suffix, out var number) && number > max)
            {
                max = number;
            }
        }

        return $"{prefix}{max + 1:000}";
    }

    private async Task SoftDeleteAsync<T>(DbSet<T> set, int id, string message) where T : RestaurantEntity
    {
        var entity = await Required(set, id, message);
        entity.IsDelete = true;
        entity.DeleteAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    private static string AccountByFundType(string? fundType)
    {
        return fundType?.Equals("Bank", StringComparison.OrdinalIgnoreCase) == true ? "112" : "111";
    }

    private string ResolveSupplierDebtAccount(int? supplierId)
    {
        if (!supplierId.HasValue) return "331";
        var supplier = _context.Customers.Find(supplierId.Value);
        return string.IsNullOrWhiteSpace(supplier?.Code) ? "331" : $"331.{supplier.Code}";
    }

    private string ResolveCustomerDebtAccount(int customerId)
    {
        var customer = _context.Customers.Find(customerId);
        return string.IsNullOrWhiteSpace(customer?.Code) ? "131" : $"131.{customer.Code}";
    }

    private static LedgerEntry CreateLedger(
        string type,
        DateTime date,
        string voucherNumber,
        string description,
        string debitCode,
        string creditCode,
        decimal amount,
        string? companyCode)
    {
        var safeVoucherNumber = voucherNumber ?? string.Empty;
        var safeDescription = description ?? string.Empty;
        var safeCompanyCode = companyCode ?? string.Empty;

        return new LedgerEntry
        {
            Type = type,
            Month = date.Month,
            BookDate = date,
            VoucherNumber = safeVoucherNumber,
            OrginalCode = safeVoucherNumber,
            OrginalVoucherNumber = safeVoucherNumber,
            OrginalBookDate = date,
            OrginalFullName = string.Empty,
            OrginalDescription = safeDescription,
            OrginalDescriptionEN = safeDescription,
            OrginalCompanyName = safeCompanyCode,
            OrginalAddress = string.Empty,
            AttachVoucher = string.Empty,
            ReferenceVoucherNumber = string.Empty,
            ReferenceFullName = string.Empty,
            ReferenceAddress = string.Empty,
            InvoiceCode = string.Empty,
            InvoiceAdditionalDeclarationCode = string.Empty,
            InvoiceNumber = string.Empty,
            InvoiceTaxCode = string.Empty,
            InvoiceAddress = string.Empty,
            InvoiceSerial = string.Empty,
            InvoiceName = string.Empty,
            InvoiceProductItem = string.Empty,
            DebitCode = debitCode,
            DebitWarehouse = string.Empty,
            DebitDetailCodeFirst = string.Empty,
            DebitDetailCodeSecond = string.Empty,
            CreditCode = creditCode,
            CreditWarehouse = string.Empty,
            CreditDetailCodeFirst = string.Empty,
            CreditDetailCodeSecond = string.Empty,
            ProjectCode = string.Empty,
            Amount = (double)amount,
            IsInternal = 1,
            DebitCodeName = string.Empty,
            DebitDetailCodeFirstName = string.Empty,
            DebitDetailCodeSecondName = string.Empty,
            CreditCodeName = string.Empty,
            CreditDetailCodeFirstName = string.Empty,
            CreditDetailCodeSecondName = string.Empty,
            DebitWarehouseName = string.Empty,
            CreditWarehouseName = string.Empty,
            Year = date.Year,
            Status = 1,
            Detail1 = string.Empty,
            Detail2 = string.Empty,
            CreateAt = DateTime.Now
        };
    }

    private static async Task<T> Required<T>(DbSet<T> set, int id, string message) where T : class
    {
        var entity = await set.FindAsync(id);
        return entity ?? throw new InvalidOperationException(message);
    }
}
