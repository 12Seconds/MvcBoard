﻿using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Utills;
using System.Data;

namespace MvcBoardAdmin.Managers
{
    public class PostDataManager : DBManager
    {
        public PostDataManager(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## PostDataManager() initialized...");
        }

        /// <summary>
        /// 게시물 조회
        /// </summary>
        /// <param name="_params">검색 필터, 검색어, 페이지</param>
        /// <returns></returns>
        public ReadPostsResponse ReadPosts(ReadPostsParams _params)
        {
            ReadPostsResponse Response = new ReadPostsResponse();

            Console.WriteLine($"## PostDataManager >> ReadPosts(BoardFilter = {_params.BoardFilter}, SearchFilter = {_params.SearchFilter}, SearchWord = {_params.SearchWord}, Page = {_params.Page})");

            int pageCount = 0;
            int totalResultCount = 0;

            List<PostWithUser> Posts = new List<PostWithUser>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_ReadPosts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoardFilter", _params.BoardFilter);
                        command.Parameters.AddWithValue("@SearchFilter", _params.SearchFilter);
                        command.Parameters.AddWithValue("@SearchWord", _params.SearchWord);
                        command.Parameters.AddWithValue("@PageIndex", _params.Page);
                        command.Parameters.AddWithValue("@RowPerPage", 10); // 테스트용 (default: 10)

                        SqlDataReader reader = command.ExecuteReader();

                        PostWithUser? post = null;

                        /* 게시물 데이터 */
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

                            post.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["UpdateDate"] != DBNull.Value) post.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["DeleteDate"] != DBNull.Value) post.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                            post.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            post.IsBlinded = reader.GetBoolean(reader.GetOrdinal("IsBlinded"));
                            post.IsNotice = reader.GetBoolean(reader.GetOrdinal("IsNotice"));

                            // Join 테이블 데이터
                            post.UserName = reader["Name"]?.ToString() ?? "";
                            post.BoardName = reader["BoardName"]?.ToString() ?? "";
                            post.CommentCount = int.Parse(reader["CommentCount"]?.ToString() ?? "0");

                            Posts.Add(post);
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

                        Response.ResultCode = 200;
                        Response.Message = "DB Success";

                        reader.Close();
                    }
                    connection.Close();
                }

                Response.ViewModel.PostList = Posts;
                Response.ViewModel.PageIndex = _params.Page;
                Response.ViewModel.TotalRowCount = totalResultCount;
                Response.ViewModel.TotalPageCount = pageCount;
                Response.ViewModel.IndicatorRange = Utility.GetIndicatorRange(new Utility.IndicatorRangeParams { Page = _params.Page, PageCount = pageCount });

                Response.ViewModel.ExBoardFilter = _params.BoardFilter;
                Response.ViewModel.ExSearchFilter = _params.SearchFilter;
                Response.ViewModel.ExSearchWord = _params.SearchWord;
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
        /// 게시물 상세 조회
        /// </summary>
        /// <param name="_params">검색 필터, 검색어, 페이지</param>
        /// <returns></returns>
        public ReadPostDetailResult ReadPostDetail(int PostId)
        {
            ReadPostDetailResult Result = new ReadPostDetailResult();

            List<PostWithUser> Posts = new List<PostWithUser>();

            Console.WriteLine($"## PostDataManager >> ReadPostDetail(PostId = {PostId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_ReadPostDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PostId", PostId);

                        SqlDataReader reader = command.ExecuteReader();

                        PostWithUser? Post = null; // TODO Question PostDetail ?

                        /* 게시물 데이터 */
                        while (reader.Read())
                        {
                            Post = new PostWithUser();

                            Post.PostId = int.Parse(reader["PostId"]?.ToString() ?? "0");
                            Post.Title = reader["Title"]?.ToString() ?? "";
                            Post.Contents = reader["Contents"].ToString();
                            Post.UserId = int.Parse(reader["UserId"]?.ToString() ?? "0");
                            Post.Likes = int.Parse(reader["Likes"]?.ToString() ?? "0");
                            Post.Views = int.Parse(reader["Views"]?.ToString() ?? "0");
                            Post.Category = int.Parse(reader["Category"]?.ToString() ?? "0");

                            Post.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["UpdateDate"] != DBNull.Value) Post.UpdateDate = DateTime.Parse(reader["UpdateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["DeleteDate"] != DBNull.Value) Post.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01");

                            Post.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            Post.IsBlinded = reader.GetBoolean(reader.GetOrdinal("IsBlinded"));
                            Post.IsNotice = reader.GetBoolean(reader.GetOrdinal("IsNotice"));

                            // Join 테이블 데이터
                            Post.LoginId = reader["Id"]?.ToString() ?? "";
                            Post.UserName = reader["Name"]?.ToString() ?? "";
                            Post.BoardName = reader["BoardName"]?.ToString() ?? "";
                            Post.CommentCount = int.Parse(reader["CommentCount"]?.ToString() ?? "0");

                            Posts.Add(Post);
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
                Result.Post = new PostWithUser();
                return Result;
            }

            if (Posts.Count < 1)
            {
                Result.Response.ResultCode = 203;
                Result.Response.Message = "DB Fail (게시물 정보를 찾을 수 없습니다.)";
                Result.Post = new PostWithUser();
                return Result;
            }

            Result.Response.ResultCode = 200;
            Result.Response.Message = "DB Success";
            Result.Post = Posts[0];
            return Result;
        }


    }
}
