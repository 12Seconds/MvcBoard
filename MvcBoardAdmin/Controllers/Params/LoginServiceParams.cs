using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcBoardAdmin.Controllers.Params
{
    public class LoginServiceParams : CommonServiceParams
    {
        public LoginParams LoginParams { get; set; }
    }
}
