using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Models.Member;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{
    public class MemberService
    {
        private readonly MemberDataManager _memberDataManager;
        public MemberService(MemberDataManager memberDataManager)
        {
            _memberDataManager = memberDataManager;
        }

        /// <summary>
        /// 유저 관리페이지 진입
        /// </summary>
        /// <returns></returns>
        public MemberManageViewModel GetMemberManageViewModel(/* _params */)
        {
            MemberManageViewModel Model = new MemberManageViewModel();

            // 그 외 필요한 작업들..

            return Model;
        }

        /// <summary>
        /// 유저(멤버) 리스트 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadMembersResponse ReadMembers(ReadMembersServiceParams _params)
        {
            // _params.HttpContext.Request

            ReadMembersResponse Response = new ReadMembersResponse();

            Response.ViewModel = _memberDataManager.ReadMembers(_params.ReadMembersParams);

            Response.Message = "검색 결과 조회 성공";

            return Response;
        }
        
        /// <summary>
        /// 유저(멤버) 정보 상세 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadMemberDetailResponse ReadMemberDetail(ReadMemberDetailServiceParams _params)
        {
            ReadMemberDetailResponse Response = _memberDataManager.ReadMemberDetail(_params.UserId);

            // 뷰모델에 필드 추가 alert 같은거 ?

            return Response;
        }

        /// <summary>
        /// 유저(멤버) 정보 수정 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse UpdateMember(UpdateMemberServiceParams _params)
        {
            // 입력값 유효성 검증 
            // TODO 직접 각 필드들을 조사해서 response 를 만들어서 넘겨주거나 (1)
            // ModelState 객체를 통채로 넘겨주어서 클라이언트 측에서 Javascript 로 추출하여 가공 및 Validataion Message 처리 (2) ?

            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _memberDataManager.UpdateMember(_params.UpdateParams);

            return Response;
        }

        /// <summary>
        /// 유저(멤버) 정보 삭제 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse DeleteMember(DeleteMemberServiceParams _params)
        {
            CommonResponse Response = new CommonResponse();

            if (_params.UserId < 1)
            {
                Response.ResultCode = 201;
                Response.Message = "입력값 오류 (유효하지 않은 Id)";
                return Response;
            }
            
            Response = _memberDataManager.DeleteMember(_params.UserId);

            return Response;
        }

    }
}
