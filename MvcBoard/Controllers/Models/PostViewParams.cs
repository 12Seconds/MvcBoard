using Microsoft.AspNetCore.Mvc;

namespace MvcBoard.Controllers.Models
{
    public class PostViewParams : BoardViewParams
    {
        // [FromQuery]
        public int PostId { get; set; } = 0;
        // [FromQuery]
        public int CommentPage { get; set; } = 1;
    }
}
