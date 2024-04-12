using MvcBoardAdmin.Models.AdminMember;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadAdminMemberDetailResponse : CommonResponse
    {
        public AdminMemberEditorViewModel ViewModel { get; set; } = new AdminMemberEditorViewModel();
    }
}
