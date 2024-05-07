using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.JWT;
using MvcBoardAdmin.Managers.Results;
using MvcBoardAdmin.Models.Home;
using MvcBoardAdmin.Utills;

namespace MvcBoardAdmin.Services
{
    public class HomeService
    {
        private readonly AdminMemberDataManager _adminMemberDataManager;
        public HomeService(AdminMemberDataManager adminMemberDataManager)
        {
            _adminMemberDataManager = adminMemberDataManager;
        }

        /// <summary>
        /// 로그인 여부 확인하여 Home/Index 의 ViewModel 반환
        /// </summary>
        /// <returns></returns>
        public HomeViewModel GetHomeViewModel(HttpContext _context)
        {
            HomeViewModel Model = new HomeViewModel();

            AuthenticationResult Result = JWTManager.Authentication(_context.Request.Cookies["jwtToken_admin"]);

            if (Result.IsAuthenticated && Result.Principal != null)
            {
                Model.IsLogined = true;
                Model.UserName = JWTManager.GetUserName(Result.Principal);
            }

            return Model;
        }

        /// <summary>
        /// 관리자 계정 로그인
        /// </summary>
        /// <param name="_params"></param>
        /// <returns></returns>
        public CommonResponse Login(LoginServiceParams _params)
        {
            // 입력값 유효성 검증
            CommonResponse Response = Utility.ModelStateValidation(_params.ModelState);

            if (Response.ResultCode != 200)
            {
                return Response;
            }

            // 로그인 요청
            LoginResult Result = _adminMemberDataManager.Login(_params.LoginParams);

            // 로그인 성공
            if (Result.ResultCode == 1)
            {
                // 토큰 발급
                string token = JWTManager.GenerateToken(Result);

                // 요청 브라우저의 쿠키에 토큰 저장
                _params.HttpContext.Response.Cookies.Append("jwtToken_admin", token, new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true
                });
                // document.cookie = `jwtToken_admin=${data.token}; path=/`;  // 추가 속성 `jwtToken_admin=${data.token}; path=/; HttpOnly; Secure`;
            }

            return Result.Response;
        }

        /// <summary>
        /// 관리자 계정 로그아웃
        /// </summary>
        public void Logout(HttpContext _context)
        {
            _context.Response.Cookies.Delete("jwtToken_admin");
        }

    }
}
