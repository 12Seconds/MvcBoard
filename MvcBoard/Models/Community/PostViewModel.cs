namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시글 뷰 모델
    /// </summary>
    public class PostViewModel : BoardViewModel // 굳이 상속 안받아도 될 것 같긴 한데..
    {
        // TODO 생성자 구조 바꿀 것..
        public PostViewModel(PostWithUser postData, int pageCount = 1, int? page = 1, int? category = null, int pageSize = 20, List<PostWithUser>? postListData = null) : base(pageCount, page, category, pageSize, postListData)
        {
            PostData = postData;
            /*
            PageCount = pageCount;
            PageIndex = pageIndex;
            postListData = postListData ?? new List<PostWithUser>();
            */

            // TODO CommentListData
        }

        public PostWithUser PostData { get; set; }
        public CommentsViewModel CommentListModel { get; set; } // = new CommentsViewModel();
    }
}
