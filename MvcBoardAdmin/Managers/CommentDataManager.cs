using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Utills;
using System.Data;

namespace MvcBoardAdmin.Managers
{

    // 1. 데이터를 반환하는 DB 요청인 경우(SELECT, ..) : Result 반환 (CommonResponse 와 데이터를 포함하는 Result Class 정의)
    // 2. 처리 결과를 반환하는 DB 요청인 경우(UPDATE, DELETE, ..) : CommonResponse 반환

    public class CommentDataManager : DBManager
    {
        public CommentDataManager(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## CommentDataManager() initialized...");
        }

        /// <summary>
        /// 댓글 조회 (검색)
        /// </summary>
        /// <param name="_params">검색 필터, 검색어, 페이지</param>
        /// <returns></returns>
        public ReadCommentsResult ReadComments(ReadCommentsParams _params)
        {
            ReadCommentsResult Result = new ReadCommentsResult();

            List<CommentWithUser> Comments = new List<CommentWithUser>();

            int pageCount = 0;
            int totalResultCount = 0;

            Console.WriteLine($"## CommentDataManager >> ReadComments(BoardFilter = {_params.BoardFilter}, SearchFilter = {_params.SearchFilter}, SearchWord = {_params.SearchWord}, Page = {_params.Page})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_ReadComments", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoardFilter", _params.BoardFilter);
                        command.Parameters.AddWithValue("@SearchFilter", _params.SearchFilter);
                        command.Parameters.AddWithValue("@SearchWord", _params.SearchWord);
                        command.Parameters.AddWithValue("@PageIndex", _params.Page);
                        command.Parameters.AddWithValue("@RowPerPage", 10); // 테스트용 (default: 10)

                        SqlDataReader reader = command.ExecuteReader();

                        CommentWithUser? comment = null;

                        /* 댓글 데이터 */
                        while (reader.Read())
                        {
                            comment = new CommentWithUser();

                            comment.CommentId = int.Parse(reader["CommentId"]?.ToString() ?? "0");
                            comment.PostId = int.Parse(reader["PostId"].ToString() ?? "0");
                            comment.UserId = int.Parse(reader["UserId"].ToString() ?? "0");
                            comment.ParentId = (reader["ParentId"] != DBNull.Value) ? int.Parse(reader["ParentId"]?.ToString() ?? "0") : 0;
                            comment.Contents = reader["Contents"]?.ToString() ?? "";
                            comment.Likes = int.Parse(reader["Likes"].ToString() ?? "0");

                            comment.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("IsAnonymous"));
                            // comment.IsAnonymous = bool.Parse(reader["IsAnonymous"]?.ToString() ?? "false");

                            comment.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["UpdateDate"] != DBNull.Value) comment.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["DeleteDate"] != DBNull.Value) comment.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                            comment.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            comment.IsBlinded = reader.GetBoolean(reader.GetOrdinal("IsBlinded"));

                            // Join 테이블 데이터
                            comment.UserName = reader["Name"]?.ToString() ?? "";
                            comment.Category = int.Parse(reader["Category"]?.ToString() ?? "0");
                            comment.BoardName = reader["BoardName"]?.ToString() ?? "";

                            // TODO IsPostWriter

                            Comments.Add(comment);
                        }

                        /* 총 페이지 수 */
                        reader.NextResult();
                        if (reader.Read())
                        {
                            pageCount = Convert.ToInt32(reader["TotalPageCount"]);
                        }

                        /* 총 결과 수 */
                        reader.NextResult();
                        if (reader.Read())
                        {
                            totalResultCount = Convert.ToInt32(reader["TotalResultCount"]);
                        }

                        reader.Close();
                    }
                    connection.Close();
                }

                Result.Response.ResultCode = 200;
                Result.Response.Message = "DB Success";

                Result.CommentList = Comments;
                Result.TotalRowCount = totalResultCount;
                Result.TotalPageCount = pageCount;
                Result.IndicatorRange = Utility.GetIndicatorRange(new Utility.IndicatorRangeParams { Page = _params.Page, PageCount = pageCount });
            }
            catch (Exception ex)
            {
                Result.Response.ResultCode = 202;
                Result.Response.Message = "DB Error";
                Result.Response.ErrorMessages.Add(ex.Message);
            }

            return Result;
        }

        // TODO 특정 게시물에 작성된 모든 댓글 조회
        /*
        public ReadCommentsResult ReadCommentByPostId(ReadCommentsParams _params)
        {
        }
        public CommentsViewModel ReadCommentByPostId(GetCommentsByPostIdParams _params)
        */


        /// <summary>
        /// 댓글 상세 조회
        /// </summary>
        /// <param name="CommentId">댓글 고유 번호</param>
        /// <returns></returns>
        public ReadCommentDetailResult ReadCommentDetail(int CommentId)
        {
            ReadCommentDetailResult Result = new ReadCommentDetailResult();

            List<CommentDetail> Comments = new List<CommentDetail>();

            Console.WriteLine($"## CommentDataManager >> ReadCommentDetail(CommentId = {CommentId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_ReadCommentDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CommentId", CommentId);

                        SqlDataReader reader = command.ExecuteReader();

                        CommentDetail? CommentDetail = null;

                        while (reader.Read())
                        {
                            CommentDetail = new CommentDetail();

                            CommentDetail.CommentId = int.Parse(reader["CommentId"]?.ToString() ?? "0");
                            CommentDetail.PostId = int.Parse(reader["PostId"].ToString() ?? "0");
                            CommentDetail.UserId = int.Parse(reader["UserId"].ToString() ?? "0");
                            CommentDetail.ParentId = (reader["ParentId"] != DBNull.Value) ? int.Parse(reader["ParentId"]?.ToString() ?? "0") : 0;
                            CommentDetail.Contents = reader["Contents"]?.ToString() ?? "";
                            CommentDetail.Likes = int.Parse(reader["Likes"].ToString() ?? "0");

                            CommentDetail.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("IsAnonymous"));
                            // comment.IsAnonymous = bool.Parse(reader["IsAnonymous"]?.ToString() ?? "false");

                            CommentDetail.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["UpdateDate"] != DBNull.Value) CommentDetail.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["DeleteDate"] != DBNull.Value) CommentDetail.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                            CommentDetail.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            CommentDetail.IsBlinded = reader.GetBoolean(reader.GetOrdinal("IsBlinded"));

                            // Join 테이블 데이터
                            CommentDetail.UserName = reader["Name"]?.ToString() ?? "";
                            CommentDetail.Category = int.Parse(reader["Category"]?.ToString() ?? "0");
                            CommentDetail.PostTitle = reader["PostTitle"]?.ToString() ?? "";
                            CommentDetail.BoardName = reader["BoardName"]?.ToString() ?? "";

                            // TODO IsPostWriter

                            Comments.Add(CommentDetail);
                        }

                        reader.Close();
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                Result.Response.ResultCode = 202;
                Result.Response.Message = "DB Error";
                Result.Response.ErrorMessages.Add(ex.Message);
                Result.CommentDetail = new CommentDetail();
                return Result;
            }

            if (Comments.Count < 1)
            {
                Result.Response.ResultCode = 203;
                Result.Response.Message = "DB Fail (댓글 정보를 찾을 수 없습니다.)";
                Result.CommentDetail = new CommentDetail();
                return Result;
            }

            Result.Response.ResultCode = 200;
            Result.Response.Message = "DB Success";
            Result.CommentDetail = Comments[0];
            return Result;
        }

        /// <summary>
        /// 댓글 정보 수정 - 숨김(블라인드), 삭제, 영구삭제 포함
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse UpdateComment(UpdateCommentParams _params)
        {
            CommonResponse Response = new CommonResponse();
            Response.ResultCode = 200;
            Response.Message = "DB Success";

            Console.WriteLine($"## CommentDataManager >> UpdateComment(CommentId = {_params.CommentId}, IsBlinded = {_params.IsBlinded}, IsDeleted = {_params.IsDeleted}, ExDeleted = {_params.ExDeleted}, IsHardDelete = {_params.IsHardDelete})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_UpdateComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CommentId", _params.CommentId);
                        command.Parameters.AddWithValue("@IsBlinded", _params.IsBlinded);
                        command.Parameters.AddWithValue("@IsDeleted", _params.IsDeleted);
                        command.Parameters.AddWithValue("@ExDeleted", _params.ExDeleted);
                        command.Parameters.AddWithValue("@IsHardDelete", _params.IsHardDelete);

                       SqlDataReader reader = command.ExecuteReader();

                        if (!reader.Read())
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Read Fail";
                            return Response;
                        }

                        int result = Convert.ToInt32(reader["Result"]);

                        if (result < 1)
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail (영향을 받은 행이 없음)";
                            return Response;
                        }

                        Response.Message = $"DB Success (RowCount: {result}"; // 무조건 1 이어야겠지
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.ResultCode = 202;
                Response.Message = "DB Error";
                Response.ErrorMessages.Add(ex.Message);
            }

            return Response;
        }

        /// <summary>
        /// 댓글 삭제, 숨김(블라인드) 요청
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse DeleteComment(DeleteCommentParams _params)
        {
            CommonResponse Response = new CommonResponse();

            Console.WriteLine($"## CommentDataManager >> DeleteComment(CommentId = {_params.CommentId}), IsBlinded = {_params.IsBlinded}, IsHardDelete = {_params.IsHardDelete}");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_DeleteComment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CommentId", _params.CommentId);
                        command.Parameters.AddWithValue("@IsBlinded", _params.IsBlinded);
                        command.Parameters.AddWithValue("@IsHardDelete", _params.IsHardDelete);

                        SqlDataReader reader = command.ExecuteReader();

                        if (!reader.Read())
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Read Fail";
                            return Response;
                        }

                        int result = Convert.ToInt32(reader["Result"]);

                        switch (result)
                        {
                            case 1:
                                Response.ResultCode = 200;
                                Response.Message = "DB Success";
                                break;
                            case 0:
                                Response.ResultCode = 203;
                                Response.Message = "DB Fail (존재하지 않는 댓글 입니다.)";
                                break;
                            case -1:
                                Response.ResultCode = 203;
                                Response.Message = "DB Fail (이미 삭제 또는 숨김 처리된 댓글 입니다.)";
                                break;
                            case -2:
                                Response.ResultCode = 203;
                                Response.Message = "DB Fail (입력값 오류 (-2))";
                                break;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.ResultCode = 202;
                Response.Message = "DB Error";
                Response.ErrorMessages.Add(ex.Message);
            }

            return Response;
        }

    }
}
