using Inferno.src.Adapters.Inbound.Controllers.HellTask;
using Inferno.src.Adapters.Outbound.Persistence.Repositories;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Core.Application.UseCases.HellTask;

public class HellTaskUseCase(IHellTaskRepository repository, ILogger<HellTaskUseCase> logger)
    : IHellTaskUseCase
{
    private readonly IHellTaskRepository _repository = repository;
    private readonly ILogger<HellTaskUseCase> _logger = logger;

    public Task<(HellTaskResponse response, string message)> CreateAsync(
        HellTaskInput input,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Creating new HellTask");
        var task = new Entity.HellTask(input.Title, input.Description, input.DemonId);
        _repository.CreateAsync(task, cancellationToken);
        throw new NotImplementedException();
    }

    public Task<(List<HellTaskResponse> response, string message)> CreateManyAsync(
        List<HellTaskInput> tasks,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<(Guid id, string message)> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<(List<HellTaskResponse> response, string message)> GetAllByIdAsync(
        int? pageSize,
        int? pageNumber,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
