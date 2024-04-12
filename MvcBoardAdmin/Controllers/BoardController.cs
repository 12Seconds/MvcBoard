using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Filters;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    /* 게시판 관리 컨트롤러 */
    [AuthenticationFilter]
    public class BoardController : Controller
    {
        private readonly BoardService _boardService;
        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        /* 게시판 관리 페이지 진입 */
        public IActionResult Index()
        {
            return View();
        }

        /* 게시판 네이게이션 메뉴 Partial View */
        [HttpGet]
        public IActionResult BoardNavigationMenu()
        {
            Console.WriteLine($"## BoardController >> BoardNavigationMenu()");

            ReadBoardTypeResponse  Response = _boardService.GetBoardTypeData();

            return PartialView("_BoardNavigation", Response);
        }

        /* 게시판 리스트 PartialView */
        [HttpGet]
        public IActionResult BoardListPartial()
        {
            Console.WriteLine($"## BoardController >> BoardListPartial()");

            ReadBoardTypeResponse Response = _boardService.GetBoardTypeData();

            return PartialView("_BoardList", Response);
        }

        /* 게시판 에디터 PartailView */
        [HttpGet]
        public IActionResult BoardEditorPartial(int BoardId = 0) // BoardId 가 0으로 전달되면 생성하는 로직
        {
            ReadBoardDetailServiceParams ServiceParams = new ReadBoardDetailServiceParams
            {
                BoardId = BoardId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            BoardDetailResponse Response = _boardService.ReadBoardDetail(ServiceParams);

            return PartialView("_BoardEditor", Response);
        }

        /* 게시판 생성 */
        [HttpPost]
        public IActionResult Create(CreateBoardParams _params)
        {

            CreateBoardServiceParams ServiceParams = new CreateBoardServiceParams
            {
                CreateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _boardService.CreateBoard(ServiceParams);

            return Ok(Response);
        }

        /* 게시판 정보 수정 */
        [HttpPost]
        public IActionResult Update(UpdateBoardParams _params)
        {
            UpdateBoardServiceParams ServiceParams = new UpdateBoardServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _boardService.UpdateBoard(ServiceParams);

            return Ok(Response);
        }

        /* 게시판 삭제 */
        [HttpPost]
        public IActionResult Delete(int BoardId)
        {
            DeleteBoardServiceParams ServiceParams = new DeleteBoardServiceParams
            {
                BoardId  = BoardId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _boardService.DeleteBoard(ServiceParams);

            return Ok(Response);
        }

    }
}