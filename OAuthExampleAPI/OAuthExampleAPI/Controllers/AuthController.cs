using Microsoft.AspNetCore.Mvc;
using OAuthExampleAPI.Services;

namespace OAuthExampleAPI.Controllers
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
            // ここでデータベース照合等のユーザー認証を行う
            // サンプルは、メールアドレスとパスワードが一致する場合に成功とします
            if (model.Email == "admin@sample.com" && model.Password == "admin")
            {
                var token = TokenService.GenerateToken(
                    _configuration["Jwt:Key"] ?? "",
                    _configuration["Jwt:Issuer"] ?? "",
                    _configuration["Jwt:Audience"] ?? "",
                    model.UserName ?? "",
                    model.Email);

                return Ok(new { token = token });
            }
            return Unauthorized();
        }

        public class UserLoginModel
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public string? Email { get; set; }
        }
    }
}
