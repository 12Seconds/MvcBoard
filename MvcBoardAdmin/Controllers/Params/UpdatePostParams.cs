using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Controllers.Params
{
    public class UpdatePostParams
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public int Category { get; set; }
        public bool IsBlinded { get; set; }
        public bool IsDeleted { get; set; }
        public bool ExDeleted { get; set; } // 이미 삭제된 게시물인 경우, 삭제 날짜를 업데이트 예외 처리하기 위함
        public bool IsHardDelete { get; set; }
    }
}
