using Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;

namespace Inferno.src.Core.Application.Analytics.Demon;

public interface IDemonRecomendationsUseCase
{
    Task<(DemonRecommendationsResponse response, string message)> GetRecomendations(
        int? pageSize,
        int? pageNumber
    );
}
