-- Script to add MultiChannel menus (Đa Kênh)
-- Run this after the migration is applied

-- Insert parent menu "Đa Kênh" 
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Code = 'dakenh')
BEGIN
    INSERT INTO Menus (Code, Name, CodeParent, [Order], IsParent)
    VALUES ('dakenh', N'Đa Kênh', NULL, 100, 1);
END

-- Insert child menu "Facebook"
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Code = 'dakenh/facebook')
BEGIN
    INSERT INTO Menus (Code, Name, CodeParent, [Order], IsParent)
    VALUES ('dakenh/facebook', N'Facebook', 'dakenh', 101, 0);
END

-- You can grant permissions by adding to MenuRoles table once UserRole IDs are known.
-- Example:
-- INSERT INTO MenuRoles (MenuId, UserRoleId, [View], [Add], [Edit], [Delete])
-- SELECT m.Id, ur.Id, 1, 1, 1, 1
-- FROM Menus m, UserRoles ur
-- WHERE m.Code = 'dakenh/facebook' AND ur.Code = 'SuperAdmin';

SELECT * FROM Menus WHERE Code LIKE 'dakenh%';
