using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JWTExampleAPI.Models;
using JWTExampleAPI.Services;

namespace JWTExampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            bool isConfigurationMissing = _configuration["Jwt:Key"].IsNullOrEmpty() || _configuration["Jwt:Issuer"].IsNullOrEmpty() || _configuration["Jwt:Audience"].IsNullOrEmpty() || model.UserName.IsNullOrEmpty();
            if (isConfigurationMissing)
            {
                throw new InvalidOperationException("JWT configuration is incomplete.");
            }
            // ここでデータベース照合等のユーザー認証を行う
            // サンプルは、メールアドレスとパスワードが一致する場合に成功とします
            if (model.Email != "admin@sample.com" || model.Password != "admin")
            {
                return Unauthorized();
            }
            var token = JWTService.GenerateToken(
                key: _configuration["Jwt:Key"]!,
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                userName: model.UserName!,
                email: model.Email
            );
            return Ok(new { token = token });
        }
    }
}
