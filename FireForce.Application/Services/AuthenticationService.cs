using BCrypt.Net;
using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;

namespace FireForce.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;


        public AuthenticationService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }


        public async Task<UserDTO?> LoginAsync(LoginDTO loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);

            if (user == null || !user.IsActive || user.IsDeleted)
                return null;

            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            await _auditLogService.LogChangeAsync("Users", "LOGIN", user.Id, "", "", user.Username);

            return MapToDTO(user);
        }


        public async Task<bool> RegisterAsync(RegisterDTO registerDto, string currentUser)
        {
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
            if (existingUser !=null )
                return false;

            
            var existingEmail = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
            if (existingEmail != null)
                return false;

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Role = registerDto.Role,
                IsActive =  true,
                CreatedBy = currentUser
            };


            var userId = await _unitOfWork.Users.AddAsync(user);

            await _auditLogService.LogChangeAsync("Users", "INSERT", userId, "",
                $"Username: {user.Username}, Role: {user.Role}", currentUser);

            return userId > 0;

        }



        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(changePasswordDto.UserId);
            if (user == null)
                return false;

            if(!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            user.UpdatedBy = user.Username;

            var result = await _unitOfWork.Users.UpdateAsync(user);


            if (result)
            {
                await _auditLogService.LogChangeAsync("Users", "UPDATE", user.Id,
                    "Password", "Password Changed", user.Username);
            }

            return result;

        }



        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? MapToDTO(user) : null;
        }


        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);
            return user != null ? MapToDTO(user) : null;
        }


        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

    }
}