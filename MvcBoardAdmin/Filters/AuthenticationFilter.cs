using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MvcBoardAdmin.Managers.JWT;
using System.Text;

namespace MvcBoardAdmin.Filters
{
    /// <summary>
    /// 인증 필터 (로그인 여부 확인)
    /// </summary>
    public class AuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter /* IActionFilter */
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var Controller = filterContext.ActionDescriptor.RouteValues["controller"];
            var Action = filterContext.ActionDescriptor.RouteValues["action"];

            // 요청으로부터 Cookie 의 JWT 토큰을 읽어서 인증
            AuthenticationResult Result = JWTManager.Authentication(filterContext.HttpContext.Request.Cookies["jwtToken"]);

            Console.WriteLine($">> [AuthenticationFilter] OnAuthorization() Requested From (Controller: {Controller}, Action: {Action}) Result.IsAuthenticated: {Result.IsAuthenticated}, UserName: {JWTManager.GetUserName(Result.Principal)}");

            // 인증 실패
            if (!Result.IsAuthenticated || Result.Principal == null)
            {
                // 방법1. O
                filterContext.Result = new RedirectToActionResult("Index", "Home", null);

                // 방법2. O but -> Home/Logout 접근 시 안됨, 한글 깨짐
                // <script>alert('로그인이 필요한 서비스입니다.'); history.back()</script>
                // filterContext.Result = new JavaScriptResult("<script>document.addEventListener('DOMContentLoaded', function (){ alert('This service requires login.'); history.back();});</script>");
                // filterContext.Result = new JavaScriptResult("document.addEventListener('DOMContentLoaded', function (){ alert('This service requires login.'); history.back();});");

                // 방법3. 2번과 사실 동일함
                /*
                var sb = new StringBuilder();
                sb.Append("<script>document.addEventListener('DOMContentLoaded', function (){");
                sb.Append("alert('로그인이 필요한 서비스입니다.'); history.back();");
                sb.Append("alert('This service requires login.'); history.back();");
                sb.Append("});</script>");
                filterContext.Result = new ContentResult { Content = sb.ToString(), ContentType = "text/html" };
                */

                // TODO 로그 테이블 만들어서 DB에 로그 적재
                /*
                ActionLog log = new ActionLog() // 여기서 ActionLog 는 테이블임
                {
                    Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
                    IP = filterContext.HttpContext.Request.UserHostAddress,
                    DateTime = filterContext.HttpContext.Timestamp
                };
                */
            }
        }

        // TODO 지울 것
        public class JavaScriptResult : ContentResult
        {
            public JavaScriptResult(string script)
            {
                this.Content = script;
                this.ContentType = "text/html";
                // this.ContentType = "application/javascript"; // application/javascript 가 오히려 안되네
            }
        }
    }
}
