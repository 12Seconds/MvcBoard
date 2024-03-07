namespace MvcBoard.Models.Community
{
    /// <summary>
    /// 게시판 뷰 모델
    /// </summary>
    public class BoardViewModel
    {
        public BoardViewModel(int pageCount = 1, int? page = null, int? category = null, int pageSize = 20, List<PostWithUser>? postListData = null)
        {
            PageCount = pageCount;
            Page = page;
            Category = category;
            PageSize = pageSize;

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
        public const int DisplayPageCount = 5; // 인디케이터에 노출되는 최대 페이지 수 (홀수)
        public int PageCount { get; set; } // 총 페이지 수
        public int? Page { get; set; } // 현재 페이지 인덱스
        public int? Category { get; set; } // 현재 카테고리
        public int PageSize { get; set; } // 한 페이지에 노출되는 게시물 수 (TODO nullable?)
        public List<PostWithUser> PostListData { get; set; } // Q. nullable 로 해야 할까? nullable로 하면 생성자 안쓸 수 있음 (프론트 쪽에서 null check 필요성)
    }
}
