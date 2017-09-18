namespace Game.Api.DataAccess.Models.Identity
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public int FailedLoginAttemptsCount { get; set; }
    }
}
