import sys
import os

# 1. IHotelBookingService.cs
content_i = open('AciPlatform.Application/Interfaces/HotelManagement/IHotelBookingService.cs', 'r', encoding='utf-8').read()
if 'AddBookingServiceAsync' not in content_i:
    content_i = content_i.replace(
        'Task<BookingDto> UpdateBookingAsync(int id, CreateBookingRequest req);',
        'Task<BookingDto> UpdateBookingAsync(int id, CreateBookingRequest req);\n    Task<BookingDto> AddBookingServiceAsync(int bookingId, AddBookingServiceRequest req);'
    )
    # Add AddBookingServiceRequest model if it does not exist
    if 'public class AddBookingServiceRequest' not in content_i:
        req_class = """
public class AddBookingServiceRequest
{
    public string ServiceCode { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Category { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; } = 0;
}
"""
        content_i += req_class
    open('AciPlatform.Application/Interfaces/HotelManagement/IHotelBookingService.cs', 'w', encoding='utf-8').write(content_i)


# 2. HotelBookingService.cs
content_s = open('AciPlatform.Infrastructure/Services/HotelManagement/HotelBookingService.cs', 'r', encoding='utf-8').read()
if 'AddBookingServiceAsync' not in content_s:
    func_s = """    public async Task<BookingDto> AddBookingServiceAsync(int bookingId, AddBookingServiceRequest req)
    {
        var b = await _db.HotelBookings.Include(x => x.Services).FirstOrDefaultAsync(x => x.Id == bookingId) 
            ?? throw new InvalidOperationException("Booking not found.");
        
        var svc = new AciPlatform.Domain.Entities.Hotel.HotelBookingService
        {
            HotelCode = b.HotelCode,
            BookingId = bookingId,
            ServiceCode = req.ServiceCode,
            ServiceName = req.ServiceName,
            Category = req.Category,
            Quantity = req.Quantity,
            UnitPrice = req.UnitPrice,
            TotalPrice = req.Quantity * req.UnitPrice
        };
        
        b.Services.Add(svc);
        b.ServicePrice += svc.TotalPrice;
        b.TotalAmount += svc.TotalPrice;
        b.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        
        return ToDto(b);
    }
"""
    # Insert before GetBookingsAsync
    content_s = content_s.replace('public async Task<(List<BookingDto> Items, int Total)> GetBookingsAsync(BookingFilterRequest filter)',
        func_s + '\n    public async Task<(List<BookingDto> Items, int Total)> GetBookingsAsync(BookingFilterRequest filter)')
    open('AciPlatform.Infrastructure/Services/HotelManagement/HotelBookingService.cs', 'w', encoding='utf-8').write(content_s)


# 3. HotelBookingsController.cs
content_c = open('AciPlatform.Api/Controllers/Hotel/HotelBookingsController.cs', 'r', encoding='utf-8').read()
if 'AddService' not in content_c:
    func_c = """    /// POST /api/hotel-bookings/{id}/services — Thêm dịch vụ/minibar
    [HttpPost("{id:int}/services")]
    public async Task<IActionResult> AddService(int id, [FromBody] AddBookingServiceRequest req)
        => Ok(await _svc.AddBookingServiceAsync(id, req));
"""
    content_c = content_c.replace('public async Task<IActionResult> Update(int id, [FromBody] CreateBookingRequest req)',
        func_c + '\n    [HttpPut("{id:int}")]\n    public async Task<IActionResult> Update(int id, [FromBody] CreateBookingRequest req)')
    open('AciPlatform.Api/Controllers/Hotel/HotelBookingsController.cs', 'w', encoding='utf-8').write(content_c)
