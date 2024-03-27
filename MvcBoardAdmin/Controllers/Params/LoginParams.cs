using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class LoginParams
    {
        // 상용 서비스에서는 다음과 같은 추가적인 입력값 검증 옵션이 필요할 것
        // [StringLength(20, MinimumLength = 6, ErrorMessage = "아이디는 최소 6자 이상, 최대 20자 이하로 입력해주세요.")]
        // [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "아이디는 영문자와 숫자로만 이루어져야 합니다.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "입력한 아이디가 올바르지 않습니다.")]
        public string Id { get; set; }

        // 상용 서비스에서는 다음과 같은 추가적인 입력값 검증 옵션이 필요할 것
        // [StringLength(20, MinimumLength = 8, ErrorMessage = "비밀번호는 최소 8자 이상, 최대 20자 이하로 입력해주세요.")]
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "비밀번호는 영문 대문자, 영문 소문자, 숫자, 특수문자를 최소 한 개씩 포함해야 합니다.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "입력한 비밀번호가 올바르지 않습니다.")]
        public string Password { get; set; }

    }
}
