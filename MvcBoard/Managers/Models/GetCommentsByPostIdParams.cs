namespace MvcBoard.Managers.Models
{
    public class GetCommentsByPostIdParams
    {
        public int PostId { get; set; }
        public int Page { get; set; }
        public int CommentPageSize { get; set; } = 10; // 한 페이지에 노출되는 댓글 수

        // TODO 이게 맞나
        public int PostUserId { get; set; }
        public int Category {  get; set; }
        public int CommentPage { get; set; }
    }
}
