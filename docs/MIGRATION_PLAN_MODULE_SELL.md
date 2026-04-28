# Kế Hoạch Migrate Module Sell (Bán Hàng) từ Isoft sang AciPlatform

## 1. Tổng Quan (Overview)
Qua quá trình phân tích đối chiếu giữa mã nguồn `isoft` và `AciPlatform`, chúng ta thấy rằng `AciPlatform` đã có sẵn nền tảng của một số module như:
- **Khách hàng** (`Customer`, `CustomersController`)
- **Quản lý xe/Điều vận** (`FleetTransportation`)
- **Quản lý kho cơ bản** (`QLKho`, `GoodsController`, `OrderController` chủ yếu thao tác với `GoodWarehouses` và `GoodWarehouseExports`).

Tuy nhiên, **Module Sell (Bán hàng)** - trái tim của hệ thống thương mại với các logic phức tạp về hóa đơn (`Bill`), định mức hàng hóa (`GoodsQuota`), chương trình khuyến mãi (`GoodsPromotion`), và bảng giá (`PriceList`) - vẫn đang hoàn toàn vắng bóng trên `AciPlatform`. 

Tài liệu này trình bày kế hoạch chi tiết để chuyển đổi (migrate) Module Sell từ `isoft` sang `AciPlatform` theo tiêu chuẩn Clean Architecture hiện tại của AciPlatform.

---

## 2. Các Thành Phần Cần Migrate (Domain Entities)
Các thực thể dữ liệu (Entities) dưới đây chưa tồn tại trong `AciPlatform.Domain` và cần được đưa sang:

- **Quản lý Hóa Đơn & Đặt Hàng:**
  - `Bill.cs` (Hóa đơn bán hàng)
  - `Order.cs` (Đơn đặt hàng)
  - `OrderDetail.cs` (Chi tiết đơn hàng)
  - `OrderSuccessful.cs` (Trạng thái đơn hàng hoàn tất)
  - `Payer.cs` (Thông tin người thanh toán)

- **Danh mục & Chính sách Hàng Hóa:**
  - `Goods.cs` (Thông tin sản phẩm cốt lõi)
  - `GoodDetail.cs` (Chi tiết cấu hình sản phẩm)
  - `GoodsPriceList.cs` (Bảng giá theo đối tượng/thời điểm)
  - `GoodsPromotion.cs` & `GoodsPromotionDetail.cs` (Chương trình khuyến mãi)
  - `GoodCustomer.cs` (Giá/Chính sách riêng theo khách hàng)

- **Định mức Sản Xuất/Pha Chế (Quota):**
  - `GoodsQuota.cs`
  - `GoodsQuotaDetail.cs`
  - `GoodsQuotaRecipe.cs` (Công thức)
  - `GoodsQuotaStep.cs`

---

## 3. Lớp Xử Lý Nghiệp Vụ (Application Services)
Trong `AciPlatform.Application/Services`, chúng ta sẽ bổ sung một thư mục `Sell` để chứa các Service quan trọng từ Isoft:

1. **`BillService.cs`**: Xử lý logic tạo mới hóa đơn (chốt đơn), tính toán thuế, cập nhật công nợ và hoàn tác hóa đơn.
2. **`GoodsService.cs`**: Quản lý danh mục hàng hóa, bảng giá, định mức (Quota) và cấu hình sản phẩm.
3. **`BillPromotionService.cs`**: Áp dụng tự động các chương trình khuyến mãi, chiết khấu lên hóa đơn.
4. **`BillReporter.cs` / `BillForSaleReporter.cs`**: Sinh các báo cáo doanh thu, công nợ, hiệu suất bán hàng.
5. **`GoodPriceListService.cs`**: Engine tính toán giá động (Dynamic Pricing) dựa trên đối tượng khách hàng và thời điểm.
6. **`OrderService.cs`**: Quản lý vòng đời của Đơn đặt hàng (Từ lúc lên đơn qua Web/App đến lúc chốt thành Bill).

*Lưu ý:* Các Service này sẽ được refactor lại (chỉnh sửa namespace, thay đổi cách tiêm DbContext) để phù hợp với Interface `IApplicationDbContext` của AciPlatform.

---

## 4. API & Controllers (Presentation)
Bổ sung và mở rộng các Controllers trong `AciPlatform.Api/Controllers`:

- **Thêm mới `BillsController.cs`:** 
  - `POST /api/Bills/create` - Chốt đơn và tạo hóa đơn.
  - `PUT /api/Bills/update` - Cập nhật trạng thái thanh toán.
  - `POST /api/Bills/create-invoice` - Tích hợp phát hành hóa đơn điện tử.
  - `GET /api/Bills/get-bill-pdf` - Xuất file PDF hóa đơn.

- **Cập nhật `GoodsController.cs` (Đã có sẵn một phần):**
  - Mở rộng thêm các API liên quan đến chi tiết cấu hình hàng hóa (`/get-detail/{id}`).
  - Bổ sung API cập nhật và quản lý bảng giá (`/update-price`).

- **Cập nhật `OrderController.cs` (Đã có sẵn một phần):**
  - Mở rộng để hỗ trợ API duyệt đơn, chuyển đổi từ Order sang Bill thực tế.

---

## 5. Lộ Trình Triển Khai Thực Tế (Implementation Steps)

1. **Giai đoạn 1: Chuẩn bị Domain & Database (Ngày 1)**
   - Copy các class Entity từ `ModuleSell/Domain` (Isoft) sang `AciPlatform.Domain/Entities/Sell`.
   - Khai báo các DbSet mới vào `IApplicationDbContext` và `ApplicationDbContext`.
   - Tạo Migration (`Add-Migration AddModuleSell`) và cập nhật Database.

2. **Giai đoạn 2: Migrate Application Layer (Ngày 2 - Ngày 3)**
   - Copy các DTOs (Data Transfer Objects), Request/Response Models.
   - Chuyển đổi các lớp Service (`BillService`, `GoodsService`, v.v.).
   - Fix các lỗi liên quan đến Dependency Injection và tham chiếu thư viện chéo.

3. **Giai đoạn 3: Migrate Controllers & Routing (Ngày 4)**
   - Khởi tạo `BillsController.cs`.
   - Bổ sung các Endpoints còn thiếu vào `GoodsController` và `OrderController`.
   - Gắn các Attribute Phân quyền (`[Authorize]`, kiểm tra Role) theo chuẩn của AciPlatform.

4. **Giai đoạn 4: Testing & Tối ưu (Ngày 5)**
   - Kiểm thử luồng tạo đơn hàng -> Thanh toán hóa đơn -> Trừ kho.
   - Đồng bộ hóa logic của `Module Sell` với `Module QLKho` (Quản lý kho hiện tại trên AciPlatform).

---
*Vui lòng xem xét kế hoạch trên. Nếu anh đồng ý với hướng đi này, tôi sẽ tiến hành bắt đầu từ **Giai đoạn 1 (Migrate Domain Entities & DbContext)** ngay trong bước tiếp theo.*
