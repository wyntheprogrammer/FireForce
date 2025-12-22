using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserDTO?> LoginAsync(LoginDTO loginDto);
        Task<bool> RegisterAsync(RegisterDTO registerDto, string currentUser);
        Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDto);
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO?> GetUserByUsernameAsync(string username);
    }
}