using Entity = Inferno.src.Core.Domain.Entities;

namespace Inferno.src.Adapters.Outbound.Persistence.Repositories.Cavern;

public interface ICavernRepository
{
    Task<Entity.Cavern> GetCavernById(Guid id);
    Task<List<Entity.Cavern>> GetAllCaverns();

    Task<List<Entity.Cavern>> GetAllCaverns(int? pageSize, int? pageNumber);

    Task<Entity.Cavern> CreateCavern(Entity.Cavern input);

    Task<List<Entity.Cavern>> CreateManyCavern(List<Entity.Cavern> inputs);

    Task<Entity.Cavern> DeleteCavern(Guid id);
}
