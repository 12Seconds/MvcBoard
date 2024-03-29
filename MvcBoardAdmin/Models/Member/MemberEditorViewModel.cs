namespace MvcBoardAdmin.Models.Member
{
    public class MemberEditorViewModel : User
    {
        public int PostCount { get; set; } // 작성한 게시물 수 
        public int CommentCount { get; set; } // 작성한 댓글 수
    }
}
