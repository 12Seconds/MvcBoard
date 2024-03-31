using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{

    // TODO 각 액션 메소드에 인증 로직 추가 필요

    public class BoardService
    {

        private readonly BoardDataManager _boardDataManagers;

        private DateTime _cachedTime;

        private TimeSpan cacheDuration = TimeSpan.FromMinutes(1); // 캐시 유효 시간: 1분 (개발용으로 1분 -> 비즈니스 로직에 따라 적절하게 설정할 것)

        private List<BoardType>? _cachedBoardTypeData = null;
        private List<BoardType> _cachedSortedBoardTypeData = new List<BoardType>();
        private List<BoardType> _cachedParentBoardTypeData = new List<BoardType>();

        private bool NeedUpdate = false; // 게시판 정보가 수정되면 업데이트 필요함을 알리는 flag 세팅

        public BoardService(BoardDataManager dataManager)
        {
            _boardDataManagers = dataManager;
        }

        // TODO 조회도 실패할 수 있기 때문에 Response 를 반환하는 구조로 변경할 것

        /// <summary>
        /// 게시판 카테고리(메뉴) 데이터 조회
        /// </summary>
        /// <returns></returns>
        public List<BoardType> GetBoardTypeData()
        {
            // 최초 조회거나, 캐시가 만료된 경우 재조회
            if (_cachedBoardTypeData == null || DateTime.Now - _cachedTime > cacheDuration || NeedUpdate)
            {
                Console.WriteLine("### BoardService >> GetBoardTypeData() --- Get New BoardTypeData (cache expired!)");
                _cachedParentBoardTypeData.RemoveRange(0, _cachedParentBoardTypeData.Count);
                _cachedBoardTypeData = _boardDataManagers.GetBoardTypeData();
                _cachedSortedBoardTypeData = SetHierarchy(_cachedBoardTypeData);
                _cachedTime = DateTime.Now;
            }

            return _cachedSortedBoardTypeData;
        }

        /// <summary>
        /// 특정 게시판의 정보만 요청
        /// </summary>
        /// <param name="_parmas"></param>
        /// <returns></returns>
        public BoardDetailResponse ReadBoardDetail(ReadBoardDetailServiceParams _params)
        {
            BoardDetailResponse Response = new BoardDetailResponse();

            // 수정 불가능한 게시판의 정보 요청인 경우 (전체, 인기, 공지)
            if (_params.BoardId > 0 && _params.BoardId < 3)
            {
                Response.ResultCode = 201;
                Response.Message = "유효하지 않은 BoardId 입니다.";
                return Response;
            }

            // Response = _boardDataManagers.ReadBoardDetail(_parmas.BoardId);
            try
            {
                GetBoardTypeData();

                // 게시판 데이터에서 찾아서 반환
                BoardType found = _cachedBoardTypeData.Find(board => board.BoardId == _params.BoardId);

                // 생성인 경우
                if (_params.BoardId == 0)
                {
                    found = new BoardType();
                    Response.ViewModel.IsNew = true;
                }

                if (found == null)
                {
                    Response.ResultCode = 203;
                    Response.Message = "해당 게시판의 정보를 찾을 수 없습니다.";
                    return Response;
                }

                Response.ViewModel.Board = found;

                // 부모 게시판 데이터 넣어주기
                Response.ViewModel.Parents = _cachedParentBoardTypeData;
                Response.Message = "DB Success";

                return Response;
            }
            catch (Exception ex)
            {
                Response.ResultCode = 203;
                Response.Message = "DB Fail";
                Response.ErrorMessages.Add(ex.Message);
                return Response;
            }
            
        }

        /// <summary>
        /// 게시판 생성 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse CreateBoard (CreateBoardServiceParams _params)
        {
            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 검증 통과시 DB 요청
            Response = _boardDataManagers.CreateBoard(_params.CreateParams);

            // 캐시 갱신 플래그 설정
            if (Response.ResultCode == 200)
            {
                NeedUpdate = true;
            }

            return Response;
        }


        /* 게시판 데이터를 부모-자식 계층으로 가공 (실제 부모 Board 객체의 Childen 리스트에 자식 Board 넣는 작업, DB에서 순서는 정렬하여 보내줌) */
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
            for (int i = 0; i < origin.Count ; i++)
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
                    for (int j = i+1; j < origin.Count ; j++)
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

            // 테스트용 코드
            // result.Add(result[5]);
            _cachedParentBoardTypeData = parents;

            return result;
        }

    }
}
