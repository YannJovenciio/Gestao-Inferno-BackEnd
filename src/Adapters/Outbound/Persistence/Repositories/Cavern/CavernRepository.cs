using Microsoft.EntityFrameworkCore;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.Cavern;

public class CavernRepository : ICavernRepository
{
    private readonly HellDbContext _context;

    public CavernRepository(HellDbContext context)
    {
        _context = context;
    }

    public async Task<Entity.Cavern> CreateCavern(Entity.Cavern input)
    {
        await _context.Caverns.AddAsync(input);
        await _context.SaveChangesAsync();
        return input;
    }

    public async Task<List<Entity.Cavern>> CreateManyCavern(List<Entity.Cavern> inputs)
    {
        await _context.Caverns.AddRangeAsync(inputs);
        await _context.SaveChangesAsync();
        return inputs;
    }

    public async Task<Entity.Cavern> DeleteCavern(Guid id)
    {
        var cavern = await _context.Caverns.FirstOrDefaultAsync(c => c.IdCavern == id);
        await _context.Caverns.Where(x => x.IdCavern == id).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
        return cavern;
    }

    public async Task<List<Entity.Cavern>> GetAllCaverns()
    {
        return await _context.Caverns.AsNoTracking().ToListAsync();
    }

    public async Task<List<Entity.Cavern>> GetAllCaverns(int? pageSize, int? pageNumber)
    {
        return await _context
            .Caverns.AsNoTracking()
            .OrderBy(c => c.CavernName)
            .Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 10))
            .Take(pageSize ?? 10)
            .ToListAsync();
    }

    public async Task<Entity.Cavern> GetCavernById(Guid id)
    {
        var cavern = await _context
            .Caverns.AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCavern == id);
        return cavern!;
    }
}
