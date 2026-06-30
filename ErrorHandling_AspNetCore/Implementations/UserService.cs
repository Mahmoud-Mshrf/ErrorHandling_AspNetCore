using ErrorHandling_AspNetCore.Data;
using ErrorHandling_AspNetCore.Dtos;
using ErrorHandling_AspNetCore.Interfaces;
using ErrorHandling_AspNetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling_AspNetCore.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        public UserService(AppDbContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<ResultDto> Register(AddUserDto dto)
        {
            bool existedBefore = await _context.Users.AnyAsync(x => x.Email == dto.Email);
            if (existedBefore)
            {
                return new ResultDto { Errors = [$"User with Email : {dto.Email} is already existed "] };
            }
            var user = new User { Email = dto.Email, FullName = dto.FullName, PasswordHash = _hasher.HashPassword(null, dto.Password) };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = $"User added successfully with id : {user.Id}" };
        }
    }
}
