namespace MvcBoard.Models.Community
{
    public class CommentsViewParams
    {
        // [FromQuery]
        public int PostId { get; set; } = 0;
        // [FromQuery]
        public int Page { get; set; } = 1;
        // [FromQuery]
        public int CommentPage { get; set; } = 1;
        // [FromQuery]
        public int Category { get; set; } = 0;
    }
}
