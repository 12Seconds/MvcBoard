using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Models.Member;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{
    public class AdminMemberService
    {
        private readonly AdminMemberDataManager _adminMemberDataManager;
        public AdminMemberService(AdminMemberDataManager adminMemberDataManager)
        {
            _adminMemberDataManager = adminMemberDataManager;
        }

        /// <summary>
        /// 관리자 계정 관리페이지 진입
        /// </summary>
        /// <returns></returns>
        public MemberManageViewModel GetAdmMemberManageViewModel(/* _params */)
        {
            MemberManageViewModel Model = new MemberManageViewModel();

            // 그 외 필요한 작업들..

            return Model;
        }

        /// <summary>
        /// 관리자 계정 리스트 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadAdminMembersResponse ReadAdmMembers(ReadMembersServiceParams _params)
        {
            // _params.HttpContext.Request

            ReadAdminMembersResponse Response = new ReadAdminMembersResponse();

            Response.ViewModel = _adminMemberDataManager.ReadAdmMembers(_params.ReadMembersParams);

            Response.Message = "검색 결과 조회 성공";

            return Response;
        }
        
        /// <summary>
        /// 관리자 계정 정보 상세 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadAdminMemberDetailResponse ReadAdmMemberDetail(ReadMemberDetailServiceParams _params)
        {
            ReadAdminMemberDetailResponse Response = _adminMemberDataManager.ReadAdmMemberDetail(_params.UserId);

            // 뷰모델에 필드 추가 alert 같은거 ?

            return Response;
        }

        /// <summary>
        /// 관리자 계정 정보 수정 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse UpdateAdmMember(UpdateMemberServiceParams _params)
        {
            // 입력값 유효성 검증 
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _adminMemberDataManager.UpdateAdmMember(_params.UpdateParams);

            return Response;
        }

        /// <summary>
        /// 관리자 계정 정보 삭제 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse DeleteAdmMember(DeleteMemberServiceParams _params)
        {
            CommonResponse Response = new CommonResponse();

            if (_params.UserId < 1)
            {
                Response.ResultCode = 201;
                Response.Message = "입력값 오류 (유효하지 않은 UserNo)";
                return Response;
            }
            
            Response = _adminMemberDataManager.DeleteAdmMember(_params.UserId);

            return Response;
        }

    }
}
