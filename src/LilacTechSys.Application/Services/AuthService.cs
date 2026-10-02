using System;
using System.Linq;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using LilacTechSys.Application.DTOs;
using LilacTechSys.Application.Interfaces;
using LilacTechSys.Domain.Entities;

namespace LilacTechSys.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<AdminUser> _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IRepository<AdminUser> userRepo, IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepo = userRepo;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var users = await _userRepo.FindAsync(u => (u.Username == request.Username || u.Email == request.Username) && u.IsActive);
            var user = users.FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ApiResponse<LoginResponse>.Fail("Invalid credentials provided.");
            }

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Username, user.Email, user.Role.ToString());
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.LastLoginAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = new AdminUserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    LastLoginAt = user.LastLoginAt
                }
            };

            return ApiResponse<LoginResponse>.Ok(response, "Login successful.");
        }

        public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var users = await _userRepo.FindAsync(u => u.RefreshToken == request.RefreshToken && u.IsActive);
            var user = users.FirstOrDefault();

            if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return ApiResponse<LoginResponse>.Fail("Invalid or expired refresh token.");
            }

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.Username, user.Email, user.Role.ToString());
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            var response = new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = new AdminUserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    LastLoginAt = user.LastLoginAt
                }
            };

            return ApiResponse<LoginResponse>.Ok(response, "Token refreshed successfully.");
        }

        public async Task<ApiResponse<AdminUserDto>> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<AdminUserDto>.Fail("User not found.");

            return ApiResponse<AdminUserDto>.Ok(new AdminUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                LastLoginAt = user.LastLoginAt
            });
        }
    }
}
