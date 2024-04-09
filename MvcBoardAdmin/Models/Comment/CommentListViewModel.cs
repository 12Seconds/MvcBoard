using MvcBoardAdmin.Controllers.Response;
using static MvcBoardAdmin.Utills.Utility;

namespace MvcBoardAdmin.Models.Comment
{
    public class CommentListViewModel
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public List<CommentWithUser> CommentList { get; set; } = new List<CommentWithUser>();
        public IndicatorRange IndicatorRange { get; set; } = new IndicatorRange();
        public int TotalRowCount { get; set; } = 0;
        public int TotalPageCount { get; set; } = 0;
        public int PageIndex { get; set; } = 1;

        /* 이전 검색 필터 및 검색어 (페이지 이동 처리 시 필요) */
        public int ExBoardFilter { get; set; } = 0; // 게시판의 Category 값
        public string ExSearchFilter { get; set; } = "";
        public string ExSearchWord { get; set; } = "";
    }
}
