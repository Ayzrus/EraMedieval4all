using Main.Class;

namespace Main.Models.Trajes
{
    public class TrajesViewParcialModel
    {

        public required IEnumerable<ArmazemClass> Armazem { get; set; }
        public TrajesClass? Trajes { get; set; }
        public bool IsEdit { get; set; }

    }
}
