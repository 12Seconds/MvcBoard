using MvcBoardAdmin.Models.Board;

namespace MvcBoardAdmin.Controllers.Response
{
    public class BoardDetailResponse : CommonResponse
    {
        public BoardEditorViewModel ViewModel { get; set; } = new BoardEditorViewModel();
    }
}
