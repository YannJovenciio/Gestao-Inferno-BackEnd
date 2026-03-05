using Inferno.src.Adapters.Inbound.Controllers.Analytics.Demon;
using Inferno.src.Adapters.Inbound.Controllers.Model;
using Inferno.src.Core.Application.Analytics.Demon;
using Microsoft.AspNetCore.Mvc;

namespace Inferno.src.Adapters.Inbound.Controllers.Analytics;

[Route("api/[Controller]")]
public class DemonAnalyticsController : Controller
{
    private readonly ILogger<DemonAnalyticsController> _logger;
    private readonly IDemonRecomendationsUseCase _demonRecomendations;

    public DemonAnalyticsController(
        ILogger<DemonAnalyticsController> logger,
        IDemonRecomendationsUseCase demonRecomendations
    )
    {
        _logger = logger;
        _demonRecomendations = demonRecomendations;
    }

    [HttpGet]
    public async Task<IActionResult> GetDemonsAnalytics(
        [FromQuery] int? pageSize,
        [FromQuery] int? pageNumber
    )
    {
        var (response, message) = await _demonRecomendations.GetRecomendations(
            pageSize,
            pageNumber
        );
        return Ok(new APIResponse<DemonRecommendationsResponse>(response, message));
    }
}
