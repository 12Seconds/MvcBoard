using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{

    // TODO 각 액션 메소드에 인증 로직 추가 필요

    public class PostService
    {
        private readonly PostDataManager _postDataManager;
        public PostService(PostDataManager postDataManager)
        {
            _postDataManager = postDataManager;
        }

        /// <summary>
        /// 게시물 리스트 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadPostsResponse ReadPosts(ReadPostsServiceParams _params)
        {
             ReadPostsResponse Response = new ReadPostsResponse();

            // 입력값 유효성 검증 (TODO)
            // ReadPostsResponse Response = Utility.ModelStateValidation(_params.ModelState) as ReadPostsResponse;
            CommonResponse _response = Utility.ModelStateValidation(_params.ModelState);

            // TODO ReadPostsResponse 반환 -> PostListViewModel 반환하는 구조로 바꿀 것 (멤버로 CommonResponse 포함)

            // TODO Question CommonResponse 를 ReadPostsResponse 로 다운 캐스팅 하면 Null 값 리턴되어 Null 참조 오류 발생
            if (_response.ResultCode != 200)
            {
                Response.ResultCode = _response.ResultCode;
                Response.Message = _response.Message;
                Response.ErrorMessages = _response.ErrorMessages;
                Response.ErrorSummary = _response.ErrorSummary;
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _postDataManager.ReadPosts(_params.ReadPostsParams);

            return Response;
        }
        
        /*
        /// <summary>
        /// 유저(멤버) 정보 상세 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public ReadMemberDetailResponse ReadMemberDetail(ReadMemberDetailServiceParams _params)
        {
            ReadMemberDetailResponse Response = _postDataManager.ReadMemberDetail(_params.UserId);

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
            // TODO Question 직접 각 필드들을 조사해서 response 를 만들어서 넘겨주거나 (1)
            // ModelState 객체를 통채로 넘겨주어서 클라이언트 측에서 Javascript 로 추출하여 가공 및 Validataion Message 처리 (2) ?

            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _postDataManager.UpdateMember(_params.UpdateParams);

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
            
            Response = _postDataManager.DeleteMember(_params.UserId);

            return Response;
        }
        */

    }
    
}
