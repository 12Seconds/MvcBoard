namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시판 뷰 모델
    /// </summary>
    public class BoardViewModel
    {
        public BoardViewModel(int pageCount = 0, int pageIndex = 1, List<Post>? postListData = null)
        {
            PageCount = pageCount;
            PageIndex = pageIndex;

            if (postListData == null)
                PostListData = new List<Post>();
            else
                PostListData = postListData; // 그냥 대입해도 문제 없나? 안되면 아래 로직
            
            // PostsData = new List<Post>();
            // if (postData != null)
            // {
            //     foreach (var post in postData)
            //     {
            //         postData.Add(post);
            //     }
            // }
        }

        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public List<Post> PostListData { get; set; } // Q. nullable 로 해야 할까? nullable로 하면 생성자 안쓸 수 있음 (프론트 쪽에서 null check 필요성)
    }
}
