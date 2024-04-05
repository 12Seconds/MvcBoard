using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;

namespace MvcBoardAdmin.Managers.Results
{
    public class ReadPostDetailResult
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        // public PostEditorViewModel ViewModel { get; set; } = new PostEditorViewModel();
        public PostWithUser Post { get; set; }
    }
}
