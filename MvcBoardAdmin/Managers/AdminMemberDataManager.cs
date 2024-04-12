using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models.AdminMember;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Utills;
using System.Data;

namespace MvcBoardAdmin.Managers
{
    /// <summary>
    /// 관리자 계정 관련 DB 요청을 처리하는 매니저
    /// </summary>
    public class AdminMemberDataManager : DBManager
    {
        public AdminMemberDataManager(IWebHostEnvironment env) : base(env)
        {
            
        }

        /// <summary>
        /// 관리자 계정 로그인
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public LoginResult Login(LoginParams _params)
        {
            LoginResult Result = new LoginResult();

            // TODO Password 로그 제거
            Console.WriteLine($"## AdminMemberDataManager >> Login(Id = {_params.Id}), Password = {_params.Password}");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_UserLogIn", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", _params.Id);
                        command.Parameters.AddWithValue("@Password", _params.Password);

                        SqlDataReader reader = command.ExecuteReader();

                        if (!reader.Read())
                        {
                            Result.Response.ResultCode = 203;
                            Result.Response.Message = "DB Read Fail";
                            return Result;
                        }

                        int result = Convert.ToInt32(reader["Result"]);

                        Result.ResultCode = int.Parse(reader["Result"]?.ToString() ?? "0");
                        Result.UserNo = int.Parse(reader["UserNo"]?.ToString() ?? "0");
                        Result.Id = reader["Id"]?.ToString() ?? "";
                        Result.Name = reader["Name"]?.ToString() ?? "";
                        Result.Image = int.Parse(reader["Image"]?.ToString() ?? "0");
                        Result.AuthorityGroup = reader["AuthorityGroup"]?.ToString() ?? "";

                        switch (Result.ResultCode)
                        {
                            case 1:
                                Result.Response.ResultCode = 200;
                                Result.Response.Message = "DB Success";
                                break;
                            case 0:
                                Result.Response.ResultCode = 203;
                                Result.Response.Message = "DB Fail 로그인 실패 (아이디와 비밀번호를 다시 확인해 주세요.)";
                                break;
                            case -1:
                                Result.Response.ResultCode = 203;
                                Result.Response.Message = "DB Fail 입력값 오류 (아이디와 비밀번호가 올바르지 않습니다.)";
                                break;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Result.Response.ResultCode = 202;
                Result.Response.Message = "DB Error";
                Result.Response.ErrorMessages.Add(ex.Message);
            }

            return Result;
        }

        /// <summary>
        /// 관리자 계정 조회
        /// </summary>
        /// <param name="_params">검색 필터, 검색어, 페이지</param>
        /// <returns></returns>
        public AdminMemberListViewModel ReadAdmMembers(ReadMembersParams _params)
        {
            AdminMemberListViewModel Model = new AdminMemberListViewModel();

            Console.WriteLine($"## AdmMemberDataManager >> ReadAdmMembers(SearchFilter = {_params.SearchFilter}, SearchWord = {_params.SearchWord}, Page = {_params.Page})");

            int pageCount = 0;
            int totalResultCount = 0;

            List<AdmUser> AdmUsers = new List<AdmUser>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("adm_ReadAdmUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchFilter", _params.SearchFilter);
                    command.Parameters.AddWithValue("@SearchWord", _params.SearchWord);
                    command.Parameters.AddWithValue("@PageIndex", _params.Page);
                    command.Parameters.AddWithValue("@RowPerPage", 5); // 테스트용 (default: 10)

                    SqlDataReader reader = command.ExecuteReader();

                    AdmUser? admUser = null;

                    /* 멤버(유저) 데이터 */
                    while (reader.Read())
                    {
                        admUser = new AdmUser();
                        admUser.UserNo = int.Parse(reader["UserNo"]?.ToString() ?? "0");
                        admUser.Id = reader["Id"]?.ToString() ?? "";
                        admUser.Name = reader["Name"].ToString() ?? "";
                        admUser.Image = int.Parse(reader["Image"]?.ToString() ?? "0");
                        admUser.AuthorityGroup = reader["AuthorityGroup"]?.ToString() ?? "normal";
                        admUser.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                        AdmUsers.Add(admUser);
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

            Model.MemberList = AdmUsers;
            Model.PageIndex = _params.Page;
            Model.TotalRowCount = totalResultCount;
            Model.TotalPageCount = pageCount;
            Model.IndicatorRange = Utility.GetIndicatorRange(new Utility.IndicatorRangeParams { Page = _params.Page, PageCount = pageCount });

            Model.ExSearchFilter = _params.SearchFilter;
            Model.ExSearchWord = _params.SearchWord;

            return Model;
        }


        /// <summary>
        /// 관리자 계정 상세 조회
        /// </summary>
        /// <param name="UserNo">유저 고유 번호</param>
        /// <returns></returns>
        public ReadAdminMemberDetailResponse ReadAdmMemberDetail(int UserNo)
        {
            ReadAdminMemberDetailResponse Response = new ReadAdminMemberDetailResponse();
            AdminMemberEditorViewModel Model = new AdminMemberEditorViewModel();

            Console.WriteLine($"## AdmMemberDataManager >> ReadAdmMemberDetail(UserNo = {UserNo})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_AdmUserDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserNo", UserNo);

                        SqlDataReader reader = command.ExecuteReader();

                        /* 멤버(유저) 데이터 */
                        while (reader.Read())
                        {
                            Model.UserNo = int.Parse(reader["UserNo"]?.ToString() ?? "0");
                            Model.Id = reader["Id"]?.ToString() ?? "";
                            Model.Name = reader["Name"].ToString() ?? "";
                            Model.Image = int.Parse(reader["Image"]?.ToString() ?? "0");
                            Model.AuthorityGroup = reader["AuthorityGroup"]?.ToString() ?? "normal";
                            /* TODO 일반 유저와 구분하여 게시물(공지), 댓글 수 얻기 */
                            Model.PostCount = 0; // int.Parse(reader["PostCount"]?.ToString() ?? "0");
                            Model.CommentCount = 0; // int.Parse(reader["CommentCount"]?.ToString() ?? "0");
                            Model.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            Model.CreateDate = DateTime.Parse(reader["CreateDate"]?.ToString() ?? "2024/01/01");
                            if (reader["DeleteDate"] != DBNull.Value) { Model.DeleteDate = DateTime.Parse(reader["DeleteDate"]?.ToString() ?? "2024/01/01"); }
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
        /// 관리자 계정 정보 수정
        /// </summary>
        /// <param name="_params"></param>
        public CommonResponse UpdateAdmMember(UpdateMemberParams _params)
        {
            CommonResponse Response = new CommonResponse();
            Response.ResultCode = 200;
            Response.Message = "DB Success";

            Console.WriteLine($"## AdmMemberDataManager >> UpdateAdmMember(UserNo = {_params.UserId}, Name = {_params.Name}, Image = {_params.Image}), AuthorityGroup = {_params.Authority})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_UpdateAdmUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserNo", _params.UserId);
                        command.Parameters.AddWithValue("@Name", _params.Name);
                        command.Parameters.AddWithValue("@Image", _params.Image);
                        command.Parameters.AddWithValue("@AuthorityGroup", _params.Authority);

                        // 영향 받은 행 수를 구하여 성공 여부 확인
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected < 1)
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail";
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
        /// 관리자 계정 정보 삭제
        /// </summary>
        /// <param name="_params"></param>
        public CommonResponse DeleteAdmMember(int UserNo)
        {
            CommonResponse Response = new CommonResponse();
            Response.ResultCode = 200;
            Response.Message = "DB Success";

            Console.WriteLine($"## AdmMemberDataManager >> DeleteAdmMember(UserNo = {UserNo})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_DeleteAdmUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserNo", UserNo);

                        // 영향 받은 행 수를 구하여 성공 여부 확인
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected < 1)
                        {
                            Response.ResultCode = 203;
                            Response.Message = "DB Fail (존재하지 않는 관리자 입니다.)";
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
