using MvcBoardAdmin.Controllers.Response;

namespace MvcBoardAdmin.Managers.Results
{
    public class LoginResult
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public int ResultCode { get; set; }
        public int UserNo { get; set; }
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Image { get; set; }
        public string AuthorityGroup { get; set; } = "";
    }
}
