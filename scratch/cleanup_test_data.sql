-- Xóa dữ liệu test cho HOMEHG
PRINT 'Dang xoa HotelBookingRooms...';
DELETE FROM HotelBookingRooms WHERE BookingId IN (SELECT Id FROM HotelBookings WHERE HotelCode = 'HOMEHG');

PRINT 'Dang xoa HotelBookingServices...';
DELETE FROM HotelBookingServices WHERE BookingId IN (SELECT Id FROM HotelBookings WHERE HotelCode = 'HOMEHG');

PRINT 'Dang xoa HotelBookings...';
DELETE FROM HotelBookings WHERE HotelCode = 'HOMEHG';

PRINT 'Dang xoa HotelGuests...';
DELETE FROM HotelGuests WHERE HotelCode = 'HOMEHG';

PRINT 'Dang xoa HotelVehicleRentals...';
DELETE FROM HotelVehicleRentals WHERE HotelCode = 'HOMEHG';

PRINT 'Dang xoa PmsTourGuideSalaries...';
DELETE FROM PmsTourGuideSalaries WHERE HotelCode = 'HOMEHG';

PRINT 'Dang xoa PmsTourGuideContracts...';
DELETE FROM PmsTourGuideContracts WHERE GuideId IN (SELECT Id FROM HotelTourGuides WHERE HotelCode = 'HOMEHG');

PRINT 'Dang xoa PMS_MinibarOrderDetails...';
DELETE FROM PMS_MinibarOrderDetails WHERE OrderId IN (SELECT Id FROM PMS_MinibarOrders WHERE HotelCode = 'HOMEHG');

PRINT 'Dang xoa PMS_MinibarOrders...';
DELETE FROM PMS_MinibarOrders WHERE HotelCode = 'HOMEHG';

PRINT 'Dang xoa PMS_LaundryOrders...';
DELETE FROM PMS_LaundryOrders WHERE HotelCode = 'HOMEHG';

PRINT 'Reset trang thai phong ve VC...';
UPDATE PMS_Rooms SET Status = 'VC', CleanDirty = 1 WHERE HotelCode = 'HOMEHG';
UPDATE HotelElements SET Status = 'VC' WHERE HotelCode = 'HOMEHG';

PRINT 'Xong!';
