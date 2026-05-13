-- ============================================================
-- MENU DATA: Quản Lý Khách Sạn (Hotel Management Module)
-- Database: AciPlatform | Created: 2026-05-11
-- ============================================================

USE AciPlatform;
GO

-- Xóa nếu đã tồn tại (chạy lại an toàn)
DELETE FROM Menus WHERE Code LIKE 'hotel%';
GO

-- ── 1. Menu CHA: Quản Lý Khách Sạn (IsParent = 1) ──────────
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES (
    'hotel',
    NULL,
    N'Quản Lý Khách Sạn',
    'Hotel Management',
    NULL,
    NULL,
    'hotel',
    1,
    13,
    N'Module quản lý khách sạn HomeHG Hà Giang'
);
GO

-- ── 2. Menu CON ──────────────────────────────────────────────

-- Dashboard Khách Sạn
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/dashboard', 'hotel', N'Dashboard Hôm Nay', 'Hotel Dashboard', NULL, '/hotel/dashboard', 'dashboard_customize', 0, 1, NULL);

-- Sơ Đồ Phòng
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/room-map', 'hotel', N'Sơ Đồ Phòng', 'Room Map', NULL, '/hotel/room-map', 'map', 0, 2, NULL);

-- Quản Lý Đặt Phòng
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/bookings', 'hotel', N'Đặt Phòng', 'Bookings', NULL, '/hotel/bookings', 'calendar_month', 0, 3, NULL);

-- Cho Thuê Xe
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/vehicles', 'hotel', N'Cho Thuê Xe', 'Vehicle Rental', NULL, '/hotel/vehicles', 'two_wheeler', 0, 4, NULL);

-- Quản Lý Tour
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/tours', 'hotel', N'Quản Lý Tour', 'Tours', NULL, '/hotel/tours', 'tour', 0, 5, NULL);

-- Hồ Sơ Khách
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/guests', 'hotel', N'Hồ Sơ Khách', 'Guests', NULL, '/hotel/guests', 'people', 0, 6, NULL);

-- Báo Cáo
INSERT INTO Menus (Code, CodeParent, Name, NameEN, NameKO, Url, Icon, IsParent, [Order], Note)
VALUES ('hotel/reports', 'hotel', N'Báo Cáo', 'Reports', NULL, '/hotel/reports', 'bar_chart', 0, 7, NULL);

GO

-- ── 3. Kiểm tra kết quả ─────────────────────────────────────
SELECT Id, Code, CodeParent, Name, Url, Icon, IsParent, [Order]
FROM Menus
WHERE Code LIKE 'hotel%'
ORDER BY IsParent DESC, [Order];
GO
