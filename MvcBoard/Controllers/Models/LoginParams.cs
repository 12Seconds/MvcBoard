using System.ComponentModel.DataAnnotations;

namespace MvcBoard.Controllers.Models
{
    public class LoginParams
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
