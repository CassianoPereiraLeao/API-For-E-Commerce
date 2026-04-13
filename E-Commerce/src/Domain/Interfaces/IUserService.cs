using Project.src.DTOs;
using Project.src.Routes.Request.Create;
using Project.src.Routes.Request.Login;
using Project.src.Routes.Request.Update;
using Project.src.Routes.Response;

namespace Project.src.Domain.Interfaces;

public interface IUserService
{
    Task<ApiResponse<List<UserDTO>>> GetAllUser(int? page, int? limit);
    Task<ApiResponse<UserDTO>> GetUserById(Guid id);
    Task<ApiResponse<UserDTO>> CreateUser(UserRequestCreate userRequestCreate);
    Task<ApiResponse<UserLoginDTO>> LoginUser(UserRequestLogin login);
    Task<ApiResponse<UpdateUserDTO>> UpdateUser(UserRequestUpdate userRequestUpdate);
    Task<ApiResponse<UserDTO>> DeleteUser(Guid id);
    Task RefreshTokenUser(string token);
}