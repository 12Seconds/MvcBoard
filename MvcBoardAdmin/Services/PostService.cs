using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models.Post;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{

    // TODO 각 액션 메소드에 인증 로직 추가 필요

    public class PostService
    {
        private readonly PostDataManager _postDataManager;
        private readonly BoardService _boardService; // TODO Question 한 서비스에서 다른 서비스 이용
        public PostService(PostDataManager postDataManager, BoardService boardService)
        {
            _postDataManager = postDataManager;
            _boardService = boardService;
        }

        public PostManageViewModel GetPostManageViewModel(/* _params */)
        {
            PostManageViewModel Model = new PostManageViewModel();

            Model.WritableBoards = _boardService.GetWritableBoards();

            // 그 외 필요한 작업들..

            return Model;
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

        /// <summary>
        /// 게시물 상세 조회하여 PostEditorPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public PostEditorViewModel GetPostEditorViewModel(GetPostEditorServiceParams _params)
        {
            PostEditorViewModel Model = new PostEditorViewModel();

            // 입력값 유효성 검증
            CommonResponse _response = Utility.ModelStateValidation(_params.ModelState);

            if (_response.ResultCode != 200)
            {
                Model.Response = _response;
                return Model;
            }

            // 검증 통과시 DB 요청
            ReadPostDetailResult Result = _postDataManager.ReadPostDetail(_params.PostId);

            Model.Response = Result.Response;
            Model.Post = Result.Post;
            Model.WritableBoards = _boardService.GetWritableBoards();

            return Model;
        }

        
        /// <summary>
        /// 게시물 정보 수정 요청 - 게시판 이동, 숨김(블라인드), 삭제, 영구삭제 포함
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse UpdatePost(UpdatePostServiceParams _params)
        {
            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _postDataManager.UpdatePost(_params.UpdateParams);

            return Response;
        }

        /// <summary>
        /// 게시물 삭제, 숨김(블라인드) 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse DeletePost(DeletePostServiceParams _params)
        {
            CommonResponse Response = new CommonResponse();

            if (_params.DeleteParams.PostId < 1)
            {
                Response.ResultCode = 201;
                Response.Message = "입력값 오류 (유효하지 않은 PostId)";
                return Response;
            }
            
            Response = _postDataManager.DeletePost(_params.DeleteParams);

            return Response;
        }

    }

}
