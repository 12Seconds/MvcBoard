namespace MvcBoard.Models.Community
{
    public class WriteCommentParams : Comment
    {
        public CommentsViewParams ViewParams { get; set; }
    }
}
