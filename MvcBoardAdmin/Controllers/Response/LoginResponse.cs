namespace MvcBoardAdmin.Controllers.Response
{
    public class LoginResponse : CommonResponse
    {
        public string JwtToken { get; set; } = ""; // TODO 불필요, 삭제 고려
    }
}
