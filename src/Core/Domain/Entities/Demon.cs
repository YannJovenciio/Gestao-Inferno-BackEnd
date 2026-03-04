using System.Text.Json.Serialization;
using Inferno.src.Core.Domain.Entities.ManyToMany;

namespace Inferno.src.Core.Domain.Entities;

public class Demon
{
    public Guid IdDemon { get; set; }
    public Guid? CategoryId { get; set; } = null;
    public string? DemonName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int? ImageId { get; set; }
    public virtual Image? Image { get; set; }

    public virtual Category Category { get; set; }
    public virtual ICollection<HellTask> HellTasks { get; set; }

    [JsonIgnore]
    public virtual ICollection<Persecution> Persecutions { get; set; } = new List<Persecution>();

    public Demon() { }

    public Demon(string demonName, Guid? category)
    {
        DemonName = demonName;
        CategoryId = category;
    }

    public override string ToString()
    {
        return $"Demon{{IdDemon={IdDemon}, DemonName={DemonName}, CategoryId={CategoryId}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}}}";
    }
}
