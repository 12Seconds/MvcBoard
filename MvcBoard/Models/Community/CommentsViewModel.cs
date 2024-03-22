namespace MvcBoard.Models.Community
{
    public class CommentsViewModel
    {
        public List<CommentWithUser> CommentListData { get; set; } = new List<CommentWithUser>();

        public const int DisplayPageCount = 5; // 인디케이터에 노출되는 최대 댓글 수 (홀수)
        public int? CommentPage { get; set; } = 1; // 현재 댓글 페이지 인덱스
        public int CommentPageSize { get; set; } = 10; // 한 페이지에 노출되는 댓글 수 (TODO nullable?)
        public int CommentPageCount { get; set; } = 0; // 총 댓글 페이지 수
        public int CommentTotalCount { get; set; } = 0; // 총 댓글 수 

        // TODO 이게 맞나...? 게시물 id 와 게시판 정보 (카테고리, 페이지)를 유지한 채 댓글페이지만 변경할 수가 없나..결국 거의 모든 정보가 다 필요하네
        public int PostId { get; set; } // 현재 게시물 아이디
        public int Category { get; set; } // 현재 카테고리
        public int Page { get; set; } // 현재 게시판 페이지 인덱스
    }
}
