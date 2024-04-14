using AutoMapper;
using LoginSystem.Data;
using LoginSystem.Models;
using LoginSystem.Security;
using Microsoft.EntityFrameworkCore;
using LoginSystem.Models.Dtos.LoginDtos;

namespace LoginSystem.Domain
{
    public class LoginDomain
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly SaltPassword _salt;


        public LoginDomain(IMapper mapper, AppDbContext context, SaltPassword salt)
        {
            _mapper = mapper;
            _context = context;
            _salt = salt;
        }

        public async Task<ReadLoginDto> AddLogin(AddLoginDto login)
        {
            try
            {
                string salting = _salt.CreatingSalt();
                string hashPassword = await _salt.HashingPassword(login.PasswordHash, salting);

                Login newLogin = new Login
                {
                    Email = login.Email,
                    PasswordHash = hashPassword,
                    Salt = salting
                };

                _context.Add(newLogin);
                _context.SaveChanges();

                ReadLoginDto? loginRead = _mapper.Map<ReadLoginDto>(newLogin);

                return loginRead!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating login: {ex.Message}");
                throw;
            }
        }

        public async Task<ReadLoginDto> UpdateEmail(string loginId, UpdateEmailDto updateEmail)
        {
            Login? existingLogin = await _context.Logins.FindAsync(loginId);

            if (existingLogin != null)
            {
                try
                {
                    _mapper.Map(updateEmail, existingLogin);

                    await _context.SaveChangesAsync();

                    ReadLoginDto? updatedLogin = _mapper.Map<ReadLoginDto>(existingLogin);

                    return updatedLogin!;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating email: {ex.Message}");
                    throw;
                }
            }

            throw new InvalidOperationException($"{loginId} not found");
        }

        public async Task<ReadLoginDto> UpdatePassword(string loginId, UpdatePasswordDto updatePassword)
        {
            Login? existingLogin = _context.Logins.Find(loginId);

            if(existingLogin != null)
            {
                try
                {
                    string salting = _salt.CreatingSalt();
                    string hashPassword = await _salt.HashingPassword(updatePassword.PasswordHash, salting);

                    existingLogin.PasswordHash = hashPassword;
                    existingLogin.Salt = salting;

                    _context.SaveChanges();

                    ReadLoginDto? updatedLogin = _mapper.Map<ReadLoginDto>(existingLogin);

                    return updatedLogin!;
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"Error changing password: {ex.Message}");
                    throw;
                }
            }
            
            throw new InvalidOperationException($"{loginId} not found");
        }

        public async Task<bool> DeleteLogin(string loginId)
        {
            Login? existingLogin = await _context.Logins.FindAsync(loginId);

            if (existingLogin != null)
            {
                try
                {
                    _context.Logins.Remove(existingLogin);
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

        public async Task<IEnumerable<ReadLoginDto>> GetAllLogins()
        {
            List<Login> allLogins = await _context.Logins.ToListAsync();
            List<ReadLoginDto>? allLoginsDto = _mapper.Map<List<ReadLoginDto>>(allLogins);

            return allLoginsDto!;
        }


        public async Task<ReadLoginDto> GetLoginById(string loginId)
        {
            Login existingLogin = await _context.Logins.FirstOrDefaultAsync(login => login.Id == loginId) ??
                                throw new InvalidOperationException($"{loginId} not found");

            return _mapper.Map<ReadLoginDto>(existingLogin)!;
        }

        public async Task<ReadLoginDto> GetLoginByEmail(string email)
        {
            Login existingLogin = await _context.Logins.FirstOrDefaultAsync(login => login.Email == email) ??
                                throw new InvalidOperationException($"{email} not found");

            return _mapper.Map<ReadLoginDto>(existingLogin)!;
        }

        public async Task<bool> IsValidLoginCreation(AddLoginDto newLogin)
        {
            if (await ExistsEmail(newLogin.Email))
            {
                return false;
            }

            //room for more validations

            return true;
        }

        public async Task<bool> ExistsEmail(string email)
        {
            Login? existingLogin = await _context.Logins.FirstOrDefaultAsync(login => login.Email == email);

            return existingLogin != null;
        }
    }
}