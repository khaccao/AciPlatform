using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class WareHousePositionService : IWareHousePositionService
{
    private readonly IApplicationDbContext _context;

    public WareHousePositionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<WareHousePosition>> GetAll(FilterParams param)
    {
        var query = _context.WareHousePositions.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x => (x.Code != null && x.Code.Contains(param.SearchText)) || (x.Name != null && x.Name.Contains(param.SearchText)));
        }

        var totalItems = await query.CountAsync();
        var data = await query.OrderBy(x => x.Id).Skip((param.Page - 1) * param.PageSize).Take(param.PageSize).ToListAsync();

        return new PagingResult<WareHousePosition>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task<IEnumerable<WareHousePositionGetAllModel>> GetAll()
    {
        return await _context.WareHousePositions.Where(x => !x.IsDeleted)
           .Join(_context.WareHouseFloorWithPositions,
                   b => b.Id,
                   d => d.WareHousePositionId,
                   (b, d) => new WareHousePositionGetAllModel
                   {
                       Id = b.Id,
                       Code = b.Code,
                       Name = b.Name,
                       WareHouseFloorId = d.WareHouseFloorId
                   })
           .ToListAsync();
    }

    public async Task<WareHousePosition> GetById(int id)
    {
        return await _context.WareHousePositions.FindAsync(id) ?? null!;
    }

    public async Task Create(WareHousePosition param)
    {
        param.CreatedDate = DateTime.Now;
        _context.WareHousePositions.Add(param);
        await _context.SaveChangesAsync();
    }

    public async Task Update(WareHousePosition param)
    {
        var itemFind = await _context.WareHousePositions.FindAsync(param.Id);
        if (itemFind == null) throw new Exception("Position not found");

        itemFind.Name = param.Name;
        itemFind.Code = param.Code;
        itemFind.Note = param.Note;
        itemFind.UpdatedDate = DateTime.Now;

        _context.WareHousePositions.Update(itemFind);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var itemFind = await _context.WareHousePositions.FindAsync(id);
        if (itemFind != null)
        {
            itemFind.IsDeleted = true;
            itemFind.UpdatedDate = DateTime.Now;
            _context.WareHousePositions.Update(itemFind);
            await _context.SaveChangesAsync();
        }
    }
}
