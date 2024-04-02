using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class UpdateBoardParams
    {
        [Required(ErrorMessage = "게시판 고유 번호를 입력해주세요.")]
        public int BoardId { get; set; } = 0;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "게시판 이름은 최소 2자 이상, 최대 50자 이하로 입력해주세요.")]
        public string BoardName { get; set;}

        [Required]
        public int Category { get; set; } // 변경할 카테고리 번호
        public int PrevCategory { get; set; } // 변경 전 카테고리 번호 (카테고리 번호를 변경하지 않는 수정인 경우, 카테고리 중복 검사를 건너뛰기 위함)

        public int ParentCategory { get; set; } = 0;
        public bool IsParent { get; set; } = false;
        public int IconType { get; set; } = 1;
        public bool IsWritable { get; set; } = false;
        public int ShowOrder { get; set; } = 0;
    }
}
