using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models.Comment;
using MvcBoardAdmin.Models.Post;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{

    // TODO 각 액션 메소드에 인증 로직 추가 필요

    public class CommentService
    {
        private readonly CommentDataManager _commentDataManager;
        private readonly BoardService _boardService; // TODO Question 한 서비스에서 다른 서비스 이용
        public CommentService(CommentDataManager commentDataManager, BoardService boardService)
        {
            _commentDataManager = commentDataManager;
            _boardService = boardService;
        }

        /// <summary>
        /// 댓글 관리페이지 진입
        /// </summary>
        /// <returns></returns>
        public CommentManageViewModel GetCommentManageViewModel(/* _params */)
        {
            CommentManageViewModel Model = new CommentManageViewModel();

            Model.WritableBoards = _boardService.GetWritableBoards();

            // 그 외 필요한 작업들..

            return Model;
        }

        /// <summary>
        /// 댓글 리스트 조회하여 CommentListPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommentListViewModel GetCommentListViewModel(ReadCommentsServiceParams _params)
        {
            CommentListViewModel Model = new CommentListViewModel();

            // 입력값 유효성 검증 (TODO)
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                Model.Response = Response;
                return Model;
            }

            // 검증 통과시 DB 요청
            ReadCommentsResult Result = _commentDataManager.ReadComments(_params.ReadCommentsParams);

            Model.Response = Result.Response;
            Model.CommentList = Result.CommentList;
            Model.IndicatorRange = Result.IndicatorRange;
            Model.TotalRowCount = Result.TotalRowCount;
            Model.TotalPageCount = Result.TotalPageCount;

            Model.PageIndex = _params.ReadCommentsParams.Page;
            Model.ExBoardFilter = _params.ReadCommentsParams.BoardFilter;
            Model.ExSearchFilter = _params.ReadCommentsParams.SearchFilter;
            Model.ExSearchWord = _params.ReadCommentsParams.SearchWord != null ? _params.ReadCommentsParams.SearchWord : "";
            // Model.ExSearchWord = _params.ReadCommentsParams.SearchWord ?? ""; // C# 8 이상 문법

            return Model;
        }

        /* 
        /// <summary>
        /// 게시물 상세 조회하여 PostEditorPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public PostEditorViewModel GetPostEditorViewModel(GetPostEditorServiceParams _params)
        {
            PostEditorViewModel Model = new PostEditorViewModel();

            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                Model.Response = Response;
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
        /// 게시물 상세 조회하여 PostDetailPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public PostDetailViewModel GetPostDetailViewModel(GetPostDetailServiceParams _params)
        {
            PostDetailViewModel Model = new PostDetailViewModel();

            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                Model.Response = Response;
                return Model;
            }

            // 검증 통과시 DB 요청
            ReadPostDetailResult Result = _postDataManager.ReadPostDetail(_params.PostId);

            Model.Response = Result.Response;
            Model.Post = Result.Post;

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
        */

    }

}
