using Microsoft.AspNetCore.Mvc;
using MvcBoard.Controllers.Models;
using MvcBoard.Models.Community;
using MvcBoard.Services;

namespace MvcBoard.Controllers
{
    public class CommunityController : Controller
    {
        private readonly CommunityService _service;

        public CommunityController(CommunityService service)
        {
            _service = service;
        }

        // 전체 게시판
        public IActionResult Index(BoardViewParams? _params)
        {
            BoardViewModel viewModel = _service.GetBoardViewData(_params ?? new BoardViewParams());

            return View(viewModel);
        }

        // 인기 게시판
        public IActionResult Hot(BoardViewParams? _params)
        {
            // TODO 별도 SP 및 함수 만들어서 호출 필요, 지금은 임시로 Category 값이 2이면 인기
            BoardViewModel viewModel = _service.GetBoardViewData(new BoardViewParams { Category = 2 });

            return View(viewModel);
        }

        // 공지 게시판
        public IActionResult Notice(BoardViewParams? _params)
        {
            // TODO 별도 함수 만들어서 호출 필요 (isNotice 컬럼 추가하고 GetNoticeBoardViewData(_params) 호출 필요함, 지금은 임시로 Category 값이 99면 공지
            BoardViewModel viewModel = _service.GetBoardViewData(new BoardViewParams{ Category = 99 }); // 임시

            return View(viewModel);
        }

        // 게시물 작성
        public IActionResult Write(WritePostParams _params)
        {
            Post? viewModel = null;

            if (_params.PostId != null && _params.PostId > 0)
            {
                viewModel = _service.GetPostWithUserById(_params.PostId); // PostWithUser -> Post 캐스팅 일어남
            }

            return View(viewModel ?? new Post());
        }

        // 게시물 작성 or 수정
        [HttpPost]
        public IActionResult Write(Post postData) // TODO 파라미터의 값을 바꾸는게 맞나...이래도 되나...
        {
            Console.WriteLine($"[Controller] Community >> Write() postData.Category: {postData.Category},  IsValid: {ModelState.IsValid}"); // TODO stringify

            if (ModelState.IsValid)
            {
                if (postData.Category == 0) postData.Category = 1; // 1: 자유게시판 (default), todo 드롭다운 or 모달 구현하면 필요 없음

                // TODO 작성자 UserId 매핑 필요 (로그인 구현 후)
                if (postData.PostId == 0)
                {
                    postData.UserId = 1;
                }

                if (postData.PostId == 0)
                {
                    _service.CreatePost(postData);
                }
                else if (postData.PostId > 0)
                {
                    _service.UpdatePost(postData);
                }

                // todo 정의될 category 에 맞게 수정 필요
                switch (postData.Category)
                {
                    case 0:
                    case 1: return RedirectToAction("Index");
                    case 2: return RedirectToAction("Hot");
                    case 99: return RedirectToAction("Notice");

                    default: return RedirectToAction("Index");
                }

                // ModelState.AddModelError(string.Empty, "게시물을 저장할 수 없습니다."); // TODO
            }
            return View(postData);
        }

        // 게시물 상세 페이지 (진입 화면)
        [HttpGet("Community/View/{PostId}")]
        public IActionResult View(PostViewParams _params)
        {
            // 게시물 데이터 조회 (by postId)
            PostWithUser? postData = _service.GetPostWithUserById(_params.PostId);

            if (postData == null)
            {
                // TODO IExceptionFilter 를 구현한 별도 예외 처리 로직 및 뷰 만들어서 넘기기 (존재하지 않는 게시물 입니다.)
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // TODO CommentsViewParams 과 PostViewParams 사실상 같음

                // 댓글 데이터 조회
                CommentsViewModel commentListData = _service.GetCommentByPostId(new CommentsViewParams { PostId = _params.PostId, Page = _params.Page, CommentPage = _params.CommentPage, Category = _params.Category});
                
                // 게시물 하단 게시판 데이터 조회
                BoardViewModel boardViewModel = _service.GetBoardViewData(_params); // TODO 업캐스팅 되나?
                PostViewModel viewModel = new PostViewModel(postData, boardViewModel.PageCount, boardViewModel.Page, boardViewModel.Category, boardViewModel.PageSize, boardViewModel.PostListData);

                viewModel.CommentListModel = commentListData; // TODO 위에 (....) 생성자 방식 수정할 것
                return View(viewModel);
            }
        }

        // TODO 게시물 수정, 삭제 개발 필요
        // TODO   댓글 수정, 삭제 개발 필요

        // 댓글 작성
        [HttpPost]
        public IActionResult WriteComment(WriteCommentParams commentParams) 
        {
            Console.WriteLine($"## CommunityController >> WriteComment() PostId: {commentParams.PostId}, UserId: {commentParams.UserId}, ParentId: {commentParams.ParentId}, Contents: {commentParams.Contents}, IsAnonymous: {commentParams.IsAnonymous}");

            _service.CreateComment(commentParams); // TODO 업캐스팅
          
            return PartialView("_Comments", _service.GetCommentByPostId(commentParams.ViewParams));
        }

    }
}
