using Inferno.src.Core.Domain.Enums;

namespace Inferno.src.Adapters.Inbound.Controllers.Analytics.Soul;

public record SoulRecommendationsDto(
    Guid SoulId,
    string SoulName,
    HellLevel? Level,
    int PersecutionCount,
    string MostActiveDemonName,
    int DemonCount,
    int SinCount
);
