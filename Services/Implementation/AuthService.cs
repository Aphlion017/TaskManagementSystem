using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Models.DTOs;
using TaskManagementSystem.Interface;


namespace TaskManagementSystem.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtHelper _jwtHelper;

        public AuthService(UserManager<ApplicationUser> userManager, JwtHelper jwtHelper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "UserManager Cannot be Null.");
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper), "JwtHelper Cannot be Null.");
        }

        public async Task<AuthResponseDTO> RegisterUserAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser { Name = request.Name, Email = request.Email, UserName = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded) throw new Exception(string.Join(";", result.Errors.Select(e => e.Description)));
            return _jwtHelper.GenerateToken(user);

        }
        public async Task<AuthResponseDTO> LoginUserAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException("Invalid Credentials.");
            return _jwtHelper.GenerateToken(user);
        }
    }

}