﻿using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
using System.Data;

namespace MvcBoardAdmin.Managers
{
    public class BoardDataManager : DBManager
    {
        public BoardDataManager(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## BoardDataManager() initialized...");
        }

        // 게시판 카테고리(메뉴) 조회
        public List<BoardType> GetBoardTypeData()
        {
            Console.WriteLine($"## BoardDataManager >> GetBoardTypeData()");

            List<BoardType> BoardTypes = new List<BoardType>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetSortedBoards", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataReader reader = command.ExecuteReader();

                    BoardType? board = null;

                    /* 게시판 카테고리 데이터 */
                    while (reader.Read())
                    {
                        board = new BoardType();

                        board.BoardId = int.Parse(reader["BoardId"]?.ToString() ?? "0");
                        board.BoardName = reader["BoardName"]?.ToString() ?? "";
                        board.Category = int.Parse(reader["Category"]?.ToString() ?? "0");
                        board.ParentCategory = int.Parse(reader["ParentCategory"]?.ToString() ?? "0");
                        board.IsParent = reader.GetBoolean(reader.GetOrdinal("IsParent"));
                        board.IconType = int.Parse(reader["IconType"]?.ToString() ?? "0");
                        board.IsWritable = reader.GetBoolean(reader.GetOrdinal("IsWritable"));
                        board.ShowOrder = int.Parse(reader["ShowOrder"]?.ToString() ?? "0");

                        BoardTypes.Add(board);
                    }

                    reader.Close();
                }
                connection.Close();
            }

            return BoardTypes;
        }

        /// <summary>
        /// 게시판 생성 요청 
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse CreateBoard(CreateBoardParams _params)
        {
            CommonResponse Response = new CommonResponse();

            Console.WriteLine($"## MemberDataManager >> CreateBoard(BoardId = {_params.BoardId}, BoardName = {_params.BoardName}, Category = {_params.Category}, ParentCategory = {_params.ParentCategory}, ShowOrder = {_params.ShowOrder}, IconType = {_params.IconType}, IsParent = {_params.IsParent}, IsWritable = {_params.IsWritable})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreateBoardType", connection)) // adm_CreateBoard 
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoardName", _params.BoardName);
                        command.Parameters.AddWithValue("@Category", _params.Category);
                        command.Parameters.AddWithValue("@ParentCategory", _params.ParentCategory);
                        command.Parameters.AddWithValue("@IsParent", _params.IsParent);
                        command.Parameters.AddWithValue("@IconType", _params.IconType);
                        command.Parameters.AddWithValue("@IsWritable", _params.IsWritable);
                        command.Parameters.AddWithValue("@ShowOrder", _params.ShowOrder);

                        SqlDataReader reader = command.ExecuteReader();

                        if (!reader.Read())
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Read Fail";
                            return Response;
                        }

                        int result = Convert.ToInt32(reader["Result"]);

                        // TODO 에러 코드 및 메시지 정의
                        switch (result)
                        {
                            case 1:
                                Response.ResultCode = 200;
                                Response.Message = "DB Success";
                                break;
                            case 0:
                                Response.ResultCode = 203;
                                Response.Message = "DB Fail";
                                break;
                            case -1:
                                Response.ResultCode = 203;
                                Response.Message = "Category 번호는 중복될 수 없습니다."; // 비즈니스 로직에 따라 중복이 필요한 경우 수정
                                break;
                            case -2:
                                Response.ResultCode = 203;
                                Response.Message = "입력값 오류 (-2)";
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

        /// <summary>
        /// 게시판 정보 상세 조회
        /// </summary>
        /// <param name="BoardId">유저 고유 번호(UserNumer)</param>
        /// <returns></returns>
        /// -> 캐시 데이터 사용 로직으로 변경하여 주석처리
        /*
        public BoardDetailResponse ReadBoardDetail(int BoardId)
        {
            BoardDetailResponse Response = new BoardDetailResponse();
            BoardEditorViewModel Model = new BoardEditorViewModel();

            Console.WriteLine($"## BoardDataManager >> ReadBoardDetail(BoardId = {BoardId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_BoardDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BoardId", BoardId);

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Model.BoardId = int.Parse(reader["BoardId"]?.ToString() ?? "0");
                            Model.BoardName = reader["BoardName"]?.ToString() ?? "";
                            Model.Category = int.Parse(reader["Category"]?.ToString() ?? "0");
                            Model.ParentCategory = int.Parse(reader["ParentCategory"]?.ToString() ?? "0");
                            Model.IsParent = reader.GetBoolean(reader.GetOrdinal("IsParent"));
                            Model.IconType = int.Parse(reader["IconType"]?.ToString() ?? "0");
                            Model.IsWritable = reader.GetBoolean(reader.GetOrdinal("IsWritable"));
                            Model.ShowOrder = int.Parse(reader["IconType"]?.ToString() ?? "0");
                        }

                        reader.Close();
                    }
                    connection.Close();
                }

                Response.ResultCode = 200;
                Response.Message = "DB Success";
                Response.ViewModel = Model;
            }
            catch (Exception ex)
            {
                Response.ResultCode = 202;
                Response.Message = "DB Error";
                Response.ErrorMessage.Add(ex.Message);
            }

            return Response;
        }
        */
    }
}
