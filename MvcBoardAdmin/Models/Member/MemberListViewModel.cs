using static MvcBoardAdmin.Utills.Utility;

namespace MvcBoardAdmin.Models.Member
{
    public class MemberListViewModel
    {
        public List<User> MemberList { get; set; } = new List<User>();
        public IndicatorRange IndicatorRange { get; set; } = new IndicatorRange();
        public int PageIndex { get; set; } = 1;
        public int TotalRowCount { get; set; } = 0;
        public int TotalPageCount { get; set; } = 0;

        /* 이전 검색 필터 및 검색어 (페이지 이동 처리 시 필요) */
        public string ExSearchFilter { get; set; } = "";
        public string ExSearchWord { get; set; } = "";
    }
}
