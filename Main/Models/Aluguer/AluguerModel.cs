namespace Main.Models.Aluguer
{
    public class AluguerModel
    {

        public required int Id { get; set; }
        public string? DataEntrega { get; set; }
        public int Cliente { get; set; }
        public int Evento { get; set; }
        public int[]? TrajesId { get; set; }

    }
}
