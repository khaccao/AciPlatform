using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public CustomersController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CustomersSearchViewModel param)
    {
        var query = _context.Customers.Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(param.SearchText))
        {
            query = query.Where(x =>
                (x.Name != null && x.Name.Contains(param.SearchText)) ||
                (x.Code != null && x.Code.Contains(param.SearchText)) ||
                (x.Phone != null && x.Phone.Contains(param.SearchText)));
        }

        if (!string.IsNullOrWhiteSpace(param.Code))
        {
            query = query.Where(x => x.Code == param.Code);
        }

        if (!string.IsNullOrWhiteSpace(param.Phone))
        {
            query = query.Where(x => x.Phone == param.Phone);
        }

        if (param.IsSupplier.HasValue)
        {
            query = query.Where(x => x.IsSupplier == param.IsSupplier.Value);
        }

        if (!string.IsNullOrWhiteSpace(param.Email))
        {
            query = query.Where(x => x.Email == param.Email);
        }

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new CustomerListItemModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                TaxCode = x.TaxCode,
                IsSupplier = x.IsSupplier,
                CreatedDate = x.CreatedDate
            })
            .ToListAsync();

        return Ok(new PagingResult<CustomerListItemModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetSelectList([FromQuery] string? searchText = null)
    {
        var query = _context.Customers.Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(x =>
                (x.Name != null && x.Name.Contains(searchText)) ||
                (x.Code != null && x.Code.Contains(searchText)));
        }

        var customers = await query
            .OrderBy(x => x.Name)
            .Select(x => new CustomerCodeNameModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            })
            .ToListAsync();

        return Ok(new ObjectReturn { data = customers, status = 200 });
    }

    [HttpGet("list-code-name")]
    public async Task<IActionResult> GetListCustomerWithCodeName()
    {
        var customers = await _context.Customers
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new CustomerCodeNameModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            })
            .ToListAsync();

        return Ok(new ObjectReturn { data = customers, status = 200 });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (customer == null)
        {
            return NotFound(new { msg = "Customer not found" });
        }

        return Ok(new ObjectReturn { data = customer, status = 200 });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        var exists = await _context.Customers.AnyAsync(x => x.Phone == customer.Phone && !x.IsDeleted);
        if (exists)
        {
            return BadRequest(new { msg = "Phone already exists" });
        }

        if (string.IsNullOrWhiteSpace(customer.Code))
        {
            customer.Code = await GenerateCustomerCode();
        }

        customer.CreatedDate = DateTime.Now;
        customer.UpdatedDate = DateTime.Now;
        customer.IsDeleted = false;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(new ObjectReturn { data = customer.Id, status = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
    {
        var existed = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (existed == null)
        {
            return NotFound(new { msg = "Customer not found" });
        }

        var duplicatedPhone = await _context.Customers.AnyAsync(x =>
            x.Id != id && !x.IsDeleted && x.Phone == customer.Phone && !string.IsNullOrEmpty(customer.Phone));

        if (duplicatedPhone)
        {
            return BadRequest(new { msg = "Phone already exists" });
        }

        existed.Code = customer.Code;
        existed.Name = customer.Name;
        existed.Avatar = customer.Avatar;
        existed.Phone = customer.Phone;
        existed.Email = customer.Email;
        existed.Address = customer.Address;
        existed.TaxCode = customer.TaxCode;
        existed.IsSupplier = customer.IsSupplier;
        existed.ProvinceId = customer.ProvinceId;
        existed.DistrictId = customer.DistrictId;
        existed.WardId = customer.WardId;
        existed.Gender = customer.Gender;
        existed.Provider = customer.Provider;
        existed.ProviderId = customer.ProviderId;
        existed.UpdatedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(new ObjectReturn { data = existed.Id, status = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existed = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (existed == null)
        {
            return NotFound(new { msg = "Customer not found" });
        }

        existed.IsDeleted = true;
        existed.UpdatedDate = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new ObjectReturn { status = 200 });
    }

    [HttpGet("get-code-customer")]
    public async Task<IActionResult> GetCodeCustomer()
    {
        var code = await GenerateCustomerCode();
        return Ok(new ObjectReturn { data = code, status = 200 });
    }

    [HttpGet("get-customer-warning")]
    public async Task<IActionResult> CustomerWarning()
    {
        var warnings = await _context.Customers
            .Where(x => !x.IsDeleted && string.IsNullOrEmpty(x.Email))
            .Select(x => new
            {
                x.Id,
                x.Code,
                x.Name,
                Warning = "Customer missing email"
            })
            .ToListAsync();

        return Ok(new ObjectReturn { data = warnings, status = 200 });
    }

    private async Task<string> GenerateCustomerCode()
    {
        var currentYear = DateTime.Now.Year;
        var prefix = $"CUS{currentYear}";
        var latestCode = await _context.Customers
            .Where(x => x.Code != null && x.Code.StartsWith(prefix))
            .OrderByDescending(x => x.Code)
            .Select(x => x.Code)
            .FirstOrDefaultAsync();

        var nextNumber = 1;
        if (!string.IsNullOrWhiteSpace(latestCode) && latestCode.Length > prefix.Length)
        {
            var numberPart = latestCode[prefix.Length..];
            if (int.TryParse(numberPart, out var parsed))
            {
                nextNumber = parsed + 1;
            }
        }

        return $"{prefix}{nextNumber:D4}";
    }
}

