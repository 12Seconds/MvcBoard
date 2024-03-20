using Microsoft.Data.SqlClient;
using System.Data;
using MvcBoard.Models.Community;
using MvcBoard.Managers.Models;
using MvcBoard.Models;

namespace MvcBoard.Managers
{
    // TODO SingleTone?

    /// <summary>
    /// 커뮤니티 (게시판) 기능과 관련된 데이터를 처리하는 매니저로, DB와의 통신은 Stored Procedure 호출로만 이루어진다.
    /// </summary>
    public class CommunityDataManagers : DBManager
    {
        public CommunityDataManagers(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## CommunityDataManagers() initialized...");
        }

        // 게시판 조회 --TODO ReadPost함수 분리...필요한가?
        public BoardViewModel GetBoardViewData(GetBoardParams _params)
        {
            // TODO 로그 모듈(매니저) 만들기
            Console.WriteLine($"## CommunityDataManager >> GetBoardViewData(category = {_params.Category}, page = {_params.Page}), size = {_params.Size})");

            int pageCount = 0;

            List<PostWithUser> Posts = new List<PostWithUser>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ReadPost", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.READPOST 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Category", _params.Category);
                    command.Parameters.AddWithValue("@Page", _params.Page);
                    command.Parameters.AddWithValue("@Size", _params.Size);

                    SqlDataReader reader = command.ExecuteReader();

                    PostWithUser? post = null;

                    /* 게시판 데이터 */
                    while (reader.Read())
                    {
                        post = new PostWithUser();

                        post.PostId = int.Parse(reader["PostId"]?.ToString() ?? "0");
                        post.Title = reader["Title"]?.ToString() ?? "";
                        post.Contents = reader["Contents"].ToString();
                        post.UserId = int.Parse(reader["UserId"]?.ToString() ?? "0");
                        post.Likes = int.Parse(reader["Likes"]?.ToString() ?? "0");
                        post.Views = int.Parse(reader["Views"]?.ToString() ?? "0");
                        post.Category = int.Parse(reader["Category"]?.ToString() ?? "0");

                        post.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01"); // TODO
                        if (reader["UpdateDate"] != DBNull.Value) post.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                        if (reader["DeleteDate"] != DBNull.Value) post.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                        // Join 테이블 데이터
                        post.UserName = reader["Name"]?.ToString() ?? "";

                        Posts.Add(post);
                    }

                    /* 총 페이지 수 */
                    reader.NextResult();
                    if (reader.Read())
                    {
                        pageCount = Convert.ToInt32(reader["TotalPageCount"]);
                        Console.WriteLine($"@@@@@@@ ## pageCount: {pageCount}"); // TODO 삭제
                    }

                    reader.Close();
                }
                connection.Close();
            }
            
            // TODO 여기도.. 뭔가 간소화 필요
            return new BoardViewModel(pageCount, _params.Page, _params.Category, _params.Size, Posts); 
        }

        // 게시물 작성 (todo return 타입 수정 필요)
        public void CreatePost(Post post)
        {
            // Newtonsoft.Json https://www.delftstack.com/ko/howto/csharp/how-to-convert-a-csharp-object-to-a-json-string-in-csharp/ 
            Console.WriteLine($"## CommunityDataManager >> CreatePost(postData = {post.Title} | {post.Contents})"); // postData json 형태로 stringify 하여 출력?

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreatePost", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.CreatePost 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", post.Title);
                    command.Parameters.AddWithValue("@Contents", post.Contents);
                    command.Parameters.AddWithValue("@UserId", post.UserId);
                    command.Parameters.AddWithValue("@Category", post.Category);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                   

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);
                    
                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }

        // 게시물 조회
        // 코드를 중복하느냐, 불필요한 Join을 공유하느냐..
        // public PostWithUser? GetPostById(int? postId = null) { }

        // 게시물 상세 조회 (유저 테이블 조인)
        public PostWithUser? GetPostWithUserById(int? postId = null)
        {
            List<PostWithUser> posts = new List<PostWithUser>();

            if (postId == null) return null;

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ReadPostById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", postId);

                    SqlDataReader reader = command.ExecuteReader();

                    PostWithUser? post = null;

                    while (reader.Read())
                    {
                        post = new PostWithUser();

                        post.PostId = int.Parse(reader["PostId"]?.ToString() ?? "0");
                        post.Title = reader["Title"]?.ToString() ?? "";
                        post.Contents = reader["Contents"].ToString();
                        post.UserId = int.Parse(reader["UserId"]?.ToString() ?? "0");
                        post.Likes = int.Parse(reader["Likes"]?.ToString() ?? "0");
                        post.Views = int.Parse(reader["Views"]?.ToString() ?? "0");
                        post.Category = int.Parse(reader["Category"]?.ToString() ?? "0");

                        post.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01"); // TODO
                        if (reader["UpdateDate"] != DBNull.Value) post.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                        if (reader["DeleteDate"] != DBNull.Value) post.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                        // Join 테이블 데이터
                        post.LoginId = reader["Id"]?.ToString() ?? "";
                        post.UserName = reader["Name"]?.ToString() ?? "";
                        post.UserImage = int.Parse(reader["Image"]?.ToString() ?? "0");

                        posts.Add(post);
                    }

                    reader.Close();
                }
                connection.Close();
            }

            if (posts.Count > 0)
                return posts[0];
            else
                return null;
        }

        // 게시물 수정
        public void UpdatePost(Post post) // TODO SP 에서 postId 유효성 처리 해야함
        {
            // Newtonsoft.Json https://www.delftstack.com/ko/howto/csharp/how-to-convert-a-csharp-object-to-a-json-string-in-csharp/ 
            Console.WriteLine($"## CommunityDataManager >> UpdatePost(postData = {post.PostId} | {post.Title})"); // postData json 형태로 stringify 하여 출력?

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdatePost", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.CreatePost 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", post.PostId);
                    command.Parameters.AddWithValue("@Title", post.Title);
                    command.Parameters.AddWithValue("@Contents", post.Contents);
                    command.Parameters.AddWithValue("@Likes", post.Likes);
                    command.Parameters.AddWithValue("@Views", post.Views);
                    command.Parameters.AddWithValue("@Category", post.Category);
                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now); // TODO SP 와 같이 확인 수정 필요

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);

                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }

        // 게시물 삭제
        public void DeletePost(int postId)
        {
            Console.WriteLine($"## CommunityDataManager >> DeletePost(postId = {postId})");
            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeletePost", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.CreatePost 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", postId);
                    command.Parameters.AddWithValue("@forceDelete", 1); // TODO 아 왜 또 얜 소문자로했지..

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);

                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }

        /* @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ */

        // TODO 게시물...데이터 조합? 전달 받은 PostData 있으면 쓰고 없으면 조회?
        /*
        public BoardViewModel GetPostViewData(int category = 1, int page = 1) // TODO category type 상수 정의 및 참조 (1: 자유게시판 ~ 99: 공지)
        {
           
        }
        */

        /* @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ */

        // 특정 게시물에 작성된 모든 댓글 조회
        public CommentsViewModel ReadCommentByPostId(GetCommentsByPostIdParams _params)
        {
            // TODO 로그 모듈(매니저) 만들기
            Console.WriteLine($"## CommunityDataManager >> ReadCommentByPostId(postId = {_params.PostId})");

            int pageCount = 0;

            List<CommentWithUser> Comments = new List<CommentWithUser>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ReadComment", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.ReadComment 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", _params.PostId);
                    command.Parameters.AddWithValue("@Page", _params.CommentPage);
                    command.Parameters.AddWithValue("@Size", _params.CommentPageSize);

                    SqlDataReader reader = command.ExecuteReader();

                    CommentWithUser? comment = null;

                    while (reader.Read())
                    {
                        comment = new CommentWithUser();
                        comment.CommentId = int.Parse(reader["CommentId"]?.ToString() ?? "0");
                        comment.PostId = int.Parse(reader["PostId"].ToString() ?? "0");
                        comment.UserId= int.Parse(reader["UserId"].ToString() ?? "0");
                        comment.ParentId = (reader["ParentId"] != DBNull.Value) ? int.Parse(reader["ParentId"]?.ToString() ?? "0") : 0;
                        comment.Contents = reader["Contents"]?.ToString() ?? "";
                        comment.Likes = int.Parse(reader["Likes"].ToString() ?? "0");

                        comment.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("IsAnonymous"));
                        // comment.IsAnonymous = bool.Parse(reader["IsAnonymous"]?.ToString() ?? "false");

                        comment.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01"); // TODO
                        if (reader["UpdateDate"] != DBNull.Value) comment.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                        if (reader["DeleteDate"] != DBNull.Value) comment.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                        // Join 테이블 데이터
                        comment.UserName = reader["Name"]?.ToString() ?? "";

                        Comments.Add(comment);
                    }

                    /* 총 페이지 수 */
                    reader.NextResult();
                    if (reader.Read())
                    {
                        pageCount = Convert.ToInt32(reader["TotalPageCount"]);
                        Console.WriteLine($"@@@@@@@ ## pageCount: {pageCount}"); // TODO 삭제
                    }

                    reader.Close();
                }
                connection.Close();
            }

            return new CommentsViewModel
            {
                CommentListData = Comments,
                CommentPage = _params.CommentPage,
                CommentPageSize = _params.CommentPageSize,
                CommentPageCount = pageCount, // todo Page
                // TODO 진짜 이게 맞나
                PostId = _params.PostId,
                Category = _params.Category,
                Page = _params.Page,
            };
        }

        // 댓글 작성
        public void CreateComment(Comment comment)
        {
            // Newtonsoft.Json https://www.delftstack.com/ko/howto/csharp/how-to-convert-a-csharp-object-to-a-json-string-in-csharp/ 
            Console.WriteLine($"## CommunityDataManager >> createComment(commentData = {comment.PostId} | {comment.Contents})"); // postData json 형태로 stringify 하여 출력?

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreateComment", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.CreateComment 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", comment.PostId);
                    command.Parameters.AddWithValue("@UserId", comment.UserId);
                    command.Parameters.AddWithValue("@ParentId", comment.ParentId);
                    command.Parameters.AddWithValue("@Contents", comment.Contents);
                    command.Parameters.AddWithValue("@IsAnonymous", comment.IsAnonymous);
                    command.Parameters.AddWithValue("@CreateDate", DateTime.Now);

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);

                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }

        // 댓글 수정 (테스트 필요)
        public void UpdateComment(Comment comment)
        {
            // Newtonsoft.Json https://www.delftstack.com/ko/howto/csharp/how-to-convert-a-csharp-object-to-a-json-string-in-csharp/ 
            Console.WriteLine($"## CommunityDataManager >> UpdateComment(commentData = {comment.CommentId} | {comment.Contents})"); // postData json 형태로 stringify 하여 출력?

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdateComment", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.UpdateComment 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentId", comment.CommentId);
                    command.Parameters.AddWithValue("@Contents", comment.Contents);
                    command.Parameters.AddWithValue("@Likes", comment.Likes);
                    command.Parameters.AddWithValue("@IsAnonymous", comment.IsAnonymous);
                    command.Parameters.AddWithValue("@UpdateDate", DateTime.Now); // TODO SP 와 같이 확인 수정 필요

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);

                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }

        // 댓글 삭제 (테스트 필요)
        // TODO 댓글을 삭제하더라도 대댓글은 남겨야 하므로 IsDeleted 필드를 추가해서 살려놓아야 할 것 같은데...
        public void DeleteComment(int commentId)
        {
            Console.WriteLine($"## CommunityDataManager >> DeleteComment(commentId = {commentId})");
            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeleteComment", connection)) // TODO SP List 상수 정의 필요. BOARD.SP.DeleteComment 이런 식
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentId", commentId);
                    command.Parameters.AddWithValue("@forceDelete", 1); // TODO 아 왜 또 얜 소문자로했지..

                    // 출력 파라미터
                    // SqlParameter result = new SqlParameter("@???", SqlDbType.VarChar);
                    // result.Direction = ParameterDirection.Output;
                    // command.Parameters.Add(result);

                    SqlDataReader reader = command.ExecuteReader();

                    // TODO 결과 확인 및 처리
                    /*
                    while (reader.Read())
                    {

                    }
                    */

                    reader.Close();
                }
                connection.Close();
            }
        }



    }
}
