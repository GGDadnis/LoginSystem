using LoginSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LoginSystem.Models.Dtos.UserProfileDtos;

namespace LoginSystem.Controllers
{
    [ApiController]
    [Route("api/userprofiles")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileDomain _userProfileDomain;

        public UserProfileController(UserProfileDomain userProfileDomain)
        {
            _userProfileDomain = userProfileDomain;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserProfile([FromBody] AddUserProfileDto userProfile)
        {
            try
            {
                var loginId = User.FindFirst("id")?.Value;
                var addedProfile = await _userProfileDomain.AddUserProfile(loginId!, userProfile);
                return Ok(addedProfile);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto updateUserProfile)
        {
            try
            {
                var loginId = User.FindFirst("id")?.Value;
                var updatedProfile = await _userProfileDomain.UpdateUserProfile(loginId!, updateUserProfile);
                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpDelete("{profileId}")]
        public async Task<IActionResult> DeleteUserProfile(string profileId)
        {
            try
            {
                var deleted = await _userProfileDomain.DeleteUserProfile(profileId);
                if (deleted)
                {
                    return Ok($"User profile {profileId} deleted successfully");
                }
                return NotFound($"{profileId} not found");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProfiles()
        {
            try{
            var allProfiles = await _userProfileDomain.GetAllProfiles();
            return Ok(allProfiles);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("by-id/{profileId}")]
        public async Task<IActionResult> GetProfileById(string profileId)
        {
            try
            {
                var profile = await _userProfileDomain.GetProfileById(profileId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetProfileByEmail(string email)
        {
            try
            {
                var profile = await _userProfileDomain.GetProfileByEmail(email);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }
    }
}