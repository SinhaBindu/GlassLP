using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace GlassLP.DTO
{
    public class AuthDTO
    {
    }

    public class LoginDto
    {
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
