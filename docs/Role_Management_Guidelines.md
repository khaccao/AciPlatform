# Tài liệu Hướng dẫn Nghiệp vụ: Vai trò & Phân quyền (Role & User Management)

Tài liệu này mô tả chi tiết kịch bản hoạt động, tính năng, và hướng dẫn thao tác chuyên sâu về phân quyền và quản lý người dùng đối với 2 vai trò cốt lõi trong nền tảng AciPlatform: **SuperAdmin** và **ADMINCOMPANY**.

---

## 1. Tổng quan Kiến trúc Vai trò (Role Architecture)

Hệ thống được thiết kế theo mô hình **Multi-Tenant (Đa khách hàng)**, trong đó dữ liệu của mỗi công ty/tổ chức được cô lập hoàn toàn với nhau thông qua mã `CompanyCode`.

*   **SuperAdmin**: Quản trị viên cấp cao nhất của nền tảng (Platform Operator). Có góc nhìn "God-eye" toàn cục.
*   **ADMINCOMPANY**: Quản trị viên cấp thuê bao (Tenant Admin). Chỉ có quyền hạn thao tác trong ranh giới tổ chức/công ty của mình.

---

## 2. Kịch bản và Vận hành đối với: SuperAdmin

### Ý nghĩa và Tầm quan trọng
SuperAdmin là đại diện của đơn vị cung cấp phần mềm. Vai trò này là khởi nguồn của mọi luồng dữ liệu, chịu trách nhiệm thiết lập các yếu tố không gian mạng ban đầu cho khách hàng và khắc phục các sự cố ở mức hệ thống (System-level).

### Tính năng và Thao tác

#### A. Quản lý Không gian Khách hàng (Company Management)
*   **Tính năng**: Tạo mới, thiết lập cấu hình, khóa hoặc mở khóa một Công ty (Tenant) trên nền tảng.
*   **Thao tác**: Chức năng Tạo Công ty -> Nhập thông tin (Tên công ty, domain, package...) -> Hệ thống sinh ra một `CompanyCode` duy nhất.

#### B. Phân quyền và Tạo người dùng (Góc độ SuperAdmin)
Đây là thao tác "trao chìa khóa" cho khách hàng.
*   **Thao tác**: 
    1. Truy cập Module Quản lý Người dùng toàn cục.
    2. Chọn **Thêm mới User**.
    3. Nhập thông tin tài khoản cho đại diện của công ty khách hàng.
    4. Gán Role là `ADMINCOMPANY`.
    5. **[Rất Quan Trọng]** Gán / Map chính xác giá trị `CompanyCode` (của công ty vừa tạo ở trên) cho user này.
*   **Ý nghĩa**: Hành động gán `CompanyCode` chính là thao tác phân lập dữ liệu. Kể từ lúc này, tài khoản `ADMINCOMPANY` đó bị "giam" trong phạm vi dữ liệu của công ty họ. Không có `CompanyCode`, hệ thống sẽ không biết user đó thuộc về ai.

#### C. Kịch bản vận hành thực tế (Use-Cases)
1.  **Onboarding Mới**: Khách hàng A ký hợp đồng sử dụng phần mềm. SuperAdmin thao tác: Tạo `Company A` -> Tạo user `NguyenVanA` với role `ADMINCOMPANY` & gắn mã `Company A` -> Bàn giao tài khoản cho anh A.
2.  **Hỗ trợ Kỹ thuật (Troubleshooting)**: Khách hàng báo lỗi không thể tạo phòng ban. SuperAdmin dùng quyền cao nhất của mình, truy cập vào hệ thống, lọc dữ liệu theo `CompanyCode` của khách hàng đó để kiểm tra ngọn ngành nguyên nhân do thiết lập sai hay do bug hệ thống, chức năng mà bản thân khách hàng không thể tự làm.

---

## 3. Kịch bản và Vận hành đối với: ADMINCOMPANY

### Ý nghĩa và Tầm quan trọng
ADMINCOMPANY (Giám đốc nhân sự / Trưởng phòng IT nội bộ) là người quản lý toàn quyền trong "Vương quốc" của họ. Mọi thao tác của role này đều bị giới hạn dữ liệu, tuân thủ nguyên tắc Multi-Tenant Data Isolation (cô lập dữ liệu).

### Tính năng và Thao tác CHUYÊN SÂU: Phân quyền & Tạo User

#### A. Thiết lập Cơ cấu tổ chức (Department & Position)
*   **Tính năng**: Định nghĩa kiến trúc tổ chức nội bộ của công ty.
*   **Ý nghĩa**: Trước khi tạo được nhân viên, phải có "Chỗ" để nhân viên đó ngồi. ADMINCOMPANY sẽ tạo các Phòng Ban (Departments) và Chức Vụ (Positions). Toàn bộ dữ liệu này khi lưu xuống Database sẽ **tự động** được hệ thống gán `CompanyCode` của Admin hiện tại vào (chỉ chạy ẩn dưới Backend, Admin không cần thấy).

#### B. Tạo người dùng và Cấp Role (User Onboarding)
*   **Luồng thao tác**:
    1. Truy cập Quản lý Người dùng (Nội bộ công ty).
    2. Điền thông tin nhân viên (Tên, Email, SĐT...).
    3. Chọn **Phòng ban** và **Chức vụ** (Danh sách dropdown này chỉ load các phòng ban thuộc công ty của họ).
    4. Gán **Roles** (ví dụ: `EMPLOYEE`, `HR_MANAGER`).
*   **Điểm khác biệt cốt lõi (Security)**: Trên màn hình tạo User của ADMINCOMPANY **sẽ không có** trường chọn `CompanyCode`. Backend sẽ tự động trích xuất `CompanyCode` từ token đăng nhập của ADMINCOMPANY và gán cứng cho User mới. Điều này ngăn chặn hoàn toàn việc Admin công ty này tạo user tống sang công ty khác (Bảo mật Data Isolation).

#### C. Quản lý luân chuyển và Phân quyền mở rộng
*   **Tính năng**: Ủy quyền (Delegation).
*   **Kịch bản**: Giám đốc công ty (đang là ADMINCOMPANY) muốn giao lại việc thêm user cho Trưởng phòng HR. Giám đốc sẽ tạo tài khoản Trưởng phòng HR, gán thêm role `HR_ADMIN`. Hệ thống lúc này chỉ cho phép `HR_ADMIN` tạo/sửa người dùng với các role thấp hơn (như `EMPLOYEE`).

#### D. Kịch bản vận hành thực tế (Use-Cases)
1.  **Tiếp nhận nhân sự mới**: Tuyển được 10 nhân viên Sales. ADMINCOMPANY vào hệ thống tạo 10 tài khoản, phân bổ vào Phòng Kinh Doanh, gán role `EMPLOYEE` để họ có thể đăng nhập báo cáo công việc/chấm công.
2.  **Luân chuyển bộ phận**: Một nhân sự được thăng chức từ Nhân viên lên Trưởng phòng. ADMINCOMPANY vào chỉnh sửa Profile user này, đổi Position bằng "Trưởng phòng" và có thể gán thêm Role quản lý để user đó duyệt được phép của cấp dưới.
3.  **Nhân viên nghỉ việc (Offboarding)**: Khi nhân viên nghỉ, thao tác chuẩn của ADMINCOMPANY là **Deactivate (Khóa/Vô hiệu hóa)** tài khoản thay vì xóa cứng (Delete). Ý nghĩa: Vô hiệu hóa thì user không đăng nhập được nữa, nhưng toàn bộ lịch sử thao tác, lương, chấm công trong quá khứ vẫn được giữ nguyên tính toàn vẹn.

---

## 4. Bảng Đối chiếu Ma trận Quyền hạn (Authorization Matrix)

Tóm tắt sự khác biệt cơ bản về bảo mật hệ thống giữa 2 Role nhằm đảm bảo tính toàn vẹn nền tảng SaaS:

| Hạng mục / Nghiệp vụ | SuperAdmin (Platform) | ADMINCOMPANY (Tenant) | Ghi chú & Ý nghĩa |
| :--- | :--- | :--- | :--- |
| **Phạm vi hiển thị dữ liệu** | **Toàn cục** (Thấy tất cả) | **Cục bộ** (Chỉ thấy dữ liệu có cùng `CompanyCode`) | Hệ thống dùng `CompanyCode` làm bộ lọc bắt buộc ở mọi API. |
| **Tạo/Sửa cơ cấu Công ty** | Được phép | Đọc/Xem thông tin công ty mình. Không thể cấu hình cấp hệ thống. | SuperAdmin nắm quyền sinh sát của Tenant. |
| **Quản lý Tổ chức (Phòng ban/Chức vụ)** | Thao tác trên mọi công ty. (Cần chọn công ty đích). | Chỉ tạo/sửa cho công ty của mình. (Không hiển thị CTY khác). | Tự động hóa Gán mã công ty ở Backend. |
| **Trường "CompanyCode" khi tạo User** | **Hiển thị** (Bắt buộc SuperAdmin phải chọn gán cho ai). | **Bị ẩn hoàn toàn** (Hệ thống tự động kế thừa từ Admin). | Ngăn chặn rủi ro tấn công leo thang đặc quyền (Privilege escalation). |
| **Quản lý Role / Phân quyền** | Quản lý cả System Roles và Tenant Roles. | Chỉ quản lý việc gán Tenant Roles cho các User nội bộ. | ADMINCOMPANY không thể cấu hình hệ thống ảnh hưởng Tenant khác. |
| **Tạo mới tài khoản Quản trị** | Có thể tạo SuperAdmin & ADMINCOMPANY mới. | Không thể tạo SuperAdmin. (Chỉ tạo thêm tài khoản quản lý nội bộ). | Đảm bảo hệ thống trung tâm không bị xâm phạm. |
