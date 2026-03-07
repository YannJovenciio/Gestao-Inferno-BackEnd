using Inferno.src.Adapters.Outbound.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Adapters.Outbound.Persistence.HellTask;

public class HellTaskRepository(HellDbContext context) : IHellTaskRepository
{
    private readonly HellDbContext _context = context;

    public async Task<Entity.HellTask> CreateAsync(
        Entity.HellTask task,
        CancellationToken cancellationToken
    )
    {
        await _context.HellTasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<List<Entity.HellTask>> CreateManyAsync(
        List<Entity.HellTask> tasks,
        CancellationToken cancellationToken
    )
    {
        await _context.HellTasks.AddRangeAsync(tasks, cancellationToken);
        await _context.SaveChangesAsync();
        return tasks;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _context
            .HellTasks.Where(h => h.HellTaskId == id)
            .ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Entity.HellTask>> GetAllByIdAsync(
        int? pageSize,
        int? pageNumber,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var tasks = await _context
            .HellTasks.AsNoTracking()
            .Where(h => h.HellTaskId == id)
            .Skip((pageSize ?? 10) * (pageNumber ?? 1))
            .Take(pageSize ?? 10)
            .ToListAsync();
        return tasks;
    }
}
