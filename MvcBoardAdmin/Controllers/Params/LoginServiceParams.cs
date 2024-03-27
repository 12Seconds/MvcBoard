using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcBoardAdmin.Controllers.Params
{
    public class LoginServiceParams
    {
        public LoginParams LoginParams { get; set; }

        // Controller 에서 Service 에 전달하여 사용할 ControllerBase 객체들로, Service 에 종속성 주입하여 사용하려고도 생각했는데, Controller 에서만 사용하는 것이 권장된다고 함
        public ModelStateDictionary ModelState { get; set; }
        public HttpContext HttpContext { get; set; }
    }
}
