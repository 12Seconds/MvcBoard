﻿using Microsoft.AspNetCore.Mvc;
using MvcBoard.Controllers.Models;
using MvcBoard.Managers.Models;
using MvcBoard.Services;

namespace MvcBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
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
                    return RedirectToAction("Index", "Community");
                }

                // TODO 나머지 경우..
                // ModelState.AddModelError  Result.Msg 같은거 처리?
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
