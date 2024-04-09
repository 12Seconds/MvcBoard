using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
using static MvcBoardAdmin.Utills.Utility;

namespace MvcBoardAdmin.Managers.Results
{
    public class ReadCommentsResult
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public List<CommentWithUser> CommentList { get; set; } = new List<CommentWithUser>();
        public IndicatorRange IndicatorRange { get; set; } = new IndicatorRange();
        public int TotalRowCount { get; set; } = 0;
        public int TotalPageCount { get; set; } = 0;
    }
}
