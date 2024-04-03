using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models.Post;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    /* 게시물 관리 컨트롤러 */
    public class PostController : Controller
    {
        private readonly PostService _postService;
        public PostController(PostService postService) 
        {
            _postService = postService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /* 게시물 리스트 PartialView (검색) */
        [HttpGet]
        public IActionResult PostListPartial(ReadPostsParams _params)
        {
            Console.WriteLine($"## PostController >> PostListPartial(SearchFilter: {_params.SearchFilter}, SearchWord: {_params.SearchWord}, Page: {_params.Page})");

            ReadPostsServiceParams ServiceParams = new ReadPostsServiceParams
            {
                ReadPostsParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            ReadPostsResponse Response = _postService.ReadPosts(ServiceParams);

            return PartialView("_PostList", Response);
        }

        /* 게시물 정보 에디터 PartialView (상세 조회) */
        [HttpGet]
        public IActionResult PostEditorPartial(int PostId)
        {
            Console.WriteLine($"## PostController >> PostEditorPartial(PostId: {PostId}");

            ReadPostDetailServiceParams ServiceParams = new ReadPostDetailServiceParams
            {
                PostId = PostId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            PostEditorViewModel Model = _postService.GetPostEditorViewModel(ServiceParams);

            return PartialView("_PostEditor", Model);
        }

        /* 게시물 정보 수정 */
        [HttpPost]
        public IActionResult Update(int postId, int isBlind) // 임시
        {
            Console.WriteLine($"## PostController >> Update(postId: {postId}, isBlind: {isBlind})");
            /*
            UpdateBoardServiceParams ServiceParams = new UpdateBoardServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };
            */

            // CommonResponse Response = _postService.UpdateBoard(ServiceParams);
            CommonResponse Response = new CommonResponse
            {
                ResultCode = 201,
                Message = "미구현"
            };

            return Ok(Response);
        }

        /* 게시물 삭제 or 블라인드 */
        [HttpPost]
        public IActionResult Delete(int postId, int isBlind) // 임시
        {
            Console.WriteLine($"## PostController >> Update(postId: {postId}, isBlind: {isBlind})");
            /*
            DeleteBoardServiceParams ServiceParams = new DeleteBoardServiceParams
            {
                BoardId = BoardId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };
            */

            // CommonResponse Response = _postService.DeleteBoard(ServiceParams);
            CommonResponse Response = new CommonResponse
            {
                ResultCode = 201,
                Message = "미구현"
            };

            return Ok(Response);
        }


    }
}
