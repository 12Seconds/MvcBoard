using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcBoard.Controllers.Models;
using MvcBoard.Managers.JWT;
using MvcBoard.Models.Community;
using MvcBoard.Services;
using System.Security.Claims;

namespace MvcBoard.Controllers
{
    public class CommunityController : Controller
    {
        private readonly CommunityService _service;
        private readonly JWTManager _jwtManager;

        public CommunityController(CommunityService service, JWTManager jwtManager)
        {
            _service = service;
            _jwtManager = jwtManager;
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

        // 게시물 작성 화면 이동 - 자동 인증
        /*
        [Authorize]
        public IActionResult Write(WritePostParams _params)
        {
            Post? viewModel = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            else
            {
                Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");
            }

            if (_params.PostId != null && _params.PostId > 0)
            {
                viewModel = _service.GetPostWithUserById(_params.PostId); // PostWithUser -> Post 캐스팅 일어남
            }

            Console.WriteLine($"@@@@@@@@@@ Write category: {_params.Category}");

            // return View("Write", viewModel ?? new Post());
            // return Ok(new { key1 = value1, key2 = value2 });

            // 아니 이러면 수정인 경우 기존 Post 데이터를 뷰로 못넘기잖아...
            return Ok(new {  }); 
        }
        */

        // 게시물 작성 화면 이동 - 수동 인증
        
        public IActionResult Write(WritePostParams _params) // public string jwtToken { get; set; } = ""; 추가
        {
            Post? viewModel = null;

            // 쿠키 받아올 수 있음 -> 근데 이게 클라이언트 구분이 되는 건가?
            string cookie = Request.Cookies["jwtToken"] ?? ""; // ControllerBase 의 Requst (Httpcontext 도 있음)

            // if (User.Identity == null || !User.Identity.IsAuthenticated)
            // Console.WriteLine($"IsAuthenticated: {User.Identity.IsAuthenticated}");

            Console.WriteLine($"@@@@@@@@@@ cookie: {cookie}");
           
            ClaimsPrincipal Principal = _jwtManager.ValidateJwtToken(cookie);

            // 인증 성공
            if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
            {
                if (_params.PostId != null && _params.PostId > 0)
                {
                    viewModel = _service.GetPostWithUserById(_params.PostId); // PostWithUser -> Post 캐스팅 일어남
                }

                Console.WriteLine($"@@@@@@@@@@ Write category: {_params.Category}");
                Console.WriteLine($"@@@@@@@@@@ cookie: {cookie}");

                return View("Write", viewModel ?? new Post());
            }
            // TODO 인증 실패 처리
            else
            {
                Console.WriteLine($"@@@@@@@@@@ 인증 실패");
                return RedirectToAction("Index", "User");
            }
        }


       
        // 게시물 작성 or 수정
        [HttpPost]
        public IActionResult Write(Post postData) // TODO 파라미터의 값을 바꾸는게 맞나...이래도 되나...
        {
            Console.WriteLine($"[Controller] Community >> Write() postData.Category: {postData.Category},  IsValid: {ModelState.IsValid}"); // TODO stringify

            int UserNumber = 0;

            // 로그인 인증
            string cookie = Request.Cookies["jwtToken"] ?? "";
            Console.WriteLine($"@@@@@@@@@@ cookie: {cookie}");
            ClaimsPrincipal Principal = _jwtManager.ValidateJwtToken(cookie);
            if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
            {
                // string Id = Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // TODO UserNumber

                UserNumber = int.Parse(Principal.FindFirst(MvcBoardClaimTypes.UserNumber)?.Value);
                // string LoginId = Principal.FindFirst(MvcBoardClaimTypes.Id)?.Value;

            }
            else
            {
                // TODO 로그인 정보가 만료되었음을 알려야함 (또는 토큰 리프레쉬)
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                if (postData.Category == 0) postData.Category = 1; // 1: 자유게시판 (default), todo 드롭다운 or 모달 구현하면 필요 없음

                // TODO 작성자 UserId 매핑 필요 (로그인 구현 후)
                postData.UserId = UserNumber;

                Console.WriteLine($"@@@@@@@@@@ 바꿔치기 UserNumber: {UserNumber}");
                /*
                if (postData.PostId == 0)
                {
                    Console.WriteLine($"@@@@@@@@@@ 바꿔치기 UserNumber: {UserNumber}");
                    postData.UserId = 1;
                    // postData.UserId = UserNumber;
                }
                */

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

                // TODO Service 로 옮길 것?
                string cookie = Request.Cookies["jwtToken"] ?? "";

                Console.WriteLine($"@@@@@@@@@@ cookie: {cookie}");

                ClaimsPrincipal Principal = _jwtManager.ValidateJwtToken(cookie);

                // 인증 성공 - TODO 단순히 게시물을 읽는 건데 매번 내 게시물임을 알기 위해서 인증을 해야하는게 맞을까
                if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
                {
                    int userNumber = Convert.ToInt32(Principal.FindFirst(MvcBoardClaimTypes.UserNumber)?.Value);

                    if (postData.UserId == userNumber)
                    {
                        postData.IsCurrunLoginUser = true;
                    }
                }
                else
                {
                    // return RedirectToAction("Index", "User");
                    // postData.IsCurrunLoginUser = true; // TODO 토큰이 만료된 경우, 이 처리를 어떻게 해주지?
                }

                // 댓글 데이터 조회
                CommentsViewModel commentListData = _service.GetCommentByPostId(new CommentsViewParams { PostId = _params.PostId, Page = _params.Page, CommentPage = _params.CommentPage, Category = _params.Category});
                
                // 게시물 하단 게시판 데이터 조회
                BoardViewModel boardViewModel = _service.GetBoardViewData(_params); // TODO 업캐스팅 되나?
                PostViewModel viewModel = new PostViewModel(postData, boardViewModel.PageCount, boardViewModel.Page, boardViewModel.Category, boardViewModel.PageSize, boardViewModel.PostListData);

                viewModel.CommentListModel = commentListData; // TODO 위에 (....) 생성자 방식 수정할 것
                return View(viewModel);
            }
        }

        // 게시물 삭제
        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            string cookie = Request.Cookies["jwtToken"] ?? "";
            ClaimsPrincipal Principal = _jwtManager.ValidateJwtToken(cookie);
            if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
            {
                _service.DeletePost(postId);
                return RedirectToAction("Index");
            }
            else
            {
                // TODO 삭제 실패 처리
                return RedirectToAction("Index");
            }
        }

        // TODO   댓글 수정, 삭제 개발 필요

        // 댓글 작성
        [HttpPost]
        public IActionResult WriteComment(WriteCommentParams commentParams) 
        {
            Console.WriteLine($"## CommunityController >> WriteComment() PostId: {commentParams.PostId}, UserId: {commentParams.UserId}, ParentId: {commentParams.ParentId}, Contents: {commentParams.Contents}, IsAnonymous: {commentParams.IsAnonymous}");

            // TODO 유효성 체크
            if (ModelState.IsValid)
            {
                // 인증
                (bool IsAuthenticated, ClaimsPrincipal? Principal) = Authentication();
                if (IsAuthenticated && Principal != null)
                {
                    int userNumber = GetUserNumber(Principal);

                    // 댓글 작성자 매핑
                    commentParams.UserId = userNumber;

                    _service.CreateComment(commentParams); // TODO 업캐스팅
                    return PartialView("_Comments", _service.GetCommentByPostId(commentParams.ViewParams));
                }
                else
                {
                    // TODO 인증 실패 시, 로그인 페이지로 보내야 함
                    return PartialView("_Comments", _service.GetCommentByPostId(commentParams.ViewParams));

                    // return RedirectToAction("Index", "User"); 
                    // 위와 같이 RedirectToAction 를 반환하면 UpdateTarget 영역이 User/Index 로 리다이렉트됨
                    // . _WriteComment.cshtml 에서 Html.AjaxBeginForm 대신 Javascript 로 Ajax 호출하여 return 결과에 따라 window.location.href = '/User/Index'; 해야 할까

                    // return View(commentParams);
                }
            }
            else
            {
                // TODO PartialView 라서 이렇게 처리하면 안되는데
                // return View(commentParams);
                return PartialView("_Comments", _service.GetCommentByPostId(commentParams.ViewParams));
            }
        }

        /// <summary>
        /// 쿠키에서 JWT 토큰을 읽어 인증
        /// </summary>
        /// <returns>
        /// bool: 인증 성공 여부
        /// ClaimsPrincipal?: Principal 객체
        /// </returns>
        public (bool, ClaimsPrincipal?) Authentication()
        {
            bool IsAuthenticated = false;

            string cookie = Request.Cookies["jwtToken"] ?? "";
            ClaimsPrincipal Principal = _jwtManager.ValidateJwtToken(cookie);

            if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
            {
                IsAuthenticated = true;
                return (IsAuthenticated, Principal);
            }
            else
            {
                return (IsAuthenticated, null);
            }
        }

        /// <summary>
        /// 현재 인증된 토큰의 Principal 객체에서 유저 고유 번호 클레임을 읽어 반환
        /// </summary>
        /// <param name="Principal"></param>
        /// <returns>int: 유저 고유 번호 (UserId -> UserNumber 바꿀 것)</returns>
        public int GetUserNumber(ClaimsPrincipal Principal)
        {
            int userNumber = 0;
            try
            {
                userNumber = Convert.ToInt32(Principal.FindFirst(MvcBoardClaimTypes.UserNumber)?.Value);
            }
            catch (Exception ex)
            {
            }

            return userNumber;
        }

    }
}
