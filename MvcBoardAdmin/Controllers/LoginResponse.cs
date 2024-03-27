namespace MvcBoardAdmin.Controllers
{
    public class LoginResponse : CommonResponse
    {
        public string JwtToken { get; set; } = "";
    }
}
