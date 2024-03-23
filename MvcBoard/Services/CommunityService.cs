using Azure.Identity;
using MvcBoard.Controllers.Models;
using MvcBoard.Managers;
using MvcBoard.Managers.Models;
using MvcBoard.Models.Community;

namespace MvcBoard.Services
{
    /// <summary>
    /// 커뮤니티 (게시판) 기능과 관련된 비즈니스 로직 처리 
    ///  - 유효성 검증 (Validation check)
    ///  - 캐시 관리 (Caching)
    /// </summary>
    public class CommunityService
    {
        private readonly CommunityDataManagers _dataManagers;

        private DateTime _cachedTime;

        private TimeSpan cacheDuration = TimeSpan.FromMinutes(1); // 캐시 유효 시간: 1분 (개발용으로 1분 -> 비즈니스 로직에 따라 적절하게 설정할 것)

        private List<BoardType>? _cachedBoardTypeData = null;

        public CommunityService(CommunityDataManagers dataManager)
        {
            _dataManagers = dataManager;
        }
        // 게시판 카테고리(메뉴) 조회
        public List<BoardType> GetBoardTypeData()
        {
            // 최초 조회거나, 캐시가 만료된 경우 재조회
            if (_cachedBoardTypeData == null || DateTime.Now - _cachedTime > cacheDuration )
            {
                Console.WriteLine("### CommunityService >> GetBoardTypeData() --- Get New BoardTypeData (cache expired!)");
                _cachedBoardTypeData = _dataManagers.GetBoardTypeData();
                _cachedTime = DateTime.Now;
            }

            return _cachedBoardTypeData;
        }

        // 게시판 조회
        public BoardViewModel GetBoardViewData(BoardViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetBoardViewData() _params.Category: {_params.Category}, _params.Page: {_params.Page}");

            GetBoardParams @params = new GetBoardParams();

            @params.Category = _params.Category;
            @params.Page = _params.Page < 0 ? 1 : _params.Page;

            return _dataManagers.GetBoardViewData(@params);
        }

        // 인기 게시판 조회 (현재는 게시판 조회와 SP 이름만 다르고 모든게 같지만, 실시간/주간/월간 필터링 기능 추가되면 달라질 것이므로 따로 작성)
        public BoardViewModel GetHotBoardViewData(BoardViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetBoardViewData() _params.Category: {_params.Category}, _params.Page: {_params.Page}");

            GetBoardParams @params = new GetBoardParams();

            @params.Category = _params.Category;
            @params.Page = _params.Page < 0 ? 1 : _params.Page;

            return _dataManagers.GetHotBoardViewData(@params);
        }

        // 게시물 작성
        public void CreatePost(Post post)
        {
            // TODO 유효성 검증
            _dataManagers.CreatePost(post);
        }

        // 게시물 수정
        public void UpdatePost(Post post)
        {
            // TODO 유효성 검증
            _dataManagers.UpdatePost(post);
        }

        // 게시물 조회
        public PostWithUser? GetPostWithUserById(int? postId = null/* int postId, int? page, int category = 0, int commentPage = 1 */)
        {
            PostWithUser postData = _dataManagers.GetPostWithUserById(postId);

          
            return postData;

            /*
            
            Q.
            Controller -> Service -> DataManager -> DBManager 구조로 변경, Controller 의 아래 비즈니스 로직(?) 을 Service 로 옮기면.. return 을 어떻게 처리해야줘야 하지?

            // TODO 게시물 데이터 (by postId), 댓글 데이터 조인 필요? -> 조인이 아니고 저장프로시저에서 SELECT 쿼리 결과 2개 반환하고 reader.NextResult()
            PostWithUser? postData = _dataManagers.GetPostDataById(postId);

            if (postData == null)
            {
                // TODO IExceptionFilter 를 구현한 별도 예외 처리 로직 및 뷰 만들어서 넘기기
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // 게시물 하단 게시판 데이터 조회
                BoardViewModel boardViewModel = _dataManagers.GetBoardViewData(category, page ?? 1);
                PostViewModel viewModel = new PostViewModel(postData, boardViewModel.PageCount, boardViewModel.Page, boardViewModel.Category, boardViewModel.PageSize, boardViewModel.PostListData);
                return View(viewModel);
            }
            */
        }

        // 게시물 삭제
        public bool DeletePost(int postId)
        {
            // 유효성 검증
            // if (postId)
            _dataManagers.DeletePost(postId);

            // todo 처리 필요
            return true;
        }

        // TODO CommunityDataManager의 다른 SP 호출 함수들 구현, 매칭 필요

        // 댓글 작성 (TODO 리턴 타입으로 Result 객체 만들어서 처리하거나..)
        public void CreateComment(Comment comment)
        {
            //TODO 유효성 검증
            if (comment.PostId < 0 || comment.UserId < 0 || comment.Contents.Length == 0)
            {
                return;
            }
            _dataManagers.CreateComment(comment);

            // TODO return
        }

        // 댓글 조회
        public CommentsViewModel GetCommentByPostId(CommentsViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetCommentByPostId() PostId: {_params.PostId}, Page: {_params.Page}, CommentPage: {_params.CommentPage}, Category: {_params.Category}");

            if (_params.PostId > 0)
            {
                GetCommentsByPostIdParams @params = new();
                
                @params.PostId = _params.PostId;
                @params.Page = _params.Page < 0 ? 1 : _params.Page;
                // TODO 이게 맞나
                @params.Category = _params.Category;
                @params.CommentPage = _params.CommentPage < 0 ? 1 : _params.CommentPage;
                @params.PostUserId = _params.PostUserId;

                CommentsViewModel ViewModel = _dataManagers.ReadCommentByPostId(@params);

                // 댓글 데이터 가공
                foreach (var comment in ViewModel.CommentListData)
                {
                    // 로그인 사용자의 댓글 여부 설정
                    if (comment.UserId > 0 && comment.UserId == _params.CurrentLoginUserNumber)
                    {
                        comment.IsCurrunLoginUser = true;
                    }

                    // 글 작성자의 댓글 여부 설정
                    if (comment.UserId == _params.PostUserId)
                    {
                        comment.IsPostWriter = true;
                    }
                }

                return ViewModel;
            }
            else
            {
                return new CommentsViewModel();
            }
        }

        // 댓글 삭제
        public bool DeleteComment(int commentId)
        {
            // 유효성 검증
            // if (commentId)
            _dataManagers.DeleteComment(commentId);

            // todo 처리 필요
            return true;
        }

    }
}
