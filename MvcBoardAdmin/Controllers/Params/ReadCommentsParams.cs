namespace MvcBoardAdmin.Controllers.Params
{
    // TODO 게시물 번호와 사용자 번호 필터를 SearchFilter 에서 분리하여 필터 depth 를 늘릴지 고민
    public class ReadCommentsParams
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
