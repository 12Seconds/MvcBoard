using MvcBoardAdmin.Models.AdminMember;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadAdminMembersResponse : CommonResponse
    {
        public AdminMemberListViewModel ViewModel { get; set; }
    }
}
