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
        public string GenerateToken(string _Id) // 로그인 Id
        {
            // 클레임 생성
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, _Id), // TODO Name: 이름, NameId: 고유 식별자 (ID 또는 이메일 주소)
                // new Claim("Role", ""), Admin, User 등?
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
                expires: DateTime.Now.AddMinutes(/* 30 */ 5), // TODO 임시
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /* 토큰 검증 (수동) */
        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tempKey); // 여기도 시크릿 키 필요

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // 토큰 검증 시 허용되는 시간의 오차 범위 (Zero: 토큰의 발행 시간과 서버의 시간이 정확히 일치 해야 유효함)
            }, out var _validatedToken);

            // bool IsAuthenticated = principal.Identity.IsAuthenticated;

            return principal; // TODO validatedToken 을 꼭 사용해야 되는지는 모르겠다. 아래 대로 하면 계속 타입 오류 발생

            // return (ClaimsPrincipal)(_validatedToken as JwtSecurityToken).Claims; // 오류 발생
        }

    }
}
