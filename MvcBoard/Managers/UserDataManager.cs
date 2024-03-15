
using Microsoft.Data.SqlClient;
using MvcBoard.Controllers.Models;
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
        public void Login()
        {

        }

        // 로그아웃
        public void Logout()
        {

        }

        // 회원 가입
        public void SignUp(SignUpParams _params)
        {
            // Password 를 로그 찍어도 되나
            Console.WriteLine($"## UserDataManager >> SignUp(UserId = {_params.Id}, Password = {_params.Password}), Name = {_params.Name})");

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
