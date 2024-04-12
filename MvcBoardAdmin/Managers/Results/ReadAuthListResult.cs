using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;

namespace MvcBoardAdmin.Managers.Results
{
    public class ReadAuthListResult
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public List<AuthDetail> AuthList { get; set; } = new List<AuthDetail>();
    }
}
