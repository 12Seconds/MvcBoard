using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Models.Home;
using MvcBoardAdmin.Services;
using System.Diagnostics;

namespace MvcBoardAdmin.Controllers
{
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
            // TODO 인증 해서 model 가공하여 넘겨줄 것 (IsLogined true/false)

            HomeViewModel model = new HomeViewModel();
            model.IsLogined = true;

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginParams _params)
        {
            // TODO _logger
            // _logger.LogDebug($"## HomeController >> Login() _params.Id: {_params.Id}, _params.Password: {_params.Password}");
            Console.WriteLine($"## HomeController >> Login() _params.Id: {_params.Id}, _params.Password: {_params.Password}");

            LoginServiceParams serviceParams = new LoginServiceParams
            {
                LoginParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            LoginResponse response = _homeService.Login(serviceParams);

            return Ok(response); // return new JsonResult(response); 동일함
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
