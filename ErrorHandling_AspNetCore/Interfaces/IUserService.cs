using ErrorHandling_AspNetCore.Dtos;

namespace ErrorHandling_AspNetCore.Interfaces
{
    public interface IUserService
    {
        Task<ResultDto> Register(AddUserDto dto);
    }
}
