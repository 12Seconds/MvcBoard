using MvcBoardAdmin.Controllers.Params;

namespace MvcBoardAdmin.Models.Comment
{
    // (CommentIndexViewModel)
    public class CommentManageViewModel
    {
        public CommentManageViewParams? Params { get; set; } = null;
        public List<BoardType> WritableBoards { get; set; } = new List<BoardType>();
    }
}
