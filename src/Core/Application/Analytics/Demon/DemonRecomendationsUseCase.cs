using Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;
using Inferno.src.Adapters.Outbound.Persistence.Repositories.Demon;

namespace Inferno.src.Core.Application.Analytics.Demon;

public class DemonRecomendationsUseCase(
    ILogger<DemonRecomendationsUseCase> logger,
    IDemonRecommendationQuery RecommendationQuery
) : IDemonRecomendationsUseCase
{
    private readonly ILogger<DemonRecomendationsUseCase> _logger = logger;
    private readonly IDemonRecommendationQuery _recommendationQuery = RecommendationQuery;

    public async Task<(DemonRecommendationsResponse response, string message)> GetRecomendations(
        int? pageSize,
        int? pageNumber
    )
    {
        var recommendations = await _recommendationQuery.GetRecommendations(
            pageSize: pageSize,
            pageNumber: pageNumber
        );

        _logger.LogInformation($"Succesfull retrivied {recommendations.Count} recommendations");

        var response = new DemonRecommendationsResponse(recommendations, recommendations.Count);

        return (response, $"Succesfull retrivied recommendations");
    }
}
