using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Filters;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Models.Home;
using MvcBoardAdmin.Services;
using System.Diagnostics;

namespace MvcBoardAdmin.Controllers
{
    /* 메인 홈 페이지 컨트롤러 (로그인/로그아웃) */
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // TODO 알아보고 사용할 것
        private readonly HomeService _homeService;

        public HomeController(ILogger<HomeController> logger, HomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            HomeViewModel Model = _homeService.GetHomeViewModel(HttpContext);

            return View(Model);
        }

        /* 로그인 요청 */
        [HttpPost]
        public IActionResult Login(LoginParams _params)
        {
            // TODO _logger
            // _logger.LogDebug($"## HomeController >> Login() Id: {_params.Id}, Password: {_params.Password}");
            Console.WriteLine($"## HomeController >> Login() Id: {_params.Id}, Password: {_params.Password}");

            LoginServiceParams ServiceParams = new LoginServiceParams
            {
                LoginParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _homeService.Login(ServiceParams);

            return Ok(Response); // return new JsonResult(Response); 동일함
        }

        /* 로그아웃 요청 */
        [HttpGet]
        [AuthenticationFilter]
        public IActionResult Logout()
        {
            Console.WriteLine($"## HomeController >> Logout()");

            _homeService.Logout(HttpContext);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }          
    }
}
