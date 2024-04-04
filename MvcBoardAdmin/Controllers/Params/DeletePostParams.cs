using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class DeletePostParams
    {
        [Required]
        public int PostId { get; set; }
        public bool IsBlinded { get; set; }
        public bool IsHardDelete { get; set; }
    }
}
