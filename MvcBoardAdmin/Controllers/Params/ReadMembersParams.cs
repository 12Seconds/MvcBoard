using System.Security.Principal;

namespace MvcBoardAdmin.Controllers.Params
{
    public class ReadMembersParams
    {
        /// <summary>
        /// 검색 필터 옵션
        /// </summary>
        public string SearchFilter { get; set; }

        /// <summary>
        /// 검색어
        /// </summary>
        public string SearchWord { get; set; }

        /// <summary>
        /// 페이지 인덱스
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
