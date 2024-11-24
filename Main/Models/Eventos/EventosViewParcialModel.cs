using Main.Class;

namespace Main.Models.Eventos
{
    public class EventosViewParcialModel
    {

        public required IEnumerable<OrganizadorClass> Organizadores { get; set; }
        public EventosClass? Evento { get; set; }
        public bool IsEdit { get; set; }

    }
}
