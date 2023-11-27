using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            if (credential.UserName == "admin" && credential.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("Department", "HR"),
                    new Claim("Role", "HRManager"),
                    new Claim("EmploymentDate", "2023-08-01")
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new 
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt
                });
            }
            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint");
            return Unauthorized(ModelState);
        }
        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

            var token = new JwtSecurityToken(
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expireAt,
                    signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(secretKey),
                            SecurityAlgorithms.HmacSha256
                        )
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class Credential
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
