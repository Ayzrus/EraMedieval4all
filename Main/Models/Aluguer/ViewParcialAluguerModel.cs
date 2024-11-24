using Main.Class;

namespace Main.Models.Aluguer
{
    public class ViewParcialAluguerModel
    {
        public required IEnumerable<ClientesClass> Cliente { get; set; }
        public required IEnumerable<EventosClass> Eventos { get; set; }
        public required IEnumerable<TrajesClass> Trajes { get; set; }
        public AluguerClass? Aluguer { get; set; }
        public bool IsEdit { get; set; }

    }
}
