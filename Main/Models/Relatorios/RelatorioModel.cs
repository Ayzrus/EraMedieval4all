using Main.Class;

namespace Main.Models.Relatorios
{
	public class RelatorioModel
	{

		public required IEnumerable<ClientesClass> Cliente { get; set; }
		public required IEnumerable<TrajesClass> Trajes { get; set; }
		public required IEnumerable<ClientesClass> ClienteV { get; set; }

	}
}
