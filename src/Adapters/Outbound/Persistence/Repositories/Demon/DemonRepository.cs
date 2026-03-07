using Inferno.src.Adapters.Outbound.Persistence.Repositories;
using Inferno.src.Core.Domain.Entities;
using Inferno.src.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DemonRepository : IDemonRepository
{
    private readonly HellDbContext _context;

    public DemonRepository(HellDbContext context)
    {
        _context = context;
    }

    public async Task<Demon> GetByIdAsync(Guid id)
    {
        var demon = await _context.Demons.AsNoTracking().FirstOrDefaultAsync(d => d.IdDemon == id);
        return demon;
    }

    public async Task<Demon> CreateAsync(Demon input)
    {
        await _context.Demons.AddAsync(input);
        await _context.SaveChangesAsync();
        return input;
    }

    public async Task<List<Demon>> CreateManyAsync(List<Demon> inputs)
    {
        await _context.AddRangeAsync(inputs);
        await _context.SaveChangesAsync();
        return inputs;
    }

    public async Task<List<Demon>> GetAllAsync(int? pageSize, int? pageNumber)
    {
        var demons = await _context
            .Demons.AsNoTracking()
            .OrderBy(d => d.DemonName)
            .Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 10))
            .Take(pageSize ?? 10)
            .ToListAsync();

        return demons;
    }

    public async Task<List<Demon>> GetAllAsync()
    {
        var demons = await _context.Demons.AsNoTracking().OrderBy(d => d.DemonName).ToListAsync();
        return demons;
    }

    public async Task<List<Demon>> GetAllWithFiltersAsync(
        Guid? categoryId,
        string? name,
        DateTime? createdAt
    )
    {
        var query = _context.Demons.AsNoTracking();

        if (categoryId.HasValue)
            query = query.Where(d => d.CategoryId == categoryId);
        if (!string.IsNullOrEmpty(name))
            query = query.Where(d => d.DemonName.Contains(name));
        if (createdAt.HasValue)
            query = query.Where(d => d.CreatedAt.Date == createdAt.Value.Date);

        return await query.ToListAsync();
    }

    public async Task<(List<Demon> demons, int totalItems)> GetRecomendations(
        int? pageSize,
        int? pageNumber
    )
    {
        var demons = await _context
            .Demons.Include(d => d.Persecutions)
                .ThenInclude(p => p.Soul)
            .Include(x => x.Category)
            .Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 10))
            .Take(pageSize ?? 10)
            .OrderBy(d => d.DemonName)
            .ToListAsync();
        var totalItems = await _context.Demons.CountAsync();

        return (demons, totalItems);
    }
}
