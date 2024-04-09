using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;

namespace MvcBoardAdmin.Managers.Results
{
    public class ReadCommentDetailResult
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public CommentDetail CommentDetail { get; set; }
    }
}
