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

        public IActionResult Index(PostManageViewParams _params)
        {
            PostManageViewModel Model = _postService.GetPostManageViewModel(/* _params */);
            Model.Params = _params;

            Model.Params.SearchFilter2 = _params.SearchFilter2; // TODO 지울 것 테스트

            return View(Model);
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

            PostListViewModel Model = _postService.GetPostListViewModel(ServiceParams);

            return PartialView("_PostList", Model);
        }

        /* 게시물 정보 에디터 PartialView (상세 조회) */
        [HttpGet]
        public IActionResult PostEditorPartial(int PostId)
        {
            Console.WriteLine($"## PostController >> PostEditorPartial(PostId: {PostId}");

            GetPostEditorServiceParams ServiceParams = new GetPostEditorServiceParams
            {
                PostId = PostId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            PostEditorViewModel Model = _postService.GetPostEditorViewModel(ServiceParams);

            return PartialView("_PostEditor", Model);
        }

        /* 게시물 정보 수정 (게시판 이동, 숨김(블라인드), 삭제, 영구삭제 포함) */
        [HttpPost]
        public IActionResult Update(UpdatePostParams _params)
        {
            Console.WriteLine($"## PostController >> Update(PostId: {_params.PostId}, Category: {_params.Category}, IsBlinded: {_params.IsBlinded}, IsDeleted: {_params.IsDeleted}, IsHardDelete: {_params.IsHardDelete})");
           
            UpdatePostServiceParams ServiceParams = new UpdatePostServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _postService.UpdatePost(ServiceParams);

            return Ok(Response);
        }

        /* 게시물 삭제, 숨김(블라인드) */
        [HttpPost]
        public IActionResult Delete(DeletePostParams _params)
        {
            Console.WriteLine($"## PostController >> Delete(PostId: {_params.PostId}, IsBlinded: {_params.IsBlinded}, IsHardDelete: {_params.IsHardDelete})");
            
            DeletePostServiceParams ServiceParams = new DeletePostServiceParams
            {
                DeleteParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _postService.DeletePost(ServiceParams);


            return Ok(Response);
        }


    }
}
