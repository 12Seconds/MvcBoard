namespace MvcBoardAdmin.Models.Member
{
    public class MemberListViewModel
    {
        public List<User> MemberList { get; set; } = new List<User>();

        /* 페이지 인디케이터는 뺄 수 있으면 따로 뺄 것 (public class PageInfo) */
        public const int DisplayPageCount = 5;
        public int PageCount { get; set; } = 0;
        public int PageIndex { get; set; } = 1;
        public int RowPerPage { get; set; } = 10;
        public int TotalRowCount { get; set; } = 0;
    }
}
