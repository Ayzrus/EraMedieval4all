namespace Main.Models.Clientes
{
    public class ClientesModel
    {

        public required int Id { get; set; }
        public required string Nif { get; set; }
        public required string Nome { get; set; }
        public required string Morada { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }

    }
}
