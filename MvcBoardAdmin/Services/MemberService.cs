using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;

namespace MvcBoardAdmin.Services
{
    public class MemberService
    {
        private readonly MemberDataManager _memberDataManager;
        public MemberService(MemberDataManager memberDataManager)
        {
            _memberDataManager = memberDataManager;
        }

        public ReadMembersResponse ReadMembers(ReadMembersServiceParams _params)
        {
            // TODO 인증
            // _params.HttpContext.Request

            ReadMembersResponse response = new ReadMembersResponse();

            response.ViewModel = _memberDataManager.ReadMembers(_params.ReadMembersParams);

            response.Message = "검색 결과 조회 성공";

            return response;
        }


    }
}
