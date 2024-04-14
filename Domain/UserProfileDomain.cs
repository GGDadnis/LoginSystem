using AutoMapper;
using LoginSystem.Data;
using LoginSystem.Models;
using Microsoft.EntityFrameworkCore;
using LoginSystem.Models.Dtos.UserProfileDtos;

namespace LoginSystem.Domain
{
    public class UserProfileDomain
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserProfileDomain(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadUserProfileDto> AddUserProfile(string loginId, AddUserProfileDto userProfile)
        {
            try
            {
                var existingLogin = await _context.Logins.FindAsync(loginId);

                if(existingLogin != null)
                {
                    UserProfile newProfile = new UserProfile
                    {
                        LoginId = loginId,
                        Login = existingLogin,
                        FirstName = userProfile.FirstName,
                        LastName = userProfile.LastName,
                        Photo = userProfile.Photo
                    };

                    _context.Add(newProfile);
                    await _context.SaveChangesAsync();

                    ReadUserProfileDto readProfile = _mapper.Map<ReadUserProfileDto>(newProfile)!;
                    
                    return readProfile;
                }
                
                throw new InvalidOperationException($"{loginId} not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user profile: {ex.Message}");
                throw;
            }
        }

        public async Task<ReadUserProfileDto> UpdateUserProfile(string loginId, UpdateUserProfileDto updateUserProfile)
        {
            Login? existingLogin = await _context.Logins
                .Include(login => login.UserProfile)
                .FirstOrDefaultAsync(login => login.Id == loginId);

            if (existingLogin != null && existingLogin.UserProfile != null)
            {
                try
                {
                    _mapper.Map(updateUserProfile, existingLogin.UserProfile);
                    await _context.SaveChangesAsync();
                    ReadUserProfileDto? updatedProfile = _mapper.Map<ReadUserProfileDto>(existingLogin.UserProfile);

                    return updatedProfile!;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating user profile: {ex.Message}");
                    throw;
                }
            }

            throw new InvalidOperationException($"{loginId} or associated UserProfile not found");
        }

        public async Task<bool> DeleteUserProfile(string userProfile)
        {
            UserProfile? existingProfile = await _context.UserProfiles.FindAsync(userProfile);

            if (existingProfile != null)
            {
                try
                {
                    _context.UserProfiles.Remove(existingProfile);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting login: {ex.Message}");
                    throw;
                }
            }

            return false;
        }

        public async Task<IEnumerable<ReadUserProfileDto>> GetAllProfiles()
        {
            List<UserProfile> allProfiles = await _context.UserProfiles.ToListAsync();
            List<ReadUserProfileDto>? allProfilesDto = _mapper.Map<List<ReadUserProfileDto>>(allProfiles);

            return allProfilesDto!;
        }

        public async Task<ReadUserProfileDto> GetProfileById(string profileId)
        {
            UserProfile existingProfile = await _context.UserProfiles.FindAsync(profileId) ?? 
                        throw new InvalidOperationException($"{profileId} not found");

            return _mapper.Map<ReadUserProfileDto>(existingProfile)!;
        }

        public async Task<ReadUserProfileDto> GetProfileByEmail(string email)
        {
            UserProfile existingProfile = await _context.UserProfiles
                .Include(profile => profile.Login)
                .FirstOrDefaultAsync(profile => profile.Login.Email == email) ?? 
                            throw new InvalidOperationException($"{email} not found");

            return _mapper.Map<ReadUserProfileDto>(existingProfile)!;
        }
    }
}