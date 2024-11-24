namespace Main.Models.Eventos
{
    public class EventosModel
    {

        public required int Id { get; set; }
        public required string Localidade { get; set; }
        public required string Descricao { get; set; }
        public required string Titulo { get; set; }
        public required string DataInicio { get; set; }
        public required string DataFim { get; set; }
        public required string Facebook { get; set; }
        public required string Instagram { get; set; }
        public required string TikTok { get; set; }
        public required int Organizador { get; set; }

    }
}
