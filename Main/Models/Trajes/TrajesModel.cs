namespace Main.Models.Trajes
{
    public class TrajesModel
    {

        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required string Valor { get; set; }
        public required int Armazem { get; set; }
        public required IFormFile Foto { get; set; }
        public string? FotoNome { get; set; }
        public required int Quantidade { get; set; }
        public required string Especificacao { get; set; }
        public required string Tipo { get; set; }
        public required string Ref { get; set; }

    }
}
