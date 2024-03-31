﻿using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    public class BoardController : Controller
    {
        private readonly BoardService _boardService;
        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /* 게시판 네이게이션 메뉴 Partial View */
        [HttpGet]
        public IActionResult BoardNavigationMenu()
        {
            Console.WriteLine($"## BoardController >> BoardNavigationMenu()");

            // TODO 인증 때문에 Response 로 받아야 될 듯
            List<BoardType> Model = _boardService.GetBoardTypeData();

            return PartialView("_BoardNavigation", Model);
        }

        /* 게시판 리스트 PartialView */
        [HttpGet]
        public IActionResult BoardListPartial()
        {
            Console.WriteLine($"## BoardController >> BoardListPartial()");

            /*
            ReadMembersServiceParams ServiceParams = new ReadMembersServiceParams
            {
                ReadMembersParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };
            */

            List<BoardType> Model = _boardService.GetBoardTypeData();

            return PartialView("_BoardList", Model);
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
        public IActionResult Update(UpdateMemberParams _params)
        {
            /*
            UpdateMemberServiceParams ServiceParams = new UpdateMemberServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _boardService.UpdateMember(ServiceParams);
            */

            //return Ok(Response);

            Console.Write("테스트 업데이트 ");
            return Ok();
        }


    }
}