using MvcBoardAdmin.Controllers.Response;

namespace MvcBoardAdmin.Models.Comment
{
    public class CommentEditorViewModel
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public CommentDetail CommentDetail { get; set; } = new CommentDetail();
    }
}
