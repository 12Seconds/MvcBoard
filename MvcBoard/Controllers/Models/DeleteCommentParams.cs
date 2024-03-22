namespace MvcBoard.Controllers.Models
{
    public class DeleteCommentParams : CommentsViewParams// TODO PostViewParams, CommentsViewParams 와 사실상 동일
    {
        /*
        public int PostId { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int CommentPage { get; set; } = 1;
        public int CurrentLoginUserNumber { get; set; } = 0;
        */
        public int CommentId { get; set; } = 0;
    }
}
