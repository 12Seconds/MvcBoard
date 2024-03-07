namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시글 뷰 모델
    /// </summary>
    public class PostViewModel : BoardViewModel // 굳이 상속 안받아도 될 것 같긴 한데..
    {
        
        public PostViewModel(PostWithUser postData, int? pageCount = 0, int? page = 1, int? category = null, List<PostWithUser>? postListData = null) : base(pageCount, page, category, postListData)
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

        // TODO
        // public <List>Comment CommentListData { get; set; }
        // public int CommentPage { get; set; }
        // public int CommentPageCount { get; set; }
    }
}
