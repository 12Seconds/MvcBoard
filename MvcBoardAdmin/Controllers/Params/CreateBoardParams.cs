using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class CreateBoardParams
    {
        public int BoardId { get; set; } = 0;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "게시판 이름은 최소 2자 이상, 최대 50자 이하로 입력해주세요.")]
        public string BoardName { get; set;}

        [Required]
        public int Category { get; set; }
        public int ParentCategory { get; set; } = 0;
        public bool IsParent { get; set; } = false;
        public int IconType { get; set; } = 1;
        public bool IsWritable { get; set; } = false;
        public int ShowOrder { get; set; } = 0;
    }
}
