using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class UpdateMemberParams
    {
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "닉네임은 최소 2자 이상, 최대 50자 이하로 입력해주세요.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "닉네임을 입력해주세요.")]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "0~100 사이의 값만 입력할 수 있습니다.")]
        public int Image { get; set; } = 0;

        [Required(AllowEmptyStrings = false, ErrorMessage = "권한을 입력해주세요.")]
        public string Authority { get; set; } = "";
    }
}
