using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Models
{
    public class SignUpModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
