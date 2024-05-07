using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models.Comment;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{
    public class CommentService
    {
        private readonly CommentDataManager _commentDataManager;
        private readonly BoardService _boardService; // TODO 한 서비스에서 다른 서비스 이용하는 구조..
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

            // 입력값 유효성 검증
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

        /// <summary>
        /// 댓글 상세 조회하여 CommentEditorPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommentEditorViewModel GetCommentEditorViewModel(GetCommentEditorServiceParams _params)
        {
            CommentEditorViewModel Model = new CommentEditorViewModel();

            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                Model.Response = Response;
                return Model;
            }

            // 검증 통과시 DB 요청
            ReadCommentDetailResult Result = _commentDataManager.ReadCommentDetail(_params.CommentId);

            Model.Response = Result.Response;
            Model.CommentDetail = Result.CommentDetail;

            return Model;
        }
        

        /// <summary>
        /// 댓글 상세 조회하여 CommentDetailPartial 의 ViewModel 반환
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommentDetailViewModel GetCommentDetailViewModel(GetCommentDetailServiceParams _params)
        {
            CommentDetailViewModel Model = new CommentDetailViewModel();

            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                Model.Response = Response;
                return Model;
            }

            // 검증 통과시 DB 요청
            ReadCommentDetailResult Result = _commentDataManager.ReadCommentDetail(_params.CommentId);

            Model.Response = Result.Response;
            Model.CommentDetail = Result.CommentDetail;

            return Model;
        }

        /// <summary>
        /// 댓글 정보 수정 요청 - 숨김(블라인드), 삭제, 영구삭제 포함
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse UpdateComment(UpdateCommentServiceParams _params)
        {
            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _commentDataManager.UpdateComment(_params.UpdateParams);

            return Response;
        }

        /// <summary>
        /// 게시물 삭제, 숨김(블라인드) 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse DeleteComment(DeleteCommentServiceParams _params)
        {
            CommonResponse Response = new CommonResponse();

            if (_params.DeleteParams.CommentId < 1)
            {
                Response.ResultCode = 201;
                Response.Message = "입력값 오류 (유효하지 않은 CommentId)";
                return Response;
            }
            
            Response = _commentDataManager.DeleteComment(_params.DeleteParams);

            return Response;
        }

    }

}
