namespace MvcBoardAdmin.Models.AdminMember
{
    public class AdminMemberEditorViewModel : AdmUser
    {
        public int PostCount { get; set; } // 작성한 게시물 수 (공지)
        public int CommentCount { get; set; } // 작성한 댓글 수
    }
}
