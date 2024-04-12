using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models;
using System.Data;

namespace MvcBoardAdmin.Managers
{

    // 1. 데이터를 반환하는 DB 요청인 경우(SELECT, ..) : Result 반환 (CommonResponse 와 데이터를 포함하는 Result Class 정의)
    // 2. 처리 결과를 반환하는 DB 요청인 경우(UPDATE, DELETE, ..) : CommonResponse 반환

    /// <summary>
    /// 권한 관련 DB 요청을 처리하는 매니저
    /// </summary>
    public class AuthorityDataManager : DBManager
    {
        public AuthorityDataManager(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## AuthorityDataManager() initialized...");
        }

        /// <summary>
        /// 권한 그룹명으로 권한 리스트 정보 조회
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public static ReadAuthListResult ReadAuthListByGroupId(string authGroupId)
        {
            ReadAuthListResult Result = new ReadAuthListResult();

            List<AuthDetail> AuthList = new List<AuthDetail>();

            Console.WriteLine($"## AuthorityDataManager >> ReadAuthListByGroupId(authGroupId = {authGroupId})");

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("adm_ReadAuthListByGroupId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AuthGroupId", authGroupId);

                        SqlDataReader reader = command.ExecuteReader();

                        AuthDetail? auth = null;

                        /* 권한 데이터 */
                        while (reader.Read())
                        {
                            auth = new AuthDetail();

                            auth.AuthGroupId = reader["AuthGroupId"]?.ToString() ?? "";
                            // Join 테이블 데이터
                            auth.AuthId = reader["AuthId"]?.ToString() ?? "";
                            auth.AuthName = reader["AuthName"]?.ToString() ?? "";
                            auth.Controller = reader["Controller"]?.ToString() ?? "";

                            AuthList.Add(auth);
                        }

                        reader.Close();
                    }
                    connection.Close();
                }

                Result.Response.ResultCode = 200;
                Result.Response.Message = "DB Success";
                Result.AuthList = AuthList;
            }
            catch (Exception ex)
            {
                Result.Response.ResultCode = 202;
                Result.Response.Message = "DB Error";
                Result.Response.ErrorMessages.Add(ex.Message);
            }

            return Result;
        }

        // TODO 권한 그룹 리스트 조회 (관리자 계정 관리 페이지 에디터 드롭다운에 필요)

    }
}
