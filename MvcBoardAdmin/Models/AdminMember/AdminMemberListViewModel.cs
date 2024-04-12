using static MvcBoardAdmin.Utills.Utility;

namespace MvcBoardAdmin.Models.AdminMember
{
    public class AdminMemberListViewModel
    {
        public List<AdmUser> MemberList { get; set; } = new List<AdmUser>();
        public IndicatorRange IndicatorRange { get; set; } = new IndicatorRange();
        public int PageIndex { get; set; } = 1;
        public int TotalRowCount { get; set; } = 0;
        public int TotalPageCount { get; set; } = 0;

        /* 이전 검색 필터 및 검색어 (페이지 이동 처리 시 필요) */
        public string ExSearchFilter { get; set; } = "";
        public string ExSearchWord { get; set; } = "";
    }
}
