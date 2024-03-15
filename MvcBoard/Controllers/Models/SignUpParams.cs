using System.ComponentModel.DataAnnotations;

namespace MvcBoard.Controllers.Models
{
    public class SignUpParams : LoginParams
    {
        [Required]
        public String Name { get; set; }
        [Required]
        public int Image {  get; set; }
    }
}
