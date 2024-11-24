namespace Main.Models.Users
{
    public class UserRegistrationModel
    {

        public required string Nome { get; set; }
        public required string Morada { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Pin { get; set; }
        public required int TipoUser { get; set; }

    }
}
