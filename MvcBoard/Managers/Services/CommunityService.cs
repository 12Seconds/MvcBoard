using MvcBoard.Models.Community;

namespace MvcBoard.Managers.Services
{
    /// <summary>
    /// 커뮤니티 (게시판) 기능과 관련된 비즈니스 로직 처리
    /// </summary>
    public class CommunityService
    {
        private readonly CommunityDataManagers _dataManagers;
        public CommunityService(IWebHostEnvironment env)
        {
            Console.WriteLine("### CommunityService() initialized...");
            _dataManagers = new CommunityDataManagers(env);
        }

        // 게시판 조회
        public BoardViewModel GetBoardViewData(int category = 0, int page = 1) // TODO category type 상수 정의 및 참조 (1: 자유게시판 ~ 99: 공지)
        {
            return _dataManagers.GetBoardViewData(category, page);
        }

        // 게시물 작성
        public void CreatePost(Post post)
        {
            _dataManagers.CreatePost(post);
        }

        // 게시물 조회
        public PostWithUser? GetPostDataById(int? postId = null/* int postId, int? page, int category = 0, int commentPage = 1 */)
        {
            return _dataManagers.GetPostDataById(postId);

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

        // TODO CommunityDataManager의 다른 SP 호출 함수들 구현, 매칭 필요

    }
}
