# 📱 Module MultiChannel - Chi Tiết Tính Năng

---

## 🎯 **TỔNG QUAN MODULE**

Module **MultiChannel** là một hệ thống quản lý bán hàng tích hợp đa kênh (Multi-Channel Sales Platform), cho phép doanh nghiệp quản lý các hoạt động kinh doanh trên nhiều nền tảng xã hội như **Facebook** thông qua một giao diện duy nhất.

### **Mục Đích:**
- ✅ Kết nối với các fanpage Facebook của doanh nghiệp
- ✅ Quản lý bài viết, nội dung và tương tác khách hàng
- ✅ Tự động hóa các quy trình bán hàng
- ✅ Tạo hóa đơn/invoice tự động
- ✅ Phân tích hiệu suất bán hàng (Insights)

---

## 🔌 **FACEBOOK INTEGRATION - CÁC TÍNH NĂNG CHI TIẾT**

### **1. CẤU HÌNH & KẾT NỐI FACEBOOK**

#### **A. Quản Lý App Configuration**
```
📍 API Endpoint: GET/POST /api/v1/multichannel/app-config
```

**Tác dụng:**
- Lưu trữ Facebook App ID và App Secret
- Cơ sở để kết nối với Facebook Graph API
- Bảo mật thông tin API Facebook

**Dữ liệu:**
```json
{
  "appId": "1234567890",
  "appSecret": "abc123def456",
  "createdDate": "2026-02-24"
}
```

#### **B. OAuth Authentication (Xác Thực OAuth)**
```
📍 API Endpoint: GET /api/v1/multichannel/oauth-url
```

**Tác dụng:**
- Tạo URL xác thực Facebook (Login with Facebook)
- Yêu cầu quyền truy cập từ người dùng
- Được phép: đọc fanpage, quản lý bài viết, xem engagement

**Quyền Được Yêu Cầu (Scopes):**
- `pages_show_list` - Xem danh sách fanpage
- `pages_read_engagement` - Xem lượt tương tác
- `pages_manage_posts` - Đăng bài trên fanpage
- `pages_read_user_content` - Đọc nội dung người dùng
- `public_profile` - Xem thông tin profile công khai
- `email` - Truy cập email

#### **C. OAuth Callback Handler (Xử Lý Callback)**
```
📍 API Endpoint: POST /api/v1/multichannel/oauth-callback
```

**Quy Trình:**
1. **Bước 1:** Nhận authorization code từ Facebook
2. **Bước 2:** Trao đổi code lấy user access token
3. **Bước 3:** Chuyển đổi thành long-lived token (hiệu lực lâu dài)
4. **Bước 4:** Tự động lấy danh sách fanpage
5. **Bước 5:** Kết nối mỗi fanpage (auto-connect pages)

**Dữ liệu Trả Về:**
```json
{
  "message": "Facebook connected successfully",
  "userAccessToken": "long_lived_token_here",
  "pagesConnected": 3,
  "pages": [
    {
      "pageId": "103456789",
      "name": "Shop ABC",
      "accessToken": "page_access_token_1"
    }
  ]
}
```

---

### **2. QUẢN LÝ FANPAGE**

#### **A. Lấy Danh Sách Fanpage**
```
📍 API Endpoint: GET /api/v1/multichannel/pages
```

**Tác dụng:**
- Hiển thị tất cả fanpage đã kết nối
- Xem trạng thái kết nối của từng fanpage
- Thông tin chi tiết: Page ID, tên, access token

**Phản Hồi:**
```json
[
  {
    "id": 1,
    "userId": 1,
    "pageId": "103456789",
    "name": "Shop ABC",
    "accessToken": "EABC1234...",
    "status": "active",
    "connectedDate": "2026-02-24"
  }
]
```

#### **B. Kết Nối Fanpage Mới**
```
📍 API Endpoint: POST /api/v1/multichannel/connect-page
```

**Tác dụng:**
- Kết nối thêm fanpage mới (ngoài quy trình OAuth)
- Lưu trữ token truy cập fanpage
- Cho phép quản lý nhiều fanpage từ một tài khoản

**Request Body:**
```json
{
  "pageId": "103456789",
  "name": "Shop ABC",
  "accessToken": "EABC1234...",
  "userToken": "user_token_here"
}
```

#### **C. Ngắt Kết Nối Fanpage**
```
📍 API Endpoint: POST /api/v1/multichannel/disconnect-page/{pageId}
```

**Tác dụng:**
- Xóa kết nối fanpage khỏi hệ thống
- Ngăn chặn truy cập API tới fanpage đó
- Giải phóng token truy cập

---

### **3. QUẢN LÝ BÀI VIẾT (POST MANAGEMENT)**

#### **A. Tạo Bài Viết Với AI Auto-Generate**
```
📍 API Endpoint: POST /api/v1/multichannel/post
```

**Tác dụng:**
- Tạo bài viết trên fanpage
- **AI tự động generate nội dung** nếu được kích hoạt
- Hỗ trợ upload hình ảnh
- Lên lịch bài viết

**Request Body:**
```json
{
  "content": "Nội dung bài viết...",
  "autoGenerateContent": true,
  "aiPrompt": "Viết bài quảng cáo sản phẩm điện thoại giảm 50%",
  "imageUrls": [
    "https://example.com/image1.jpg",
    "https://example.com/image2.jpg"
  ],
  "scheduledTime": "2026-02-25T10:00:00Z",
  "pageId": 1
}
```

**Tính Năng:**
- ✅ **Manual Content** - Viết tay nội dung
- ✅ **AI Auto-Generate** - AI tạo nội dung tự động
- ✅ **Image Support** - Đính kèm nhiều hình ảnh
- ✅ **Schedule Posts** - Lên lịch đăng bài
- ✅ **Instant Publish** - Đăng ngay lập tức

**Phản Hồi:**
```json
{
  "id": 5,
  "userId": 1,
  "pageId": 1,
  "content": "Nội dung đã được AI generate...",
  "status": "Published",
  "publishedDate": "2026-02-24T15:30:00Z",
  "engagement": {
    "likes": 120,
    "comments": 25,
    "shares": 8
  }
}
```

#### **B. Đăng Bài Lên Facebook Trực Tiếp**
```
📍 API Endpoint: POST /api/v1/multichannel/publish
```

**Tác dụng:**
- Gửi bài viết trực tiếp lên Facebook Graph API
- Bypass hệ thống draft, đăng ngay lập tức
- Hỗ trợ text + hình ảnh

**Request Body:**
```json
{
  "pageId": 1,
  "message": "Khuyến mãi hôm nay: Mua 1 tặng 1!"
}
```

**Quy Trình:**
1. Lấy thông tin fanpage từ DB
2. Lấy access token
3. Gọi Facebook Graph API `/v18.0/{page_id}/feed`
4. Đăng bài viết
5. Trả về kết quả

---

### **4. PHÂN TÍCH & INSIGHTS**

#### **A. Lấy Thống Kê Fanpage**
```
📍 API Endpoint: GET /api/v1/multichannel/insights/{pageId}?metric=impressions
```

**Tác dụng:**
- Xem hiệu suất fanpage
- Thống kê lượng tiếp cận (Impressions)
- Phân tích engagement rate
- Theo dõi lượng followers tăng/giảm

**Các Metrics Có Sẵn:**
- `impressions` - Lượt hiển thị
- `engaged_users` - Người dùng tương tác
- `fan_count` - Số followers
- `post_impressions` - Hiển thị bài viết
- `post_engaged_users` - Người tương tác bài viết

**Phản Hồi:**
```json
{
  "pageId": 1,
  "metric": "impressions",
  "data": [
    {
      "date": "2026-02-24",
      "value": 5420,
      "change": "+12%"
    }
  ],
  "totalImpressions": 245600,
  "averageDaily": 5120
}
```

---

### **5. TỰ ĐỘNG HÓA (AUTOMATION)**

#### **A. Tạo Quy Trình Tự Động Hóa**
```
📍 API Endpoint: POST /api/v1/multichannel/automation
```

**Tác dụng:**
- Tạo workflow tự động hóa
- Kích hoạt hành động dựa trên sự kiện
- Tiết kiệm thời gian quản lý

**Request Body:**
```json
{
  "name": "Auto-Response for New Comments",
  "triggerType": "new_comment",
  "workflowJson": {
    "steps": [
      {
        "type": "trigger",
        "event": "comment_received",
        "condition": "contains_keyword('giá')"
      },
      {
        "type": "action",
        "action": "send_response",
        "message": "Cảm ơn bạn! Vui lòng inbox để được tư vấn chi tiết về giá."
      }
    ]
  }
}
```

**Ví Dụ Workflow:**
1. **Trigger:** Có bình luận mới trên bài viết
2. **Condition:** Bình luận chứa từ khóa "giá"
3. **Action:** Tự động trả lời với tin nhắn được cài đặt

#### **B. Lấy Danh Sách Quy Trình**
```
📍 API Endpoint: GET /api/v1/multichannel/automation
```

**Phản Hồi:**
```json
[
  {
    "id": 1,
    "name": "Auto-Response for New Comments",
    "triggerType": "new_comment",
    "status": "active",
    "createdDate": "2026-02-24",
    "executionCount": 145
  }
]
```

---

## 🤖 **AI SERVICE - TỰ ĐỘNG TẠO NỘI DUNG**

### **Tính Năng AI Content Generation**

```csharp
// Dịch vụ AI
IAIService.GenerateContentAsync(aiPrompt)
```

**Ví Dụ Sử Dụng:**
```json
{
  "aiPrompt": "Viết caption cho bài quảng cáo áo sơ mi nam màu đỏ, giá 250k, khuyến mãi 30%"
}
```

**Kết Quả AI Có Thể Tạo Ra:**
```
"🔥 SUPER SALE 🔥
Áo Sơ Mi Nam Màu Đỏ - CHẤT LƯỢNG PREMIUM
⭐ Giá Gốc: 250.000đ
⭐ KHUYẾN MÃI 30% = CHỈ 175.000đ
⭐ Chất liệu: 100% cotton, mềm mại, thoáng khí
⭐ Bảo hành: 12 tháng

🎁 Mua hôm nay - Giao ngay!
📲 Inbox để đặt hàng
💳 Thanh toán sau khi nhận hàng

#sale #giamgia #aosomi #nam #fashion"
```

---

## 🧾 **INVOICE AUTHORIZATION - HÓA ĐƠN & THANH TOÁN**

### **Tính Năng Tạo & Phê Duyệt Hóa Đơn**

```
📍 API Endpoint: POST /api/auth/invoice
```

**Tác dụng:**
- Tạo hóa đơn từ đơn hàng Facebook
- Kiểm tra quyền phê duyệt
- Ghi log giao dịch

**Quy Trình:**
1. Khách hàng mua hàng qua Facebook inbox
2. Hệ thống tạo đơn hàng (Bill)
3. Tự động tạo hóa đơn (Invoice)
4. Kiểm tra quyền của người dùng
5. Ghi nhận trong hệ thống kế toán

**Request:**
```json
{
  "billId": 42
}
```

---

## 📊 **CƠ SỞ DỮ LIỆU - ENTITIES**

### **Các Bảng Dữ Liệu MultiChannel:**

```sql
-- Cấu Hình Facebook App
FacebookAppConfigs
├── Id (int)
├── AppId (string)
├── AppSecret (string)
├── CreatedDate (datetime)
└── UpdatedDate (datetime)

-- Thông Tin Fanpage
FacebookPages
├── Id (int)
├── UserId (int)
├── PageId (string) -- Facebook Page ID
├── Name (string)
├── AccessToken (string)
├── UserToken (string)
├── Status (string)
└── ConnectedDate (datetime)

-- Bài Viết Trên Fanpage
SocialPosts
├── Id (int)
├── UserId (int)
├── PageId (int)
├── Content (string)
├── ImageUrls (string[])
├── Status (enum) -- Draft, Published, Scheduled
├── PublishedDate (datetime)
├── ScheduledTime (datetime)
├── Engagement (JSON) -- Likes, Comments, Shares
└── CreatedDate (datetime)

-- Quy Trình Tự Động Hóa
AutomationWorkflows
├── Id (int)
├── UserId (int)
├── Name (string)
├── WorkflowJson (JSON)
├── TriggerType (string)
├── Status (string)
└── CreatedDate (datetime)

-- Ghi Log Tự Động Hóa
AutomationLogs
├── Id (int)
├── WorkflowId (int)
├── ExecutionTime (datetime)
├── Status (string)
├── Result (JSON)
└── ErrorMessage (string)
```

---

## 🔄 **TÍCH HỢP Qtradicional ERP**

### **Liên Kết Với Module QLNS:**
- Khách hàng → Tạo Hồ Sơ Nhân Sự (nếu là nhân viên bán hàng)
- Đơn hàng → Tạo Hợp Đồng/Invoice
- Thanh toán → Ghi nhận Phúc Lợi/Lương

### **Liên Kết Với Module Chấm Công:**
- Ghi nhận công việc bán hàng (nếu nhân viên làm việc trên Facebook)
- Tính hoa hồng dựa trên doanh số

---

## 💡 **CÁC TÍNH NĂNG NÂNG CAO**

| Tính Năng | Mô Tả | Trạng Thái |
|-----------|-------|-----------|
| **Multi-Channel** | Hỗ trợ Instagram, TikTok Shop (tương lai) | ⏳ Sắp tới |
| **Chatbot AI** | Tự động trả lời tin nhắn | ⏳ Sắp tới |
| **Inventory Sync** | Cập nhật tồn kho tự động | ⏳ Sắp tới |
| **Analytics Dashboard** | Bảng điều khiển phân tích chi tiết | ⏳ Sắp tới |
| **CRM Integration** | Quản lý khách hàng tích hợp | ⏳ Sắp tới |
| **Email Marketing** | Gửi email marketing campaigns | ⏳ Sắp tới |

---

## 🚀 **CÁCH SỬ DỤNG**

### **Bước 1: Cấu Hình Facebook App**
1. Đăng nhập vào AWS/VPS
2. Vào Admin Dashboard
3. Cấu hình App ID & App Secret

### **Bước 2: Kết Nối Facebook**
1. Nhấn "Connect Facebook"
2. Xác thực tài khoản Facebook
3. Chấp nhận các quyền yêu cầu
4. Chọn fanpage cần quản lý

### **Bước 3: Tạo & Đăng Bài Viết**
1. Vào "Create Post"
2. Chọn fanpage
3. Viết nội dung hoặc dùng AI tạo
4. Đính kèm hình ảnh
5. Đăng ngay hoặc lên lịch

### **Bước 4: Theo Dõi Hiệu Suất**
1. Vào "Analytics"
2. Chọn fanpage & thời gian
3. Xem insights (lượt tiếp cận, engagement, v.v.)

---

## ✅ **KẾT LUẬN**

Module **MultiChannel** của AciPlatform là một hệ thống quản lý bán hàng hiện đại với khả năng:
- ✅ Tích hợp Facebook seamlessly
- ✅ AI tạo nội dung tự động
- ✅ Tự động hóa quy trình bán hàng
- ✅ Phân tích chi tiết hiệu suất
- ✅ Quản lý hóa đơn & thanh toán
- ✅ Liên kết với ERP truyền thống

