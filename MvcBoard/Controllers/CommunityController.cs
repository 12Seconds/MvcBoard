﻿using Microsoft.AspNetCore.Mvc;
using MvcBoard.Managers.Services;
using MvcBoard.Models.Community;

namespace MvcBoard.Controllers
{
    public class CommunityController : Controller
    {
        private readonly CommunityService _service;

        public CommunityController(IWebHostEnvironment env)
        {
            _service = new CommunityService(env);
        }

        // 전체 게시판
        public IActionResult Index(int? category = null, int? page = 1) // TODO Params 객체로 만들어 넘기기
        {
            BoardViewModel viewModel = _service.GetBoardViewData(category ?? 0, page ?? 1);

            return View(viewModel);
        }

        // 인기 게시판
        public IActionResult Hot(int? category = null, int? page = 1)
        {
            // TODO 별도 함수 만들어서 호출 필요
            BoardViewModel viewModel = _service.GetBoardViewData(2, page ?? 1); // 2는 질문답변 게시판 카테고리 번호임

            return View(viewModel);
        }

        // 공지 게시판
        public IActionResult Notice(int? category = null, int? page = 1)
        {
            // TODO 별도 함수 만들어서 호출 필요 (isNotice)
            BoardViewModel viewModel = _service.GetBoardViewData(99, page ?? 1); // TODO 99 는 임시, 수정 필요

            return View(viewModel);
        }

        // 게시물 작성
        public IActionResult Write(int category = 0)
        {
            // WriteViewModel viewModel = new WriteViewModel(category); // TODO WriteViewModel class 삭제 고려
            Post viewModel = new Post();
            viewModel.Category = category;

            return View(viewModel);
        }

        // 게시물 작성
        [HttpPost]
        public IActionResult Write(Post postData)
        {
            Console.WriteLine($"[Controller] Community >> Write() postData.Category: {postData.Category},  IsValid: {ModelState.IsValid}"); // TODO stringify

            if (ModelState.IsValid)
            {
                if (postData.Category == 0) postData.Category = 1; // 1: 자유게시판 (default), todo 드롭다운 or 모달 구현하면 필요 없음

                // TODO 작성자 UserId 매핑 필요 (로그인 구현 후)
                postData.UserId = 1;

                _service.CreatePost(postData);

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
        public IActionResult View(int postId, int? page, int category = 0, int commentPage = 1)
        {
            // TODO 게시물 데이터 (by postId)
            PostWithUser? postData = _service.GetPostDataById(postId);

            if (postData == null)
            {
                // TODO IExceptionFilter 를 구현한 별도 예외 처리 로직 및 뷰 만들어서 넘기기
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // TODO 댓글 데이터

                // 게시물 하단 게시판 데이터 조회
                BoardViewModel boardViewModel = _service.GetBoardViewData(category, page ?? 1);
                PostViewModel viewModel = new PostViewModel(postData, boardViewModel.PageCount, boardViewModel.Page, boardViewModel.Category, boardViewModel.PageSize, boardViewModel.PostListData);
                return View(viewModel);
            }
        }

        // TODO 게시물 수정, 삭제 개발 필요 


        [HttpPost]
        public IActionResult WriteComment(Comment comment) // TODO 새 모델 객체?
        {
            return RedirectToAction("Index"); // 임시
            // return View(comment);
        }
    }
}
