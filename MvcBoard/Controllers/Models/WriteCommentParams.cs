namespace MvcBoard.Controllers.Models
{
    public class WriteCommentParams : Comment
    {
        public CommentsViewParams ViewParams { get; set; }
    }
}
