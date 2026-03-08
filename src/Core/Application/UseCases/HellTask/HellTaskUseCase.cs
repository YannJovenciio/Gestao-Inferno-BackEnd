using Inferno.src.Adapters.Inbound.Controllers.HellTask;
using Inferno.src.Adapters.Outbound.Persistence.Repositories;
using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Core.Application.UseCases.HellTask;

public class HellTaskUseCase(IHellTaskRepository repository, ILogger<HellTaskUseCase> logger)
    : IHellTaskUseCase
{
    private readonly IHellTaskRepository _repository = repository;
    private readonly ILogger<HellTaskUseCase> _logger = logger;

    public async Task<(HellTaskResponse response, string message)> CreateAsync(
        HellTaskInput input,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("Creating new HellTask");
        var task = new Entity.HellTask(input.Title, input.Description, input.DemonId);
        await _repository.CreateAsync(task, cancellationToken);
        var response = new HellTaskResponse(
            task.HellTaskId,
            task.Title,
            task.Description,
            task.CreatedAt,
            task.DeadLine,
            task.Status,
            task.Progress
        );
        return (response, "Succesfull created task");
    }

    public async Task<(List<HellTaskResponse> response, string message)> CreateManyAsync(
        List<HellTaskInput> input,
        CancellationToken cancellationToken
    )
    {
        var task = input
            .Select(x => new Entity.HellTask(x.Title, x.Description, x.DemonId))
            .ToList();
        await _repository.CreateManyAsync(task, cancellationToken);
        var response = task.Select(x => new HellTaskResponse(
                x.HellTaskId,
                x.Title,
                x.Description,
                x.CreatedAt,
                x.DeadLine,
                x.Status,
                x.Progress
            ))
            .ToList();
        return (response, "Succesfull created tasks");
    }

    public async Task<(Guid id, string message)> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return (id, "Task deleted succesfully");
    }

    public async Task<(List<HellTaskResponse> response, string message)> GetAllByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var tasks = await _repository.GetAllByIdAsync(id, cancellationToken);
        if (tasks is null or { Count: 0 })
            return ([], "No tasks found for id");
        var response = tasks
            .Select(x => new HellTaskResponse(
                x.HellTaskId,
                x.Title,
                x.Description,
                x.CreatedAt,
                x.DeadLine,
                x.Status,
                x.Progress
            ))
            .ToList();
        return (response, "succesfull retrivied tasks");
    }
}
