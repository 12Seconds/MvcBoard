using MvcBoard.Controllers.Models;
using MvcBoard.Controllers.Response;
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
        private List<BoardType> _cachedSortedBoardTypeData = new List<BoardType>();
        private List<BoardType> _cachedParentBoardTypeData = new List<BoardType>();

        // private bool NeedUpdate = false; // 게시판 정보가 수정되면 업데이트 필요함을 알리는 flag 세팅

        public CommunityService(CommunityDataManagers dataManager)
        {
            _dataManagers = dataManager;
        }
        // 게시판 카테고리(메뉴) 조회
        /// <summary>
        /// 게시판 카테고리(메뉴) 데이터 조회
        /// </summary>
        /// <param name="GetOrigin">true: 1-2 Detph 계층화된 데이터 / false: Select 원본 </param>
        /// <returns></returns>
        public ReadBoardTypeResponse GetBoardTypeData(bool GetOrigin = false)
        {
            ReadBoardTypeResponse Response = new ReadBoardTypeResponse();

            // 최초 조회거나, 캐시가 만료된 경우 재조회
            if (_cachedBoardTypeData == null || DateTime.Now - _cachedTime > cacheDuration/* || NeedUpdate*/)
            {
                Console.WriteLine("### CommunityService >> GetBoardTypeData() --- Get New BoardTypeData (cache expired!)");

                _cachedSortedBoardTypeData.RemoveRange(0, _cachedSortedBoardTypeData.Count);
                _cachedParentBoardTypeData.RemoveRange(0, _cachedParentBoardTypeData.Count);

                Response = _dataManagers.GetBoardTypeData();
                _cachedBoardTypeData = Response.BoardTypes;

                // 조회 실패한 경우
                if (Response.ResultCode != 200)
                {
                    // NeedUpdate = true;
                    Response.BoardTypes = new List<BoardType>();
                    return Response;
                }

                _cachedSortedBoardTypeData = SetHierarchy(Response.BoardTypes);
                _cachedTime = DateTime.Now;

                // NeedUpdate = false;
                Response.BoardTypes = GetOrigin ? _cachedBoardTypeData : _cachedSortedBoardTypeData;
                return Response;
            }

            Response.BoardTypes = GetOrigin ? _cachedBoardTypeData : _cachedSortedBoardTypeData;
            return Response;
        }

        /* 게시판 데이터를 부모-자식 계층으로 가공(실제 부모 Board 객체의 Childen 리스트에 자식 Board 넣는 작업, DB에서 순서는 정렬하여 보내줌) */
        /*
        Board 1    (부모)
        Board 1-1  (자식)
        Board 1-2  (자식)
        
        -> 
        
        Board 1        (부모)
            Board 1-1  (자식)
            Board 1-2  (자식)
         */
        public List<BoardType> SetHierarchy(List<BoardType> origin)
        {
            List<BoardType> result = new List<BoardType>();
            List<BoardType> parents = new List<BoardType>();

            bool[] isAdded = new bool[origin.Count];
            Array.Fill(isAdded, false);

            // 부모 게시판인 항목 찾아서 자식 추가 작업
            for (int i = 0; i < origin.Count; i++)
            {
                BoardType board = origin[i];

                // 0~2 고정 게시판 제외 (전체, 인기, 공지)
                if (i < 3)
                {
                    isAdded[i] = true;
                    result.Add(board);
                    continue;
                }

                // 부모 게시판인 경우
                if (board.ParentCategory == 0)
                {
                    for (int j = i + 1; j < origin.Count; j++)
                    {
                        if (board.Category == origin[j].ParentCategory)
                        {
                            board.Children.Add(origin[j]);
                            isAdded[j] = true;
                        }
                    }
                    isAdded[i] = true;
                    result.Add(board);
                    parents.Add(board);
                }
            }

            // 남은 게시판 추가 (추가되지 못한 자식 게시판)
            for (int i = 3; i < origin.Count; i++)
            {
                if (isAdded[i]) { continue; }
                result.Add(origin[i]);
            }

            _cachedParentBoardTypeData = parents;

            return result;
        }

        // 게시판 이름 조회
        public string GetBoardName(int category)
        {
            string boardName = "";

            if (_cachedBoardTypeData == null)
            {
                return boardName;
            }

            foreach(BoardType boardType in _cachedBoardTypeData)
            {
                if (boardType.Category == category)
                {
                    boardName = boardType.BoardName;
                }
            }

            // 공지 임시 처리
            if (category == 99)
            {
                boardName = "공지 게시판";
            }

            return boardName;
        }

        // 게시판 조회
        public BoardViewModel GetBoardViewData(BoardViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetBoardViewData() _params.Category: {_params.Category}, _params.Page: {_params.Page}");

            GetBoardParams @params = new GetBoardParams();

            @params.Category = _params.Category;
            @params.Page = _params.Page < 0 ? 1 : _params.Page;

            BoardViewModel viewModel = _dataManagers.GetBoardViewData(@params);
            viewModel.BoardName = GetBoardName(@params.Category);

            return viewModel;
        }

        // 인기 게시판 조회 (현재는 게시판 조회와 SP 이름만 다르고 모든게 같지만, 실시간/주간/월간 필터링 기능 추가되면 달라질 것이므로 따로 작성)
        public BoardViewModel GetHotBoardViewData(BoardViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetHotBoardViewData() _params.Category: {_params.Category}, _params.Page: {_params.Page}");

            GetBoardParams @params = new GetBoardParams();

            @params.Category = _params.Category;
            @params.Page = _params.Page < 0 ? 1 : _params.Page;

            BoardViewModel viewModel = _dataManagers.GetHotBoardViewData(@params);
            viewModel.BoardName = GetBoardName(@params.Category);

            return viewModel;
        }

        // 공지 게시판 조회
        public BoardViewModel GetNoticeBoardViewData(BoardViewParams _params)
        {
            Console.WriteLine($"### CommunityService >> GetNoticeBoardViewData() _params.Category: {_params.Category}, _params.Page: {_params.Page}");

            GetBoardParams @params = new GetBoardParams();

            @params.Category = _params.Category;
            @params.Page = _params.Page < 0 ? 1 : _params.Page;

            BoardViewModel viewModel = _dataManagers.GetNoticeBoardViewData(@params);
            viewModel.BoardName = GetBoardName(@params.Category);

            return viewModel;
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
