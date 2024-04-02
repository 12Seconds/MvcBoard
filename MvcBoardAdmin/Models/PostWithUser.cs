namespace MvcBoardAdmin.Models
{
    public class PostWithUser : PostModel
    {
        // public int UserId { get; set; } // Id (UserNumber), Post 에 이미 정의됨
        // public string LoginId { get; set; } // Id를 Number 로 변경 후, UserId로 변경 필요 (필요 없으면 삭제,  현재는 안씀)

        public string UserName { get; set; } // 닉네임
        public int UserImage { get; set; } // 프로필 이미지

        public bool IsCurrunLoginUser = false;
        public string BoardName { get; set; } = "";
        // TODO 날짜 형식 변환하여 string 으로 저장하는 필드 추가할 것

        public int CommentCount { get; set; }
    }
}
