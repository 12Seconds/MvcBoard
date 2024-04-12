using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Filters;
using MvcBoardAdmin.Models.Comment;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    /* 댓글 관리 컨트롤러 */
    [AuthenticationFilter]
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
        [HttpGet]
        public IActionResult CommentEditorPartial(int CommentId)
        {
            Console.WriteLine($"## CommentController >> CommentEditorPartial(CommentId: {CommentId}");

            GetCommentEditorServiceParams ServiceParams = new GetCommentEditorServiceParams
            {
                CommentId = CommentId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommentEditorViewModel Model = _commentService.GetCommentEditorViewModel(ServiceParams);

            return PartialView("_CommentEditor", Model);
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

        /* 댓글 정보 수정 (숨김(블라인드), 삭제, 영구삭제 포함) */
        [HttpPost]
        public IActionResult Update(UpdateCommentParams _params)
        {
            Console.WriteLine($"## CommentController >> Update(CommentId: {_params.CommentId}, IsBlinded: {_params.IsBlinded}, IsDeleted: {_params.IsDeleted}, ExDeleted: {_params.ExDeleted}, IsHardDelete: {_params.IsHardDelete})");
           
            UpdateCommentServiceParams ServiceParams = new UpdateCommentServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _commentService.UpdateComment(ServiceParams);

            return Ok(Response);
        }

        /* 댓글 삭제, 숨김(블라인드) */
        [HttpPost]
        public IActionResult Delete(DeleteCommentParams _params)
        {
            Console.WriteLine($"## CommentController >> Delete(CommentId: {_params.CommentId}, IsBlinded: {_params.IsBlinded}, IsHardDelete: {_params.IsHardDelete})");
            
            DeleteCommentServiceParams ServiceParams = new DeleteCommentServiceParams
            {
                DeleteParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _commentService.DeleteComment(ServiceParams);


            return Ok(Response);
        }

    }
}
