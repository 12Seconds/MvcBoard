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

        // 임시
        public string SearchFilter { get; set; } = "";
        public string SearchWord { get; set; } = "";

    }
}
