using Microsoft.Data.SqlClient;

namespace MvcBoardAdmin.Managers
{
    public class DBManager
    {
        private readonly IWebHostEnvironment _env;

        private static string SERVER = "DESKTOP-5AFMG8G";
        private static string DATABASE = "MVC_BOARD_DB";
        private string CONNECTION_STRING = "";

        private SqlConnection? connection { get; set; }
        public DBManager(IWebHostEnvironment env/*, IConfiguration config*/) {

            _env = env;

            // config.GetConnectionString(); TODO 작업중

            try
            {
                _env = env ?? throw new ArgumentNullException(nameof(env));

                Console.WriteLine($"## DBManager() _env.IsDevelopment(): {_env.IsDevelopment()}, _env.IsProduction(): {_env.IsProduction()}");

                if (_env.IsProduction())
                {
                    Console.WriteLine($"## IsProduction @@@@@@@@ "); // TODO 제거

                    // TODO 수정 필요
                    CONNECTION_STRING = $"Server={SERVER};Database={DATABASE};Trusted_Connection=true;TrustServerCertificate=True";
                }
                else if (_env.IsDevelopment())
                {
                    Console.WriteLine($"## IsDevelopment @@@@@@@@ "); // TODO 제거

                    CONNECTION_STRING = $"Server={SERVER};Database={DATABASE};Trusted_Connection=true;TrustServerCertificate=True";
                }
                /*
                else
                {
                }
                */
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"## DBManager() constructor env exception !!!");
                Console.WriteLine(ex.Message);

                // TODO 로깅을 통해 예외 기록
                // Logger.LogError(ex, "ArgumentNullException occurred in YourCustomClass constructor");

                // 상위 호출자에게 예외 전달
                throw;
            }
        }

        public SqlConnection GetConnection() {
            if (connection != null)
            {
                return connection; // TODO 할당을 하는 곳이 없어서 사실상 재활용을 한번도 안하는데 의미가 있나
            }
            else
            {
                return new SqlConnection(CONNECTION_STRING);
            }
        }

    }
}
