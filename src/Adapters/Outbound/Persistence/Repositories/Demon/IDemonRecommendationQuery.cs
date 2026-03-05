using Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.Demon;

public interface IDemonRecommendationQuery
{
    Task<List<DemonRecommendations>> GetRecommendations(int? pageSize, int? pageNumber);
}
