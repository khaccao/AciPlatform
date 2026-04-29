using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.Ledger;
using AciPlatform.Domain.Entities.Sell;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AciPlatform.Application.Services.Ledger
{
    public interface ICustomerLedgerIntegrationService
    {
        Task SyncCustomerToChartOfAccountAsync(Customer customer, int year);
        Task AutoAccountSalesInvoiceAsync(Order order, int year, int isInternal = 1);
        Task AutoAccountPaymentReceiptAsync(int customerId, double amount, string paymentMethod, int year, int isInternal = 1);
        Task<double> GetCustomerDebtBalanceAsync(int customerId, int year, int isInternal = 1);
    }

    public class CustomerLedgerIntegrationService : ICustomerLedgerIntegrationService
    {
        private readonly IApplicationDbContext _context;

        public CustomerLedgerIntegrationService(IApplicationDbContext context)
        {
            _context = context;
        }

        // BÆ°á»›c 1: Khá»Ÿi táº¡o vĂ  Quáº£n lĂ½ Há»“ sÆ¡ (CRM - KhĂ¡ch HĂ ng) -> Äáº©y vĂ o Danh má»¥c TK (131.KH0001)
        public async Task SyncCustomerToChartOfAccountAsync(Customer customer, int year)
        {
            var parentAccountCode = "131"; // Pháº£i thu khĂ¡ch hĂ ng (TĂ i khoáº£n gá»‘c)
            var customerAccountCode = $"{parentAccountCode}.{customer.Code}";

            var existingAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(c => c.Code == customerAccountCode && c.Year == year);

            if (existingAccount == null)
            {
                var newAccount = new ChartOfAccount
                {
                    Code = customerAccountCode,
                    Name = customer.Name,
                    ParentRef = parentAccountCode,
                    HasChild = false,
                    HasDetails = true,
                    Type = 1, // Loáº¡i tĂ i khoáº£n cĂ´ng ná»£
                    Year = year,
                    DisplayInsert = false,
                    DisplayDelete = false,
                    Duration = "12",
                    AccGroup = 1 // NhĂ³m tĂ i sáº£n
                };
                
                _context.ChartOfAccounts.Add(newAccount);
            }
            else
            {
                existingAccount.Name = customer.Name;
                _context.ChartOfAccounts.Update(existingAccount);
            }

            // Äáº£m báº£o TK máº¹ (131) Ä‘Æ°á»£c Ä‘Ă¡nh dáº¥u lĂ  HasChild = true
            var parentAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(c => c.Code == parentAccountCode && c.Year == year);
            if (parentAccount != null && !parentAccount.HasChild)
            {
                parentAccount.HasChild = true;
                _context.ChartOfAccounts.Update(parentAccount);
            }

            await _context.SaveChangesAsync();
        }

        // BÆ°á»›c 4: Háº¡ch toĂ¡n Tá»± Ä‘á»™ng Ghi nháº­n Doanh thu & CĂ´ng ná»£, GiĂ¡ vá»‘n & Kho
        public async Task AutoAccountSalesInvoiceAsync(Order order, int year, int isInternal = 1)
        {
            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null) throw new Exception("Customer not found");

            var customerAccountCode = $"131.{customer.Code}";
            var revenueAccountCode = "511"; // Doanh thu
            var taxAccountCode = "33311";   // Thuáº¿ GTGT Ä‘áº§u ra
            var cogsAccountCode = "632";    // GiĂ¡ vá»‘n hĂ ng bĂ¡n
            var inventoryAccountCode = "156"; // HĂ ng hĂ³a

            var currentDate = DateTime.Now;

            // 1. Ghi nháº­n Doanh thu & CĂ´ng ná»£ (Ná»£ 131, CĂ³ 511, CĂ³ 33311)
            var revenueAmount = (double)order.TotalPricePaid; // Giáº£ sá»­ Tá»•ng tiá»n Ä‘Ă£ bao gá»“m thuáº¿
            var taxAmount = revenueAmount * 0.1; // VAT 10% giáº£ Ä‘á»‹nh
            var netRevenue = revenueAmount - taxAmount;

            var ledgerRevenue = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "BanHang",
                Month = currentDate.Month,
                BookDate = currentDate,
                OrginalVoucherNumber = $"HD-{order.Id.ToString()}",
                OrginalBookDate = currentDate,
                OrginalDescription = $"Xuáº¥t hĂ³a Ä‘Æ¡n bĂ¡n hĂ ng cho Ä‘Æ¡n {order.Id.ToString()}",
                DebitCode = customerAccountCode,
                CreditCode = revenueAccountCode,
                Detail1 = customerAccountCode,
                Amount = netRevenue,
                IsInternal = isInternal,
                Year = year
            };

            var ledgerTax = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "BanHang",
                Month = currentDate.Month,
                BookDate = currentDate,
                OrginalVoucherNumber = $"HD-{order.Id.ToString()}",
                OrginalBookDate = currentDate,
                OrginalDescription = $"Thuáº¿ GTGT Ä‘áº§u ra cho Ä‘Æ¡n {order.Id.ToString()}",
                DebitCode = customerAccountCode,
                CreditCode = taxAccountCode,
                Amount = taxAmount,
                IsInternal = isInternal,
                Year = year
            };

            // 2. Ghi nháº­n GiĂ¡ vá»‘n (Ná»£ 632, CĂ³ 156)
            var cogsAmount = netRevenue * 0.7; // Giáº£ sá»­ giĂ¡ vá»‘n = 70% doanh thu thuáº§n

            var ledgerCogs = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "XuatKho",
                Month = currentDate.Month,
                BookDate = currentDate,
                OrginalVoucherNumber = $"XK-{order.Id.ToString()}",
                OrginalBookDate = currentDate,
                OrginalDescription = $"Xuáº¥t kho bĂ¡n hĂ ng Ä‘Æ¡n {order.Id.ToString()}",
                DebitCode = cogsAccountCode,
                CreditCode = inventoryAccountCode,
                Amount = cogsAmount,
                IsInternal = isInternal,
                Year = year
            };

            _context.Set<Domain.Entities.Ledger.LedgerEntry>().AddRange(ledgerRevenue, ledgerTax, ledgerCogs);
            await _context.SaveChangesAsync();
        }

        // BÆ°á»›c 5: Theo dĂµi Thanh toĂ¡n cĂ´ng ná»£ (Láº­p Phiáº¿u Thu)
        public async Task AutoAccountPaymentReceiptAsync(int customerId, double amount, string paymentMethod, int year, int isInternal = 1)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) throw new Exception("Customer not found");

            var customerAccountCode = $"131.{customer.Code}";
            var cashAccountCode = paymentMethod == "Cash" ? "111" : "112";

            var currentDate = DateTime.Now;

            var ledgerReceipt = new Domain.Entities.Ledger.LedgerEntry
            {
                Type = "PhieuThu",
                Month = currentDate.Month,
                BookDate = currentDate,
                OrginalVoucherNumber = $"PT-{DateTime.Now.Ticks}",
                OrginalBookDate = currentDate,
                OrginalDescription = $"KhĂ¡ch hĂ ng {customer.Name} thanh toĂ¡n",
                DebitCode = cashAccountCode,
                CreditCode = customerAccountCode,
                Detail2 = customerAccountCode,
                Amount = amount,
                IsInternal = isInternal,
                Year = year
            };

            _context.Set<Domain.Entities.Ledger.LedgerEntry>().Add(ledgerReceipt);
            await _context.SaveChangesAsync();
        }

        // Truy xuáº¥t Tá»•ng dÆ° ná»£ hiá»‡n táº¡i (DĂ nh cho Module Customer hiá»ƒn thá»‹)
        public async Task<double> GetCustomerDebtBalanceAsync(int customerId, int year, int isInternal = 1)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return 0;

            var customerAccountCode = $"131.{customer.Code}";

            // DÆ° ná»£ = Tá»•ng Ná»£ (ÄĂ£ mua) - Tá»•ng CĂ³ (ÄĂ£ tráº£) + Sá»‘ dÆ° Ä‘áº§u ká»³ (náº¿u cĂ³)
            var ledgers = await _context.Set<Domain.Entities.Ledger.LedgerEntry>()
                .Where(l => l.Year == year && l.IsInternal == isInternal)
                .Where(l => l.DebitCode == customerAccountCode || l.CreditCode == customerAccountCode)
                .ToListAsync();

            var totalDebit = ledgers.Where(l => l.DebitCode == customerAccountCode).Sum(l => l.Amount);
            var totalCredit = ledgers.Where(l => l.CreditCode == customerAccountCode).Sum(l => l.Amount);

            var chartOfAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(c => c.Code == customerAccountCode && c.Year == year);
            var openingDebit = chartOfAccount?.OpeningDebit ?? 0.0;
            var openingCredit = chartOfAccount?.OpeningCredit ?? 0.0;

            if (isInternal == 2) // Náº¿u lĂ  sá»• ná»™i bá»™
            {
                openingDebit = chartOfAccount?.OpeningDebitNB ?? 0.0;
                openingCredit = chartOfAccount?.OpeningCreditNB ?? 0.0;
            }

            return (openingDebit + totalDebit) - (openingCredit + totalCredit);
        }
    }
}









