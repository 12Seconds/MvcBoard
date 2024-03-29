using MvcBoardAdmin.Models.Member;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadMemberDetailResponse : CommonResponse
    {
        public MemberEditorViewModel ViewModel { get; set; } = new MemberEditorViewModel();
    }
}
