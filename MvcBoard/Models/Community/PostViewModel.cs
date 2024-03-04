namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시글 뷰 모델
    /// </summary>
    public class PostViewModel : BoardViewModel // 굳이 상속 안받아도 될 것 같긴 한데..
    {
        /*
        public PostViewModel(Post postData, int pageCount = 0, int pageIndex = 1, List<Post>? postListData = null) : base(pageCount, pageIndex, postListData)
        {
            PostData = postData;
            // TODO CommentListData
        }
        */

        public Post? PostData { get; set; }

        // TODO
        // public <List>Comment CommentListData { get; set; }
    }
}
