using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Models.Comment;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    /* 댓글 관리 컨트롤러 */
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService) 
        {
            _commentService = commentService;
        }

        public IActionResult Index(CommentManageViewParams _params) // CommentManageViewParams
        {
            CommentManageViewModel Model = _commentService.GetCommentManageViewModel(/* _params */);
            Model.Params = _params;

            return View(Model);
        }

        /* 댓글 리스트 PartialView (검색) */
        [HttpGet]
        public IActionResult CommentListPartial(ReadCommentsParams _params)
        {
            Console.WriteLine($"## CommentController >> CommentListPartial(BoardFilter: {_params.BoardFilter}, SearchFilter: {_params.SearchFilter}, SearchWord: {_params.SearchWord}, Page: {_params.Page})");

            ReadCommentsServiceParams ServiceParams = new ReadCommentsServiceParams
            {
                ReadCommentsParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommentListViewModel Model = _commentService.GetCommentListViewModel(ServiceParams);

            return PartialView("_CommentList", Model);
        }

        /* 댓글 정보 에디터 PartialView (상세 조회) */
        /*
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

        /* 댓글 정보 상세 화면 PartialView (상세 조회) */
        [HttpGet]
        public IActionResult CommentDetailPartial(int CommentId)
        {
            Console.WriteLine($"## CommentController >> CommentDetailPartial(CommentId: {CommentId}");

            GetCommentDetailServiceParams ServiceParams = new GetCommentDetailServiceParams
            {
                CommentId = CommentId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommentDetailViewModel Model = _commentService.GetCommentDetailViewModel(ServiceParams);

            return PartialView("_CommentDetail", Model);
        }

        /* 게시물 정보 수정 (게시판 이동, 숨김(블라인드), 삭제, 영구삭제 포함) */
        /*
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
        /*
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

        */
    }
}
