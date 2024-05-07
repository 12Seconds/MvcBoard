using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Models.Member;
using MvcBoardAdmin.Utills;
using System.Data;

namespace MvcBoardAdmin.Managers
{
    public class MemberDataManager : DBManager
    {
        public MemberDataManager(IWebHostEnvironment env) : base(env)
        {
            
        }

        /// <summary>
        /// 멤버(유저) 조회
        /// </summary>
        /// <param name="_params">검색 필터, 검색어, 페이지</param>
        /// <returns></returns>
        public MemberListViewModel ReadMembers(ReadMembersParams _params)
        {
            MemberListViewModel Model = new MemberListViewModel();

            Console.WriteLine($"## MemberDataManager >> ReadMembers(SearchFilter = {_params.SearchFilter}, SearchWord = {_params.SearchWord}, Page = {_params.Page})");

            int pageCount = 0;
            int totalResultCount = 0;

            List<User> Users = new List<User>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("adm_ReadUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchFilter", _params.SearchFilter);
                    command.Parameters.AddWithValue("@SearchWord", _params.SearchWord);
                    command.Parameters.AddWithValue("@PageIndex", _params.Page);
                    command.Parameters.AddWithValue("@RowPerPage", 5); // 테스트용 (default: 10)

                    SqlDataReader reader = command.ExecuteReader();

                    User? user = null;

                    /* 멤버(유저) 데이터 */
                    while (reader.Read())
                    {
                        user = new User();
                        user.UserId = int.Parse(reader["UserId"]?.ToString() ?? "0");
                        user.Id = reader["Id"]?.ToString() ?? "";
                        user.Name = reader["Name"].ToString() ?? "";
                        user.Image = int.Parse(reader["Image"]?.ToString() ?? "0");
                        user.Authority = reader["Authority"]?.ToString() ?? "normal";
                        user.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        Users.Add(user);
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

            Model.MemberList = Users;
            Model.PageIndex = _params.Page;
            Model.TotalRowCount = totalResultCount;
            Model.TotalPageCount = pageCount;
            Model.IndicatorRange = Utility.GetIndicatorRange(new Utility.IndicatorRangeParams { Page = _params.Page, PageCount = pageCount });
            
            Model.ExSearchFilter = _params.SearchFilter;
            Model.ExSearchWord = _params.SearchWord;

            return Model;
        }

        /// <summary>
        /// 멤버(유저) 상세 조회
        /// </summary>
        /// <param name="UserId">유저 고유 번호(UserNumer)</param>
        /// <returns></returns>
        public ReadMemberDetailResponse ReadMemberDetail(int UserId)
        {
            ReadMemberDetailResponse Response = new ReadMemberDetailResponse();
            MemberEditorViewModel Model = new MemberEditorViewModel();

            Console.WriteLine($"## MemberDataManager >> ReadMemberDetail(UserId = {UserId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_UserDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", UserId);

                        SqlDataReader reader = command.ExecuteReader();

                        /* 멤버(유저) 데이터 */
                        while (reader.Read())
                        {
                            Model.UserId = int.Parse(reader["UserId"]?.ToString() ?? "0");
                            Model.Id = reader["Id"]?.ToString() ?? "";
                            Model.Name = reader["Name"].ToString() ?? "";
                            Model.Image = int.Parse(reader["Image"]?.ToString() ?? "0");
                            Model.Authority = reader["Authority"]?.ToString() ?? "normal";
                            Model.PostCount = int.Parse(reader["PostCount"]?.ToString() ?? "0");
                            Model.CommentCount = int.Parse(reader["CommentCount"]?.ToString() ?? "0");
                            Model.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            Model.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01"); // TODO
                            if (reader["DeleteDate"] != DBNull.Value) { Model.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01"); }

                            Model.Password = ""; // 필요 시 SP 에서 추가
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
                Response.ErrorMessages.Add(ex.Message);
            }

            return Response;
        }

        /// <summary>
        /// 멤버(유저) 정보 수정
        /// </summary>
        /// <param name="_params"></param>
        public CommonResponse UpdateMember(UpdateMemberParams _params)
        {
            CommonResponse Response = new CommonResponse();
            Response.ResultCode = 200;
            Response.Message = "DB Success";

            Console.WriteLine($"## MemberDataManager >> UpdateMember(UserId = {_params.UserId}, Name = {_params.Name}, Image = {_params.Image}), Authority = {_params.Authority})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_UpdateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", _params.UserId);
                        command.Parameters.AddWithValue("@Name", _params.Name);
                        command.Parameters.AddWithValue("@Image", _params.Image);
                        command.Parameters.AddWithValue("@Authority", _params.Authority);

                        // 방법1) ExecuteNonQuery 함수로 영향 받은 행 수를 구하여 성공 여부 확인
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected < 1)
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail";
                        }

                        // 방법2) ExecuteReader HasRow: 영향 받은 행이 있으면 true 없으면 false
                        /*
                        SqlDataReader reader = command.ExecuteReader();
                        if (!reader.HasRows) 
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail";
                        }
                        reader.Close();
                        */
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
        /// 멤버(유저) 정보 삭제
        /// </summary>
        /// <param name="_params"></param>
        public CommonResponse DeleteMember(int UserId)
        {
            CommonResponse Response = new CommonResponse();
            Response.ResultCode = 200;
            Response.Message = "DB Success";

            Console.WriteLine($"## MemberDataManager >> DeleteMember(UserId = {UserId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_DeleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", UserId);

                        // 영향 받은 행 수를 구하여 성공 여부 확인
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected < 1)
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail (존재하지 않는 사용자 입니다.)";
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
