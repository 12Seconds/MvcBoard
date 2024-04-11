using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Managers.Results;
using System.Data;

namespace MvcBoardAdmin.Managers
{
    /// <summary>
    /// 관리자 계정 관련 DB 요청을 처리하는 매니저
    /// </summary>
    public class AdmUserDataManager : DBManager
    {
        public AdmUserDataManager(IWebHostEnvironment env) : base(env)
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
            Console.WriteLine($"## PostDataManager >> Login(Id = {_params.Id}), Password = {_params.Password}");

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

    }
}
