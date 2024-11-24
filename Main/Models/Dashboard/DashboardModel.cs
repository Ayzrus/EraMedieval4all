using Main.Class;

namespace Main.Models.Dashboard
{
    public class DashboardModel
    {

        public required int Aluguer { get; set; }
        public required int TrajesAlugados { get; set; }
        public required int TrajesRegistados { get; set; }
        public required decimal ValorTotal { get; set; }
		public required IEnumerable<ClientesClass> Cliente { get; set; }
		public required IEnumerable<ClientesClass> ClienteN { get; set; }
		public required IEnumerable<TrajesClass> Trajes { get; set; }

	}
}
