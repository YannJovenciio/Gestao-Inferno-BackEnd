using System.ComponentModel.DataAnnotations;

namespace Inferno.src.Adapters.Inbound.Controllers.Cavern;

public record CavernInput(
    [property: Required(ErrorMessage = "Location is required")] string Location,
    [property: Required(ErrorMessage = "Cavern name is required")] string CavernName,
    int Capacity
);
