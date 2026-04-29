using AciPlatform.Application.Interfaces;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Domain.Entities.Ledger;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AciPlatform.Application.Services.Ledger
{
    public interface IInternalAccountingService
    {
        Task<long> CreatePaymentVoucherAsync(PaymentVoucherRequestModel request, int year);
        Task<bool> ApprovePaymentVoucherAsync(ApproveVoucherRequestModel request, string approverName);
        Task<long> CreateWarehouseReceiptAsync(WarehouseReceiptRequestModel request, int year);
    }

    public class InternalAccountingService : IInternalAccountingService
    {
        private readonly IApplicationDbContext _context;

        public InternalAccountingService(IApplicationDbContext context)
        {
            _context = context;
        }

        // Nhân viên / Kế toán viên lập phiếu chi
        public async Task<long> CreatePaymentVoucherAsync(PaymentVoucherRequestModel request, int year)
        {
            // Trạng thái ban đầu có thể là "Chờ duyệt" (Ví dụ Dùng cột ReferenceVoucherNumber để check)
            var ledgerEntry = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "PhieuChi", // Phiếu chi
                Month = request.VoucherDate.Month,
                BookDate = request.VoucherDate,
                OrginalVoucherNumber = $"PC-{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                OrginalBookDate = request.VoucherDate,
                OrginalDescription = $"Chi tiền cho {request.ReceiverName} - Lý do: {request.Reason}",
                DebitCode = request.DebitAccount, 
                CreditCode = request.CreditAccount,
                Amount = request.Amount,
                IsInternal = request.IsInternal,
                Year = year,
                // Đánh dấu là chưa duyệt
                Status = 0, 
                Detail1 = request.ReceiverName
            };

            // Nếu chi cho NCC cụ thể (công nợ 331)
            if (request.SupplierId.HasValue && request.DebitAccount.StartsWith("331"))
            {
                var supplier = await _context.Customers.FindAsync(request.SupplierId.Value);
                if (supplier != null)
                {
                    ledgerEntry.DebitCode = $"331.{supplier.Code}";
                    ledgerEntry.Detail2 = supplier.Code ?? "";
                }
            }

            _context.Set<Domain.Entities.Ledger.LedgerEntry>().Add(ledgerEntry);
            await _context.SaveChangesAsync();

            return ledgerEntry.Id;
        }

        // Kế toán trưởng duyệt phiếu chi
        public async Task<bool> ApprovePaymentVoucherAsync(ApproveVoucherRequestModel request, string approverName)
        {
            var ledger = await _context.Set<Domain.Entities.Ledger.LedgerEntry>().FindAsync(request.LedgerId);
            if (ledger == null) return false;

            if (request.IsApproved)
            {
                ledger.Status = 1; // Đã duyệt, lúc này mới tính vào sổ cái chính thức
                ledger.OrginalDescription += $" (Đã duyệt bởi {approverName}. Ghi chú: {request.Note})";
            }
            else
            {
                ledger.Status = -1; // Từ chối
                ledger.OrginalDescription += $" (Từ chối bởi {approverName}. Ghi chú: {request.Note})";
            }

            _context.Set<Domain.Entities.Ledger.LedgerEntry>().Update(ledger);
            await _context.SaveChangesAsync();

            return true;
        }

        // Nhân viên kho lập phiếu nhập kho báo lên cho kế toán
        public async Task<long> CreateWarehouseReceiptAsync(WarehouseReceiptRequestModel request, int year)
        {
            var supplier = await _context.Customers.FindAsync(request.SupplierId);
            if (supplier == null) throw new Exception("Supplier not found");

            var totalAmount = request.Items.Sum(i => i.Quantity * i.UnitPrice);
            
            // Hạch toán Nhập Kho (Nợ 156, Có 331)
            var ledgerReceipt = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "NhapKho",
                Month = request.ReceiptDate.Month,
                BookDate = request.ReceiptDate,
                OrginalVoucherNumber = request.ReceiptNumber,
                OrginalBookDate = request.ReceiptDate,
                OrginalDescription = $"Nhập kho từ NCC {supplier.Name} - {request.Note}",
                DebitCode = "156", // Tài khoản Hàng hóa
                CreditCode = $"331.{supplier.Code}", // Phải trả người bán
                Detail2 = supplier.Code ?? "",
                Amount = totalAmount,
                IsInternal = request.IsInternal,
                Year = year,
                Status = 1 // Nhập kho xong thì ghi công nợ luôn
            };

            // Lưu chi tiết từng mặt hàng vào kho
            // Tương tự, nếu dự án có bảng WarehouseLedger / InventoryLedger thì ghi vào đây
            // ... (Logic Insert Warehouse details)

            _context.Set<Domain.Entities.Ledger.LedgerEntry>().Add(ledgerReceipt);
            await _context.SaveChangesAsync();

            return ledgerReceipt.Id;
        }
    }
}






