using LoginSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using LoginSystem.Models.Dtos.LoginDtos;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly LoginDomain _loginDomain;

        public LoginController(LoginDomain loginDomain)
        {
            _loginDomain = loginDomain;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddLogin(AddLoginDto login)
        {
            if(await _loginDomain.IsValidLoginCreation(login))
            {
                try
                {
                    ReadLoginDto newLogin = await _loginDomain.AddLogin(login);
                    return Ok(newLogin);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"{ex.Message}");
                }
            }
            else
            {
                return BadRequest("Invalid login creation");
            }
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto updateEmailDto)
        {
            try
            {
                var loginId = User.FindFirst("id")?.Value;
                
                if (! await _loginDomain.ExistsEmail(updateEmailDto.Email))
                {
                    return BadRequest("Email already exists");
                }

                var updatedLogin = await _loginDomain.UpdateEmail(loginId!, updateEmailDto);
                return Ok(updatedLogin);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var loginId = User.FindFirst("id")?.Value;
                var updatedLogin = await _loginDomain.UpdatePassword(loginId!, updatePasswordDto);
                return Ok(updatedLogin);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpDelete("delete/{loginId}")]
        public async Task<IActionResult> DeleteLogin(string loginId)
        {
            try
            {
                var success = await _loginDomain.DeleteLogin(loginId);
                if (success)
                    return NoContent();
                else
                    return NotFound($"{loginId} not found");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogins()
        {
            try
            {
                var allLogins = await _loginDomain.GetAllLogins();
                return Ok(allLogins);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("by-id/{loginId}")]
        public async Task<IActionResult> GetLoginById(string loginId)
        {
            try
            {
                var login = await _loginDomain.GetLoginById(loginId);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetLoginByEmail(string email)
        {
            try
            {
                var login = await _loginDomain.GetLoginByEmail(email);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }
    }
}