# Hướng dẫn sử dụng RefreshToken với Cookie và Assign Menu

## 1. Refresh Token qua HTTP-Only Cookie

### Login API (`POST /api/auth/login`)

**Request:**
```json
{
  "username": "admin",
  "password": "admin",
  "companyCode": "optional"
}
```

**Response:**
```json
{
  "status": 200,
  "message": "Login success",
  "data": {
    "id": 1,
    "username": "admin",
    "fullname": "Administrator",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "menus": [...],
    ...
  }
}
```

**Cookie Set:**
- Tên: `refreshToken`
- HttpOnly: `true`
- Secure: `true` (yêu cầu HTTPS)
- SameSite: `Strict`
- Expires: 30 ngày

### Refresh Token API (`POST /api/auth/refresh`)

**Request:** Không cần body (lấy refreshToken từ cookie)

**Response:**
```json
{
  "status": 200,
  "message": "Token refreshed successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "menus": [...]
  }
}
```

**Cơ chế hoạt động:**
1. Client login → Server trả về accessToken và set refreshToken vào cookie
2. Client lưu accessToken để gọi API
3. Khi accessToken hết hạn → Client gọi `/api/auth/refresh` (cookie tự động gửi)
4. Server validate refreshToken từ cookie → Trả về accessToken mới và set refreshToken mới

**Lợi ích:**
- RefreshToken không bao giờ lộ ra JavaScript (HttpOnly)
- Không thể bị XSS attack đánh cắp
- Secure flag đảm bảo chỉ gửi qua HTTPS
- SameSite Strict chống CSRF attack

---

## 2. Assign Menu cho User

### Entity: UserMenu
Bảng `UserMenus` lưu menu permissions riêng cho từng user (không dựa vào role).

### API Endpoints

#### 2.1. Lấy danh sách menu assignments của user
```
GET /api/usermenus/{userId}
```

**Response:**
```json
{
  "status": 200,
  "message": "Success",
  "data": [
    {
      "id": 1,
      "userId": 5,
      "menuId": 10,
      "menuCode": "users",
      "view": true,
      "add": true,
      "edit": false,
      "delete": false
    }
  ]
}
```

#### 2.2. Lấy menu permissions của user (formatted)
```
GET /api/usermenus/{userId}/permissions
```

**Response:**
```json
{
  "status": 200,
  "message": "Success",
  "data": [
    {
      "id": 10,
      "menuCode": "users",
      "name": "User Management",
      "nameEN": "User Management",
      "nameKO": "사용자 관리",
      "order": 1,
      "view": true,
      "add": true,
      "edit": false,
      "delete": false
    }
  ]
}
```

#### 2.3. Assign menus cho user
```
POST /api/usermenus/{userId}/assign
```

**Request Body:**
```json
[
  {
    "menuId": 10,
    "menuCode": "users",
    "view": true,
    "add": true,
    "edit": false,
    "delete": false
  },
  {
    "menuId": 11,
    "menuCode": "menus",
    "view": true,
    "add": false,
    "edit": false,
    "delete": false
  }
]
```

**Response:**
```json
{
  "status": 200,
  "message": "Menus assigned successfully"
}
```

**Lưu ý:**
- API này sẽ **xóa tất cả** menu assignments cũ và thêm mới
- Cần có Bearer token trong header để authenticate

#### 2.4. Xóa một menu assignment
```
DELETE /api/usermenus/{userId}/menu/{menuId}
```

#### 2.5. Xóa tất cả menu assignments của user
```
DELETE /api/usermenus/{userId}/clear
```

---

## 3. Logic Ưu tiên Menu

Khi lấy menu permissions cho user trong `MenuService.GetMenuPermissionsByUserId()`:

**Thứ tự ưu tiên:**
1. **UserMenu** (nếu có) - Menu được assign trực tiếp cho user
2. **SuperAdmin Role** - Nếu user có role SuperAdmin → Full permissions
3. **MenuRole** - Menu permissions dựa trên role của user

**Code Logic:**
```csharp
// 1. Check UserMenu assignments
var userMenus = await _context.UserMenus
    .Where(um => um.UserId == userId)
    .ToListAsync();

if (userMenus.Any())
{
    // Return UserMenu permissions (highest priority)
    return userMenus;
}

// 2. Check SuperAdmin role
if (roleCodes.Contains("SuperAdmin"))
{
    // Return all menus with full permissions
}

// 3. Fall back to MenuRole permissions
var menuRoles = await _context.MenuRoles
    .Where(mr => roleIds.Contains(mr.UserRoleId.Value))
    .ToListAsync();
```

---

## 4. Migration Database

Migration đã được tạo tự động: `AddUserMenus`

**Apply migration:**
```bash
cd AciPlatform.Infrastructure
dotnet ef database update --startup-project ..\AciPlatform.Api
```

Hoặc để auto-migration khi chạy app (đã config trong `Program.cs`):
```bash
cd AciPlatform.Api
dotnet run
```

**Bảng UserMenus:**
- `Id` (PK)
- `UserId` (FK → Users)
- `MenuId` (FK → Menus)
- `MenuCode`
- `View`, `Add`, `Edit`, `Delete` (permissions)
- `CreatedDate`, `CreatedBy`
- Unique constraint: `(UserId, MenuId)`

---

## 5. Ví dụ Sử dụng

### Frontend Flow với Refresh Token Cookie

```javascript
// 1. Login
const loginResponse = await fetch('/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ username: 'admin', password: 'admin' }),
  credentials: 'include' // Quan trọng: gửi và nhận cookie
});

const { data } = await loginResponse.json();
localStorage.setItem('accessToken', data.token);

// 2. Gọi API với Access Token
const apiResponse = await fetch('/api/users', {
  headers: { 'Authorization': `Bearer ${localStorage.getItem('accessToken')}` }
});

// 3. Khi Access Token hết hạn (401)
if (apiResponse.status === 401) {
  // Refresh token (cookie tự động gửi)
  const refreshResponse = await fetch('/api/auth/refresh', {
    method: 'POST',
    credentials: 'include' // Quan trọng!
  });
  
  const { data } = await refreshResponse.json();
  localStorage.setItem('accessToken', data.token);
  
  // Retry original request
  const retryResponse = await fetch('/api/users', {
    headers: { 'Authorization': `Bearer ${data.token}` }
  });
}
```

### Assign Menu cho User

```javascript
// Admin assign menus cho user ID = 5
const assignResponse = await fetch('/api/usermenus/5/assign', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${accessToken}`
  },
  body: JSON.stringify([
    { menuId: 1, menuCode: 'users', view: true, add: true, edit: false, delete: false },
    { menuId: 2, menuCode: 'menus', view: true, add: false, edit: false, delete: false }
  ])
});
```

---

## 6. Security Notes

### HTTPS/Secure Cookie
- Trong production, **BẮT BUỘC** dùng HTTPS
- Cookie `Secure` flag chỉ hoạt động qua HTTPS
- Update `appsettings.Production.json`:
```json
{
  "UseSecureCookies": true
}
```

### SameSite Policy
- `Strict`: Chỉ gửi cookie trong cùng domain (khuyến nghị)
- `Lax`: Cho phép GET request từ external site
- `None`: Cho phép cross-site (cần Secure flag)

### Token Expiration
- **AccessToken**: 8 giờ (config trong `appsettings.json`)
- **RefreshToken**: 30 ngày
- Có thể điều chỉnh trong code:
```csharp
// AuthController.cs - Login method
var refresh = await _refreshTokenService.CreateAsync(user.Id, TimeSpan.FromDays(30));
```

---

## 7. Testing

### Test với curl
```bash
# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}' \
  -c cookies.txt

# Refresh Token
curl -X POST http://localhost:5000/api/auth/refresh \
  -b cookies.txt \
  -c cookies.txt

# Assign Menu
curl -X POST http://localhost:5000/api/usermenus/5/assign \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -d '[{"menuId":1,"menuCode":"users","view":true,"add":true}]'
```

---

## 8. Changes Summary

### Files Added:
- `AciPlatform.Domain/Entities/UserMenu.cs`
- `AciPlatform.Application/Interfaces/IUserMenuService.cs`
- `AciPlatform.Application/Services/UserMenuService.cs`
- `AciPlatform.Api/Controllers/UserMenusController.cs`
- `AciPlatform.Infrastructure/Migrations/[timestamp]_AddUserMenus.cs`

### Files Modified:
- `AciPlatform.Api/Controllers/AuthController.cs`
  - Login: Set refreshToken vào cookie
  - Refresh: Đọc refreshToken từ cookie và trả về menus
- `AciPlatform.Application/Services/MenuService.cs`
  - GetMenuPermissionsByUserId: Ưu tiên UserMenu
- `AciPlatform.Application/DTOs/AuthDtos.cs`
  - Added: UserMenuAssignDto
- `AciPlatform.Application/Interfaces/IApplicationDbContext.cs`
  - Added: DbSet<UserMenu> UserMenus
- `AciPlatform.Infrastructure/Persistence/ApplicationDbContext.cs`
  - Added: UserMenus DbSet và configuration
- `AciPlatform.Api/Program.cs`
  - Register: IUserMenuService
