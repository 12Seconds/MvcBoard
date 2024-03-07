namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시판 뷰 모델
    /// </summary>
    public class BoardViewModel
    {
        public BoardViewModel(int? pageCount = 0, int? page = null, int? category = null, List<PostWithUser>? postListData = null)
        {
            PageCount = pageCount;
            Page = page;
            Category = category;

            if (postListData == null)
                PostListData = new List<PostWithUser>();
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

        public int? PageCount { get; set; }
        public int? Page { get; set; }
        public int? Category { get; set; } // 현재 카테고리
        public List<PostWithUser> PostListData { get; set; } // Q. nullable 로 해야 할까? nullable로 하면 생성자 안쓸 수 있음 (프론트 쪽에서 null check 필요성)
    }
}
