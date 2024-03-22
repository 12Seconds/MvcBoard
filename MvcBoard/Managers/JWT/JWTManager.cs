using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MvcBoard.Managers.JWT
{
    public class JWTManager
    {

        // 주의. Program.cs 에서도 사용해야 함, Key값 환경변수 처리 필요
        private readonly string issuer = "MusicGround";
        private readonly string audience = "MvcBoard";
        private readonly string _tempKey = "LetsGoHaul_ejaldjxjwlfakszmaekcodnj";

        /* 토큰 생성 */
        public string GenerateToken(int _UserNumber, string _LoginId)
        {
            // 클레임 생성
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, _LoginId), // TODO Name: 이름, NameId: 고유 식별자 (ID 또는 이메일 주소)

                new Claim(MvcBoardClaimTypes.UserNumber, _UserNumber.ToString()),
                new Claim(MvcBoardClaimTypes.Id, _LoginId), // TODO, _params.Id
                // new Claim(MvcBoardClaimTypes.Role, ""), Admin, User 등?
            };

            // 보안 키 생성 (TODO 시크릿 키는 임시 , 환경 변수 읽을 것)
            // var key = _configuration.GetSection("SymmetricSecurityKey").Value;
            var key = _tempKey;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 토큰 생성
            var token = new JwtSecurityToken(
                issuer: issuer, // 발급 주체 (도메인 또는 서비스명)
                audience: audience, // 발급 대상
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // TODO 임시
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /* 토큰 검증 (수동) */
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tempKey); // 여기도 시크릿 키 필요

            ClaimsPrincipal principal;

            try
            {
                // 검증 옵션 설정
                TokenValidationParameters vaildationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // 토큰 검증 시 허용되는 시간의 오차 범위 (Zero: 토큰의 발행 시간과 서버의 시간이 정확히 일치 해야 유효함)
                };

                principal = tokenHandler.ValidateToken(token, vaildationParams, out var _validatedToken);

                return principal; // TODO validatedToken 을 꼭 사용해야 되는지는 모르겠다. 아래 대로 하면 계속 타입 오류 발생
                // return (ClaimsPrincipal)(_validatedToken as JwtSecurityToken).Claims; // 오류 발생

                // bool IsAuthenticated = principal.Identity.IsAuthenticated;
            }
            catch (Exception ex) {

                // System.Exception { Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException}

                // 만료된 경우
                // ex.Expires (System.DateTime)
                // {2024-03-20 오전 10:42:27}

                // ex.Message (string)
                // IDX10223: Lifetime validation failed. The token is expired. ValidTo (UTC): '2024-03-20 오전 10:42:27', Current time (UTC): '2024-03-20 오후 12:52:47'.

                return null;
            }

            // bool IsAuthenticated = principal.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 쿠키에서 JWT 토큰을 읽어 인증
        /// </summary>
        /// <returns>
        /// bool: 인증 성공 여부
        /// ClaimsPrincipal?: Principal 객체
        /// </returns>
        public (bool, ClaimsPrincipal?) Authentication(string? cookie)
        {
            bool IsAuthenticated = false;

            if (cookie == null)
            {
                return (false, null);
            }

            ClaimsPrincipal Principal = ValidateJwtToken(cookie);

            if (Principal != null && Principal.Identity != null && Principal.Identity.IsAuthenticated)
            {
                IsAuthenticated = true;
                return (IsAuthenticated, Principal);
            }
            else
            {
                return (IsAuthenticated, null);
            }
        }

        /// <summary>
        /// 현재 인증된 토큰의 Principal 객체에서 유저 고유 번호 클레임을 읽어 반환
        /// </summary>
        /// <param name="Principal"></param>
        /// <returns>int: 유저 고유 번호 (UserId -> UserNumber 바꿀 것)</returns>
        public int GetUserNumber(ClaimsPrincipal Principal)
        {
            int userNumber = 0;
            try
            {
                userNumber = Convert.ToInt32(Principal.FindFirst(MvcBoardClaimTypes.UserNumber)?.Value);
            }
            catch (Exception ex)
            {
            }

            return userNumber;
        }

    }

    // 서비스(앱) 인증 및 비즈니스 로직에 사용할 클레임 타임 정의
    public static class MvcBoardClaimTypes
    {
        public const string UserNumber = "MvcBoard_CT_UserId";
        public const string Id = "MvcBoard_CT_Id";
        public const string Name = "MvcBoard_CT_Name";
        public const string Image = "MvcBoard_CT_Image";
    }
}
