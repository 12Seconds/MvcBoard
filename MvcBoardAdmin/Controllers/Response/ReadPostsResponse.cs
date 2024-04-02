using MvcBoardAdmin.Models.Post;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadPostsResponse : CommonResponse
    {
        public PostListViewModel ViewModel { get; set; } = new PostListViewModel();
    }
}
