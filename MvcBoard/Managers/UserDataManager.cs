
using Microsoft.Data.SqlClient;
using MvcBoard.Controllers.Models;
using MvcBoard.Managers.Models;
using System.Data;

namespace MvcBoard.Managers
{
    public class UserDataManager : DBManager
    {
        public UserDataManager(IWebHostEnvironment env) : base(env)
        {
            Console.WriteLine("## UserDataManager() initialized...");
        }

        // 로그인
        public LogInResultParams Login(LoginParams _params)
        {
            // Password 를 로그 찍어도 되나
            Console.WriteLine($"## UserDataManager >> Login(Id = {_params.Id}, Password = {_params.Password})");

            LogInResultParams Result = new LogInResultParams();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("LogIn", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", _params.Id);
                    command.Parameters.AddWithValue("@Password", _params.Password);

                    // TODO 타입 지정
                    // command.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = _params.Id;
                    // command.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = _params.Password;

                   Result.ResultCode = (int)command.ExecuteScalar();

                    Result.ResultMsg = Result.ResultCode switch
                    {
                        0 => "로그인 실패",
                        1 => "로그인 성공",
                        -1 or _ => "입력값 오류"
                    };
                }
                connection.Close();
            }
            return Result;
        }

        // 로그아웃
        public void Logout()
        {

        }

        // 회원 가입
        public void SignUp(SignUpParams _params)
        {
            // Password 를 로그 찍어도 되나
            Console.WriteLine($"## UserDataManager >> SignUp(Id = {_params.Id}, Password = {_params.Password}), Name = {_params.Name})");

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", _params.Id);
                    command.Parameters.AddWithValue("@Password", _params.Password);
                    command.Parameters.AddWithValue("@Name", _params.Name);
                    command.Parameters.AddWithValue("@Image", _params.Image);

                    SqlDataReader reader = command.ExecuteReader(); // TODO SP 내부에서 처리하고 결과 알려줘야 함
                    reader.Close();
                }
                connection.Close();
            }
        }

        // 회원 정보 조회
        public void GetUserInfo()
        {

        }
    }
}
