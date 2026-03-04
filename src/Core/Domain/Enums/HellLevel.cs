using System.ComponentModel;

namespace Inferno.src.Core.Domain.Enums;

public enum HellLevel
{
    [Description("Inferior")]
    Inferior = 1,

    [Description("Medio")]
    Medio = 2,

    [Description("Superior")]
    Superior = 3,
}
