using MvcBoardAdmin.Controllers.Params;

namespace MvcBoardAdmin.Models.Post
{
    // (PostIndexViewModel)
    public class PostManageViewModel 
    {
        public PostManageViewParams? Params { get; set; } = null;
        public List<BoardType> WritableBoards { get; set; } = new List<BoardType>();
    }
}
