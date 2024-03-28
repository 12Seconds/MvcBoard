using Microsoft.Data.SqlClient;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Models;
using MvcBoardAdmin.Models.Member;
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

            Console.WriteLine($"## MemberDataManager >> ReadMembers(SearchFilter = {_params.SearchFilter}, SearchWord = {_params.SearchWord})");

            int pageCount = 0;
            int totalResultCount = 0;

            List<User> Users = new List<User>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("adm_ReadUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchFilter", _params.SearchFilter);
                    command.Parameters.AddWithValue("@SearchWord", _params.SearchWord);
                    command.Parameters.AddWithValue("@PageIndex", _params.Page);
                    command.Parameters.AddWithValue("@RowPerPage", 10); // 테스트용 (default: 10)

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
            Model.PageCount = pageCount;
            Model.PageIndex = _params.Page;
            Model.RowPerPage = 10; // 테스트용 (default: 10)
            Model.TotalRowCount = totalResultCount;

            return Model;
        }


    }
}
