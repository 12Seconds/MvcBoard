using Microsoft.AspNetCore.Mvc;

namespace MvcBoard.Models.Community
{
    public class PostViewParams : BoardViewParams
    {
        // [FromQuery]
        public int PostId { get; set; } = 0;
        // [FromQuery]
        public int CommentPage { get; set; } = 1;
    }
}
