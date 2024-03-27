using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Models.Home;
using System.Diagnostics;

namespace MvcBoardAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // TODO 알아보고 사용할 것

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // TODO 인증 해서 modle 가공하여 넘겨줄 것 (IsLogined true/false)

            HomeViewModel model = new HomeViewModel();
            model.IsLogined = true;

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginParams _params)
        {
            // TODO _logger 사용
            Console.WriteLine($"## HomeController >> Login() _params.Id: {_params.Id}, _params.Password: {_params.Password}");

            // LoginResult result = _service.Login(_params)
            
            // result 에 따른 처리로 response 생성 

            // LoginResponse response = new LoginResponse {};

            var responseData = new
            {
                status = "success 1 ",
                message = "Data successfully processed 1 ",
                data = "Your data here  1 "
            };

            return Ok(responseData);

            // return new JsonResult(responseData);
            // return Json(new { resultCode = _params });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
