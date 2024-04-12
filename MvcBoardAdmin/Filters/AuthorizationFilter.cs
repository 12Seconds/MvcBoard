using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MvcBoardAdmin.Managers;
using MvcBoardAdmin.Managers.JWT;
using MvcBoardAdmin.Managers.Results;

namespace MvcBoardAdmin.Filters
{
    /// <summary>
    /// 권한 필터 (승인/인가)
    /// </summary>
    public class AuthorizationFilter : ActionFilterAttribute, IAuthorizationFilter /* IActionFilter */
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var Controller = filterContext.ActionDescriptor.RouteValues["controller"];
            var Action = filterContext.ActionDescriptor.RouteValues["action"];
            string AuthGroupId;

            // 1. 요청으로부터 Cookie 의 JWT 토큰을 읽어서 인증
            AuthenticationResult Result = JWTManager.Authentication(filterContext.HttpContext.Request.Cookies["jwtToken"]);

            Console.WriteLine($">> [AuthorizationFilter] OnAuthorization() Requested From (Controller: {Controller}, Action: {Action}) Result.IsAuthenticated: {Result.IsAuthenticated}, UserName: {JWTManager.GetUserName(Result.Principal)}");

            // 인증 실패
            if (!Result.IsAuthenticated || Result.Principal == null)
            {
                filterContext.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            // 2. DB 권한 테이블 조회해서 비교하고 권한이 있는지 확인
            AuthGroupId = JWTManager.GetAuthGroupId(Result.Principal!);
            ReadAuthListResult AuthListResult = AuthorityDataManager.ReadAuthListByGroupId(AuthGroupId);

            // 권한 정보 조회 오류
            if (AuthListResult.Response.ResultCode != 200)
            {
                // TODO 오류 문구 노출
                filterContext.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }
            
            bool Authorized = false;
            foreach (var auth in AuthListResult.AuthList)
            {
                if (auth.Controller == Controller)
                {
                    Authorized = true;
                    break;
                }
            }

            // 권한 없음
            if (!Authorized)
            {
                // TODO 권한 없음 문구 노출
                filterContext.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            Console.WriteLine($">> [AuthorizationFilter] OnAuthorization() Authorized: {Authorized} Requested From (Controller: {Controller}, Action: {Action}) Result.IsAuthenticated: {Result.IsAuthenticated}, UserName: {JWTManager.GetUserName(Result.Principal)} (AuthGroupId: {AuthGroupId})");
        }
    }
}
