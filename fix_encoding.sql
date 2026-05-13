USE AciPlatform_Hotel;
GO

-- Fix Tours
UPDATE HotelTours SET TourName = N'Hà Giang Loop 3N2Đ', Highlights = N'["Đèo Mã Pì Lèng","Cao nguyên đá Đồng Văn","Sông Nho Quế","Cột cờ Lũng Cú"]' 
WHERE HotelCode = 'HOMEHG' AND TourCode = 'LOOP_3D2N';

UPDATE HotelTours SET TourName = N'Hà Giang Loop 1 Ngày', Highlights = N'["Đèo Bắc Sum","Làng Văn hóa","Đồng Lâm"]' 
WHERE HotelCode = 'HOMEHG' AND TourCode = 'LOOP_1D';

UPDATE HotelTours SET TourName = N'Trekking Bản Làng', Highlights = N'["Bản dân tộc Mông","Ruộng bậc thang","Chợ phiên Đồng Văn"]' 
WHERE HotelCode = 'HOMEHG' AND TourCode = 'TREK_BAN';

UPDATE HotelTours SET TourName = N'Tour Xe Ô Tô Đồng Văn', Highlights = N'["Cao nguyên đá Đồng Văn","Phố cổ Đồng Văn","Lũng Cú"]' 
WHERE HotelCode = 'HOMEHG' AND TourCode = 'CAR_TOUR';

-- Fix Room Types
UPDATE PMS_RoomTypes SET Ten = N'Phòng Khép Kín (Private)' WHERE HotelCode = 'HOMEHG' AND Ma = 'KHEPKIN';
UPDATE PMS_RoomTypes SET Ten = N'Phòng Tập Thể (Dormitory) - Giường' WHERE HotelCode = 'HOMEHG' AND Ma = 'TAPTHE';
UPDATE PMS_RoomTypes SET Ten = N'Phòng Tập Thể Lớn (Group Room)' WHERE HotelCode = 'HOMEHG' AND Ma = 'TAPTHE_L';

-- Fix Rooms
UPDATE PMS_Rooms SET Ten = N'Phòng Khép Kín' WHERE HotelCode = 'HOMEHG' AND Ma = 'KHEPKIN';
UPDATE PMS_Rooms SET Ten = N'Phòng Tập Thể (Lớn)' WHERE HotelCode = 'HOMEHG' AND Ma = 'TAPTHE_L';
UPDATE PMS_Rooms SET Ten = N'Phòng Tập Thể' WHERE HotelCode = 'HOMEHG' AND Ma = 'TAPTHE';
GO
