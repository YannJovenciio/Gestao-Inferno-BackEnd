using Inferno.src.Core.Domain.Enums;

namespace Inferno.src.Core.Domain.Entities;

public class HellTask
{
    public Guid HellTaskId { get; private set; }
    public string? Title { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime DeadLine { get; set; }
    public HellTaskStatus Status { get; private set; } = HellTaskStatus.NotStarted;
    public int Progress { get; private set; }

    //FK
    public virtual Demon? Demon { get; private set; }
    public Guid DemonId { get; private set; }

    public HellTask() { }

    public HellTask(string title, string description, Guid demonId)
    {
        HellTaskId = Guid.NewGuid();
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        DeadLine = CreatedAt.AddDays(30);
        DemonId = demonId;
    }

    public void UpdateProgress(int newProgress)
    {
        if (newProgress < 0 || newProgress > 100)
            throw new InvalidOperationException("Invalid progress provided");
        Progress = newProgress;
        if (Progress == 100)
            Status = HellTaskStatus.Completed;
        else if (Progress > 0 && Progress < 100)
            Status = HellTaskStatus.InProgress;
        else
            Status = HellTaskStatus.NotStarted;
    }
}
