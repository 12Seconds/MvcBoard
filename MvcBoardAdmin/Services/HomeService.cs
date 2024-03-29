using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;

namespace MvcBoardAdmin.Services
{
    public class HomeService
    {
        public HomeService() { }

        public LoginResponse Login(LoginServiceParams _params)
        {
            LoginResponse response = new LoginResponse();

            ModelStateDictionary ModelState = _params.ModelState;


            // 입력값 유효성 검증 
            if (!ModelState.IsValid)
            {
                // 아이디 검증
                if (ModelState.GetFieldValidationState("Id") == ModelValidationState.Invalid)
                {
                    // 여러 검증 옵션에 대한 에러 메시지를 모두 읽기 위한 코드 (LoginParams 주석 참고)
                    /*
                    var errorMessages = ModelState["Id"]?.Errors.Select(e => e.ErrorMessage).ToList();
                    if (errorMessages != null)
                    {
                        foreach (var errorMessage in errorMessages)
                        {
                            Console.WriteLine(errorMessage);
                            response.ErrorMessage = errorMessage; // 배열로 저장하거나, 마지막 검증 옵션에 대한 에러 문구만 출력 (유저 입장에서는 순차적 검증이 될 것)
                        }
                    }
                    */
                    response.ResultCode = 201;
                    response.Message = "입력값 오류";
                    response.ErrorMessage.Add("입력한 아이디가 올바르지 않습니다.");
                    return response;
                }

                // 비밀번호 검증
                if (ModelState.GetFieldValidationState("Password") == ModelValidationState.Invalid)
                {
                    response.ResultCode = 201;
                    response.Message = "입력값 오류";
                    response.ErrorMessage.Add("입력한 비밀번호가 올바르지 않습니다.");
                    return response;
                }

            }

            // TODO 인증 및 로그인 SP 호출
            response.Message = "로그인 성공";
            response.JwtToken = "임시 토큰";

            // 토큰 쿠키 저장 테스트 (됨)
            _params.HttpContext.Response.Cookies.Append("jwtToken", $"test010_{_params.LoginParams.Id}", new CookieOptions
            {
                Path = "/"
            });

            return response;
        }

    }
}
