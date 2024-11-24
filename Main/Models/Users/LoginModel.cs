namespace Main.Models.Users
{
    public class LoginModel
    {

        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool Verificou { get; set; }

    }
}
