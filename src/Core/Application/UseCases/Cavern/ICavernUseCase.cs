using Inferno.src.Adapters.Inbound.Controllers.Cavern;

namespace Inferno.src.Core.Application.UseCases.Cavern;

public interface ICavernUseCase
{
    Task<(CavernResponse? response, string message)> GetCavernById(Guid id);
    Task<(List<CavernResponse>? responses, string message)> GetAllCaverns();
    Task<(List<CavernResponse>? responses, string message)> GetAllCaverns(
        int pageSize,
        int pageNumber
    );
    Task<(CavernResponse? response, string message)> CreateCavern(CavernInput input);
    Task<(List<CavernResponse>? responses, string message)> CreateManyCavern(
        List<CavernInput>? inputs
    );

    Task<(CavernResponse? response, string message)> DeleteCavern(Guid id);
}
