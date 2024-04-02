namespace MvcBoardAdmin.Controllers.Params
{
    public class ReadPostsParams // TODO 초기값, Nullable, ModelState 확인 필요
    {
        /// <summary>
        /// 게시판 필터 옵션
        /// </summary>
        public int BoardFilter { get; set; } = 0;

        /// <summary>
        /// 검색 필터 옵션
        /// </summary>
        public string SearchFilter { get; set; } = "";

        /// <summary>
        /// 검색어
        /// </summary>
        public string? SearchWord { get; set; } = "";

        /// <summary>
        /// 페이지 인덱스
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
