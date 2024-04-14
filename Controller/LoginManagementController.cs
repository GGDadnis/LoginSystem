using System.Text;
using LoginSystem.Data;
using LoginSystem.Models;
using LoginSystem.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LoginSystem.Models.Dtos.LoginDtos;

namespace LoginSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginManagementController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SaltPassword _salt;
        private readonly IConfiguration _configuration;


        public LoginManagementController(AppDbContext context, SaltPassword salt, IConfiguration configuration)
        {
            _salt = new SaltPassword();
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUsuario([FromBody] AddLoginDto doinglogin)
        {
            Login? login = await _context.Logins.FirstOrDefaultAsync(login => login.Email == doinglogin.Email);

            if (login != null && await _salt.HashingPassword(doinglogin.PasswordHash, login.Salt) == login.PasswordHash)
            {
                var tokenString = TokenBear(login);
                return Ok(new { token = tokenString });
            }

            return Unauthorized("Invalid email or password");
        }

        private string TokenBear(Login login)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var identity = Claims(login);

            var token =
                new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    signingCredentials: credentials,
                    claims: identity,
                    expires: DateTime.UtcNow.AddHours(6)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] Claims(Login login)
        {
            return new[] {
                        new Claim("id", login.Id),
                        new Claim("username", login.Email),
                    };
        }
    }
}