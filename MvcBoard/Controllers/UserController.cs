﻿using Microsoft.AspNetCore.Mvc;
using MvcBoard.Controllers.Models;
using MvcBoard.Managers.JWT;
using MvcBoard.Managers.Models;
using MvcBoard.Services;

namespace MvcBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _service;
        private readonly JWTManager _jwtManager;
        public UserController(UserService service, JWTManager jwtManager)
        {
            _service = service;
            _jwtManager = jwtManager;
        }
        
        // 로그인 화면
        public IActionResult Index()
        {
            return View();
        }
        
        /*
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login(LoginParams _params)
        {
            return View();
        }
        */

        // 로그인
        [HttpPost]
        public IActionResult Login(LoginParams _params)
        {
            LogInResultParams Result = new();

            if (ModelState.IsValid)
            {
                // TODO
                Result = _service.Login(_params);

                if (Result.ResultCode == 1)
                {
                    // TODO _params_.Id 를 UserId 로 바꿀 것 -> Login 에서 Id 를 이용해서 해당 유저의 정보를 읽어서 반환해주어야 함

                    // TODO 토큰에 Id 외 다른 정보(클레임)도 저장하려면 _service.Login 반환 타입이 수정되어야 함 (유저의 정보를 포함해야 함)

                    // 성공 시 토큰 발급
                    string token = _jwtManager.GenerateToken(_params.Id);

                    // TODO 로그인에 성공하여 토큰이 발급된 유저의 정보를 배열로 저장하고 관리를 하면 서버 메모리 부하가 심할까?

                    return Ok(new { token });
                }
                else
                {
                    // TODO 나머지 경우..
                    // ModelState.AddModelError  Result.Msg 같은거 처리?

                    return Unauthorized(new { message = "Invalid username or password" });
                }
            }
           
            return View("Index", _params);
        }


        // 회원가입 화면
        public IActionResult SignUp()
        {
            return View();
        }

        // 회원가입
        [HttpPost]
        public IActionResult SignUp(SignUpParams _params)
        {
            if (ModelState.IsValid)
            {
                // TODO
                _service.SignUp(_params);

                return RedirectToAction("Index");
            }
            return View(_params);
        }
    }
}
