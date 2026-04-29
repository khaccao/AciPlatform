using AciPlatform.Domain.Entities.Ledger;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AciPlatform.Infrastructure.Persistence.Seeders;

public static class ChartOfAccountSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, int year)
    {
        if (await context.ChartOfAccounts.AnyAsync(c => c.Year == year))
            return; // Đã có dữ liệu thì bỏ qua

        var accounts = new List<ChartOfAccount>
        {
            // LOẠI TÀI KHOẢN TÀI SẢN (1, 2)
            new() { Code = "111", Name = "Tiền mặt", Type = 1, AccGroup = 1, HasChild = true },
            new() { Code = "1111", Name = "Tiền Việt Nam", ParentRef = "111", Type = 1, AccGroup = 1 },
            new() { Code = "1112", Name = "Ngoại tệ", ParentRef = "111", Type = 1, AccGroup = 1, IsForeignCurrency = true },
            new() { Code = "112", Name = "Tiền gửi Ngân hàng", Type = 1, AccGroup = 1, HasChild = true },
            new() { Code = "1121", Name = "Tiền Việt Nam", ParentRef = "112", Type = 1, AccGroup = 1 },
            new() { Code = "1122", Name = "Ngoại tệ", ParentRef = "112", Type = 1, AccGroup = 1, IsForeignCurrency = true },
            new() { Code = "131", Name = "Phải thu của khách hàng", Type = 1, AccGroup = 1, HasDetails = true, HasChild = true },
            new() { Code = "133", Name = "Thuế GTGT được khấu trừ", Type = 1, AccGroup = 1, HasChild = true },
            new() { Code = "1331", Name = "Thuế GTGT được khấu trừ của hàng hóa, dịch vụ", ParentRef = "133", Type = 1, AccGroup = 1 },
            new() { Code = "141", Name = "Tạm ứng", Type = 1, AccGroup = 1, HasDetails = true },
            new() { Code = "152", Name = "Nguyên liệu, vật liệu", Type = 1, AccGroup = 1 },
            new() { Code = "153", Name = "Công cụ, dụng cụ", Type = 1, AccGroup = 1 },
            new() { Code = "154", Name = "Chi phí sản xuất, kinh doanh dở dang", Type = 1, AccGroup = 1 },
            new() { Code = "155", Name = "Thành phẩm", Type = 1, AccGroup = 1 },
            new() { Code = "156", Name = "Hàng hóa", Type = 1, AccGroup = 1, HasChild = true },
            new() { Code = "1561", Name = "Giá mua hàng hóa", ParentRef = "156", Type = 1, AccGroup = 1 },
            new() { Code = "1562", Name = "Chi phí thu mua hàng hóa", ParentRef = "156", Type = 1, AccGroup = 1 },
            new() { Code = "211", Name = "Tài sản cố định hữu hình", Type = 1, AccGroup = 2, HasChild = true },
            new() { Code = "2111", Name = "Nhà cửa, vật kiến trúc", ParentRef = "211", Type = 1, AccGroup = 2 },
            new() { Code = "2112", Name = "Máy móc, thiết bị", ParentRef = "211", Type = 1, AccGroup = 2 },
            new() { Code = "214", Name = "Hao mòn tài sản cố định", Type = 1, AccGroup = 2, HasChild = true },
            new() { Code = "2141", Name = "Hao mòn TSCĐ hữu hình", ParentRef = "214", Type = 1, AccGroup = 2 },
            new() { Code = "242", Name = "Chi phí trả trước", Type = 1, AccGroup = 2 },

            // LOẠI TÀI KHOẢN NỢ PHẢI TRẢ (3)
            new() { Code = "331", Name = "Phải trả cho người bán", Type = 2, AccGroup = 3, HasDetails = true },
            new() { Code = "333", Name = "Thuế và các khoản phải nộp Nhà nước", Type = 2, AccGroup = 3, HasChild = true },
            new() { Code = "3331", Name = "Thuế giá trị gia tăng phải nộp", ParentRef = "333", Type = 2, AccGroup = 3, HasChild = true },
            new() { Code = "33311", Name = "Thuế GTGT đầu ra", ParentRef = "3331", Type = 2, AccGroup = 3 },
            new() { Code = "3334", Name = "Thuế thu nhập doanh nghiệp", ParentRef = "333", Type = 2, AccGroup = 3 },
            new() { Code = "3335", Name = "Thuế thu nhập cá nhân", ParentRef = "333", Type = 2, AccGroup = 3 },
            new() { Code = "334", Name = "Phải trả người lao động", Type = 2, AccGroup = 3, HasChild = true },
            new() { Code = "3341", Name = "Phải trả công nhân viên", ParentRef = "334", Type = 2, AccGroup = 3 },
            new() { Code = "338", Name = "Phải trả, phải nộp khác", Type = 2, AccGroup = 3, HasChild = true },
            new() { Code = "3383", Name = "Bảo hiểm xã hội", ParentRef = "338", Type = 2, AccGroup = 3 },
            new() { Code = "3384", Name = "Bảo hiểm y tế", ParentRef = "338", Type = 2, AccGroup = 3 },
            new() { Code = "341", Name = "Vay và nợ thuê tài chính", Type = 2, AccGroup = 3 },

            // LOẠI TÀI KHOẢN VỐN CHỦ SỞ HỮU (4)
            new() { Code = "411", Name = "Vốn đầu tư của chủ sở hữu", Type = 2, AccGroup = 4, HasChild = true },
            new() { Code = "4111", Name = "Vốn góp của chủ sở hữu", ParentRef = "411", Type = 2, AccGroup = 4 },
            new() { Code = "421", Name = "Lợi nhuận sau thuế chưa phân phối", Type = 2, AccGroup = 4, HasChild = true },
            new() { Code = "4211", Name = "Lợi nhuận sau thuế chưa phân phối năm trước", ParentRef = "421", Type = 2, AccGroup = 4 },
            new() { Code = "4212", Name = "Lợi nhuận sau thuế chưa phân phối năm nay", ParentRef = "421", Type = 2, AccGroup = 4 },

            // LOẠI TÀI KHOẢN DOANH THU (5, 7)
            new() { Code = "511", Name = "Doanh thu bán hàng và cung cấp dịch vụ", Type = 3, AccGroup = 5, HasChild = true },
            new() { Code = "5111", Name = "Doanh thu bán hàng hóa", ParentRef = "511", Type = 3, AccGroup = 5 },
            new() { Code = "5112", Name = "Doanh thu bán các thành phẩm", ParentRef = "511", Type = 3, AccGroup = 5 },
            new() { Code = "5113", Name = "Doanh thu cung cấp dịch vụ", ParentRef = "511", Type = 3, AccGroup = 5 },
            new() { Code = "515", Name = "Doanh thu hoạt động tài chính", Type = 3, AccGroup = 5 },
            new() { Code = "711", Name = "Thu nhập khác", Type = 3, AccGroup = 7 },

            // LOẠI TÀI KHOẢN CHI PHÍ (6, 8)
            new() { Code = "621", Name = "Chi phí nguyên liệu, vật liệu trực tiếp", Type = 4, AccGroup = 6 },
            new() { Code = "622", Name = "Chi phí nhân công trực tiếp", Type = 4, AccGroup = 6 },
            new() { Code = "627", Name = "Chi phí sản xuất chung", Type = 4, AccGroup = 6 },
            new() { Code = "632", Name = "Giá vốn hàng bán", Type = 4, AccGroup = 6 },
            new() { Code = "641", Name = "Chi phí bán hàng", Type = 4, AccGroup = 6, HasChild = true },
            new() { Code = "6411", Name = "Chi phí nhân viên", ParentRef = "641", Type = 4, AccGroup = 6 },
            new() { Code = "642", Name = "Chi phí quản lý doanh nghiệp", Type = 4, AccGroup = 6, HasChild = true },
            new() { Code = "6421", Name = "Chi phí nhân viên quản lý", ParentRef = "642", Type = 4, AccGroup = 6 },
            new() { Code = "811", Name = "Chi phí khác", Type = 4, AccGroup = 8 },
            new() { Code = "821", Name = "Chi phí thuế thu nhập doanh nghiệp", Type = 4, AccGroup = 8 },

            // TÀI KHOẢN XÁC ĐỊNH KẾT QUẢ KINH DOANH (9)
            new() { Code = "911", Name = "Xác định kết quả kinh doanh", Type = 3, AccGroup = 9 }
        };

        foreach (var acc in accounts)
        {
            acc.Year = year;
            acc.IsInternal = 1; // Sổ thuế
            acc.Duration = "12";
            acc.DisplayInsert = false;
            acc.DisplayDelete = false;

            // Clone cho Sổ nội bộ
            var accNB = new ChartOfAccount
            {
                Code = acc.Code,
                Name = acc.Name,
                ParentRef = acc.ParentRef,
                Type = acc.Type,
                AccGroup = acc.AccGroup,
                HasChild = acc.HasChild,
                HasDetails = acc.HasDetails,
                Year = year,
                IsInternal = 2, // Sổ nội bộ
                Duration = "12",
                DisplayInsert = false,
                DisplayDelete = false
            };
            
            context.ChartOfAccounts.Add(acc);
            context.ChartOfAccounts.Add(accNB);
        }

        await context.SaveChangesAsync();
    }
}
