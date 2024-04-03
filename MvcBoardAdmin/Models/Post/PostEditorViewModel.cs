using MvcBoardAdmin.Controllers.Response;

namespace MvcBoardAdmin.Models.Post
{
    public class PostEditorViewModel
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public PostWithUser Post { get; set; } = new PostWithUser();  // PostDetail 만들어서 분리?
        public List<BoardType> WritableBoards { get; set; } = new List<BoardType>();
    }
}
