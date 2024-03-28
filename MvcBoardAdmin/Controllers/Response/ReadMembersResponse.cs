using MvcBoardAdmin.Models.Member;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadMembersResponse : CommonResponse
    {
        public MemberListViewModel ViewModel { get; set; }
    }
}
