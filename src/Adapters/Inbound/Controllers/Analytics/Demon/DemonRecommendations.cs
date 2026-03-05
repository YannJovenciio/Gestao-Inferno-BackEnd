namespace Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;

public record DemonRecommendations(
    Guid IdDemon,
    string DemonName,
    string Category,
    int PersecutionCount,
    string MostTorturedSoulName,
    int SoulCount
);

public record DemonRecommendationsResponse(
    List<DemonRecommendations> Recommendations,
    int TotalItems
);
