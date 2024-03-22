namespace MvcBoard.Controllers.Models
{
    public class CommentsViewParams : PostViewParams
    {
        // 두 Params 멤버 변수는 사실상 같음
        /*
        // [FromQuery]
        public int PostId { get; set; } = 0;
        // [FromQuery]
        public int Page { get; set; } = 1;
        // [FromQuery]
        public int CommentPage { get; set; } = 1;
        // [FromQuery]
        public int Category { get; set; } = 0;
        */
        public int CurrentLoginUserNumber { get; set; } = 0;
        public bool IsLoggedIn { get; set; } = false;
    }
}
