using Inferno.src.Core.Domain.Enums;

namespace Inferno.src.Core.Domain.Entities
{
    public class Hell
    {
        public Guid IdHell { get; set; }
        public string? HellName { get; set; }
        public string Descricao { get; set; }
        public HellLevel Nivel { get; set; } = HellLevel.Inferior;

        public Hell() { }
    }
}
