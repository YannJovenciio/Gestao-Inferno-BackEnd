using Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;
using Microsoft.EntityFrameworkCore;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.Demon;

public class DemonRecommendationQuery(HellDbContext context) : IDemonRecommendationQuery
{
    private readonly HellDbContext _context = context;

    public async Task<List<DemonRecommendations>> GetRecommendations(int? pageSize, int? pageNumber)
    {
        var demonRecommendations = await _context
            .Demons.AsNoTracking()
            .GroupJoin(
                _context.Persecution.Include(p => p.Soul),
                d => d.IdDemon,
                p => p.IdDemon,
                (demon, persecutions) => new { Demon = demon, Persecution = persecutions }
            )
            .ToListAsync();

        var recommendations = demonRecommendations
            .Select(x => new DemonRecommendations(
                x.Demon.IdDemon,
                x.Demon.DemonName ?? "Unknown",
                string.IsNullOrEmpty(x.Demon.Category?.CategoryName)
                    ? "Unknown"
                    : x.Demon.Category.CategoryName,
                x.Persecution.Count(),
                x.Persecution.GroupBy(p => p.Soul)
                    .OrderByDescending(d => d.Count())
                    .FirstOrDefault()
                    ?.Key.SoulName
                ?? "Unknown",
                x.Demon.Persecutions.GroupBy(d => d.Soul).Count()
            ))
            .ToList();

        if (pageSize.HasValue && pageNumber.HasValue)
        {
            return
            [
                .. recommendations
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value),
            ];
        }

        return recommendations;
    }
}
