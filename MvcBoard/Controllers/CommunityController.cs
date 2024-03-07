using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcBoard.Managers;
using MvcBoard.Models.Community;
using System.Data;

namespace MvcBoard.Controllers
{
    public class CommunityController : Controller
    {
        private readonly MvcBoardDbContext _context; // TODO 안쓰면 지울 것

        private readonly CommunityDataManagers _dataManagers;

        public CommunityController(MvcBoardDbContext context)
        {
            _context = context;
            _dataManagers = new CommunityDataManagers();
        }

        // 전체 게시판
        public IActionResult Index(int? category = null, int? page = 1)
        {
            BoardViewModel viewModel = _dataManagers.GetBoardViewData(category ?? 0, page ?? 1);

            return View(viewModel);
        }

        // 인기 게시판
        public IActionResult Hot(int? category = null, int? page = 1)
        {
            // TODO 별도 함수 만들어서 호출 필요
            BoardViewModel viewModel = _dataManagers.GetBoardViewData(2, page ?? 1); // 2는 질문답변 게시판 카테고리 번호임

            return View(viewModel);
        }

        // 공지 게시판
        public IActionResult Notice(int? category = null, int? page = 1)
        {
            // TODO 별도 함수 만들어서 호출 필요 (isNotice)
            BoardViewModel viewModel = _dataManagers.GetBoardViewData(99, page ?? 1); // TODO 99 는 임시, 수정 필요

            return View(viewModel);
        }

        public IActionResult Write(int category = 0)
        {
            // WriteViewModel viewModel = new WriteViewModel(category); // TODO WreiteViewModel class 삭제 고려
            Post viewModel = new Post();
            viewModel.Category = category;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Write(Post postData)
        {
            Console.WriteLine($"[Controller] Community >> Write() postData.Category: {postData.Category},  IsValid: {ModelState.IsValid}"); // TODO stringify

            if (ModelState.IsValid)
            {
                if (postData.Category == 0) postData.Category = 1; // 1: 자유게시판 (default), todo 드롭다운 or 모달 구현하면 필요 없음

                // TODO 작성자 UserId 매핑 필요 (로그인 구현 후)
                postData.UserId = 1;

                _dataManagers.CreatePost(postData);

                // todo 정의될 category 에 맞게 수정 필요
                switch (postData.Category)
                {
                    case 0:
                    case 1: return RedirectToAction("Index");
                    case 2: return RedirectToAction("Hot");
                    case 99: return RedirectToAction("Notice");

                    default: return RedirectToAction("Index");
                }

                // ModelState.AddModelError(string.Empty, "게시물을 저장할 수 없습니다."); // TODO
            }
            return View(postData);
        }

        // TODO 게시물 뷰 페이지 
        [HttpGet("Community/View/{PostId}")]
        public IActionResult View(int postId, int? page, int category = 0, int commentPage = 1)
        {
            // TODO 게시물 데이터 (by postId), 댓글 데이터 조인 필요? -> 조인이 아니고 저장프로시저에서 SELECT 쿼리 결과 2개 반환하고 reader.NextResult()
            PostWithUser? postData = _dataManagers.GetPostDataById(postId);

            if (postData == null) {
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
        }
           

            // 글쓰기 테스트
        public IActionResult CreateTest()
        {
            Post postData = new Post
            {
                PostId = 0, // TODO 전달하지 않는 값
                Title = "테스트 제목",
                Contents = "테스트 내용",
                UserId = 50,
                Likes = 10,
                Views = 10,
                Category = 1,
                CreateDate = DateTime.Now,
                UpdateDate = null,
                DeleteDate = null
            };
            // Post postData2 = new Post(0, "테스트 제목", "테스트 내용", 50, 10, 10, 10, 1, DateTime.Now, null, null); // 생성자 필요한데, 추가해도 상관 없는건가 스캐폴딩 다시 하면 지워질텐데

            _dataManagers.CreatePost(postData);

            return View();
        }

        // 글수정 테스트
        public IActionResult UpdateTest(int postId = -1) // TODO Post형 데이터 받아서 업데이트 하도록 수정 필요
        {
            if (postId == -1) return View();

            Post postData = new Post
            {
                PostId = postId,
                Title = "테스트 제목 (수정됨)",
                Contents = "테스트 내용 (수정됨)",
                UserId = 50,
                Likes = 10,
                Views = 10,
                Category = 1,
                CreateDate = DateTime.Now,
                UpdateDate = null,
                DeleteDate = null
            };
            // Post postData2 = new Post(8, "테스트 제목", "테스트 내용", 50, 10, 10, 10, 1, DateTime.Now, null, null); // 생성자 필요한데, 추가해도 상관 없는건가 스캐폴딩 다시 하면 지워질텐데

            _dataManagers.UpdatePost(postData);

            return View();
        }

        // 게시글 삭제 테스트
        public IActionResult DeleteTest(int postId = -1)
        {
            if (postId != -1)
            {
                _dataManagers.DeletePost(postId);
            }
         
            return View();
        }


        // TODO 지울 것
        public List<Post> TestGetNotice(int page = 1)
        {
            Console.WriteLine($"@ CommunityController >> TestGetNotice(page = {page})");

            int category = 1; // todo 99

            // DataSet ds = new DataSet();
            // DataTable dt = new DataTable();
            // string nReturn = string.Empty;

            int count = 0;

            List<Post> Posts = new List<Post>();
            
            using (SqlConnection con = new SqlConnection("Server=DESKTOP-5AFMG8G;Database=MVC_BOARD_DB;Trusted_Connection=true;TrustServerCertificate=True"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("ReadPost", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@page", page);


                    // SqlDataAdapter da = new SqlDataAdapter(cmd);


                    // 방법1
                    // da.Fill(ds);
                    /*
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            Console.WriteLine(r["DESC"]);
                        }
                    }
                    */

                    // 방법2
                    /*
                    da.Fill(dt);
                    

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            nReturn = dr[0].ToString();
                        }
                    }
                    */

                    // 방법3
                   
                    SqlDataReader reader = cmd.ExecuteReader();
                    // List<Post> Posts = new List<Post>();
                    Post post = null;

                    while (reader.Read()) 
                    {
                        post = new Post();
                        post.PostId = int.Parse(reader["PostId"].ToString());
                        post.Title = reader["Title"].ToString();
                        post.Contents = reader["Contents"].ToString();
                        post.UserId = int.Parse(reader["UserId"].ToString());
                        post.Likes = int.Parse(reader["Likes"].ToString());
                        post.Views = int.Parse(reader["Views"].ToString());
                        post.Category = int.Parse(reader["Category"].ToString());

                        // TODO 시간 처리...
                        // post.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString());
                        // post.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString());
                        // post.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString());

                        Posts.Add(post);
                        count++;
                    }


                    ViewData["Count"] = count;

                    string testString = "";

                    foreach (var p in Posts)
                    {
                        testString += $"{p.Title} ";
                    }

                    ViewData["testString"] = testString;

                    reader.Close();
                    con.Close();

                }
            }


            return Posts;
            // return nReturn?. "";
        }
        

    }
}
