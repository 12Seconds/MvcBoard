using Microsoft.AspNetCore.Mvc;
using MvcBoard.Controllers.Models;
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
            if (ModelState.IsValid)
            {
                // TODO
                _service.Login();

                return RedirectToAction("Index", "Community");
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
