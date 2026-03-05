using System.ComponentModel;

namespace Inferno.src.Core.Domain.Enums;

public enum HellTaskStatus
{
    [Description("NotStarted")]
    NotStarted = 1,

    [Description("InProgress")]
    InProgress = 2,

    [Description("Completed")]
    Completed = 3,
}
