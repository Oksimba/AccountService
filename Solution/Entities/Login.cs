using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Login
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
