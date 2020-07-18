using System.ComponentModel.DataAnnotations;

namespace LJSS.Models
{
    public class Login
    {
        [Required]
        //public string Email { get; set; }
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}