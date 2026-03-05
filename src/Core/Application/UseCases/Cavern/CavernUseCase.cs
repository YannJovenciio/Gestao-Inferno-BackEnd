using Inferno.src.Adapters.Inbound.Controllers.Cavern;
using Inferno.src.Adapters.Outbound.Persistence.Repositories.Cavern;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Core.Application.UseCases.Cavern;

public class CavernUseCase : ICavernUseCase
{
    public ILogger<CavernUseCase> _logger;
    private readonly ICavernRepository _repository;

    public CavernUseCase(ILogger<CavernUseCase> logger, ICavernRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<(CavernResponse? response, string message)> CreateCavern(CavernInput input)
    {
        _logger.LogInformation("Received request to create cavern");
        if (input == null)
        {
            _logger.LogWarning("Invalid input provided for CreateCavern: null");
            return (null, "Invalid input provided");
        }

        var cavern = await _repository.CreateCavern(
            new Entity.Cavern(input.CavernName, input.Location, input.Capacity)
        );

        var response = new CavernResponse(
            cavern.IdCavern,
            cavern.CavernName,
            cavern.Location,
            cavern.Capacity
        );

        _logger.LogInformation("Successfully created cavern with id: {CavernId}", cavern.IdCavern);
        return (response, "Succesfull created cavern");
    }

    public async Task<(List<CavernResponse>? responses, string message)> CreateManyCavern(
        List<CavernInput> inputs
    )
    {
        _logger.LogInformation("Received request to create many caverns");
        if (inputs == null)
        {
            _logger.LogWarning("Invalid input provided for CreateManyCavern: null");
            return (null, "Invalid input provided");
        }
        var caverns = await _repository.CreateManyCavern([
            .. inputs.Select(c => new Entity.Cavern(c.Location, c.CavernName, c.Capacity)),
        ]);
        var response = caverns
            .Select(c => new CavernResponse(c.IdCavern, c.CavernName, c.Location, c.Capacity))
            .ToList();

        _logger.LogInformation("Successfully created {CavernCount} caverns", caverns.Count);
        return (response, $"Succesfull created {caverns.Count} caverns");
    }

    public async Task<(CavernResponse? response, string message)> DeleteCavern(Guid id)
    {
        _logger.LogInformation("Received request to delete cavern with id: {CavernId}", id);
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Invalid input provided for DeleteCavern: empty Guid");
            return (null, "Invalid input provided");
        }
        var cavern = await _repository.DeleteCavern(id);
        var response = new CavernResponse(
            cavern.IdCavern,
            cavern.CavernName,
            cavern.Location,
            cavern.Capacity
        );
        _logger.LogInformation("Successfully deleted cavern with id: {CavernId}", cavern.IdCavern);
        return (response, $"Succesfull deleted cavern with id{id}");
    }

    public async Task<(List<CavernResponse>? responses, string message)> GetAllCaverns()
    {
        _logger.LogInformation("Received request to get all caverns");
        var caverns = await _repository.GetAllCaverns();
        if (caverns.Count == 0 || caverns == null)
        {
            _logger.LogInformation("No caverns found");
            return (null, "No caverns found ");
        }
        var response = caverns
            .Select(c => new CavernResponse(c.IdCavern, c.CavernName, c.Location, c.Capacity))
            .ToList();
        _logger.LogInformation("Successfully retrieved {CavernCount} caverns", response.Count);
        return (response, $"Succesfull retrivied {response.Count} caverns");
    }

    public async Task<(List<CavernResponse>? responses, string message)> GetAllCaverns(
        int pageSize,
        int pageNumber
    )
    {
        _logger.LogInformation(
            "Received request to get all caverns with pagination: pageSize={PageSize}, pageNumber={PageNumber}",
            pageSize,
            pageNumber
        );
        var caverns = await _repository.GetAllCaverns(pageSize, pageNumber);
        if (caverns.Count == 0 || caverns == null)
        {
            _logger.LogInformation("No caverns found for pagination request");
            return (null, "No caverns found ");
        }
        var response = caverns
            .Select(c => new CavernResponse(c.IdCavern, c.CavernName, c.Location, c.Capacity))
            .ToList();
        _logger.LogInformation("Successfully retrieved {CavernCount} caverns", response.Count);
        return (response, $"Succesfull retrivied {response.Count} caverns");
    }

    public async Task<(CavernResponse? response, string message)> GetCavernById(Guid id)
    {
        _logger.LogInformation("Received request to get cavern by id: {CavernId}", id);
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Invalid input provided for GetCavernById: empty Guid");
            return (null, "Invalid input provided");
        }

        var cavern = await _repository.GetCavernById(id);

        var response = new CavernResponse(
            cavern.IdCavern,
            cavern.CavernName,
            cavern.Location,
            cavern.Capacity
        );
        _logger.LogInformation("Successfully found cavern with id: {CavernId}", cavern.IdCavern);
        return (response, $"Succesfull found cavern for id {id}");
    }
}
