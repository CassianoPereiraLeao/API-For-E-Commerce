using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Project.src.Domain.Interfaces;
using Project.src.DTOs;
using Project.src.Infra.Entities;
using Project.src.Infra.Repository.Interfaces;
using Project.src.Middlewares;
using Project.src.Routes.Request.Create;
using Project.src.Routes.Request.Login;
using Project.src.Routes.Request.Update;
using Project.src.Routes.Response;
using Project.src.Routes.Validations;

namespace Project.src.Domain.Services;

public class UserService : IUserService
{
    private readonly IValidator<UserRequestCreate> _validator_create;
    private readonly IValidator<UserRequestLogin> _validator_login;
    private readonly IUserRepository _repository;
    private readonly JwtService _token;
    public UserService(IValidator<UserRequestLogin> validator_login, 
    IValidator<UserRequestCreate> validator_create, IUserRepository repository, JwtService token)
    {
        _validator_create = validator_create;
        _repository = repository;
        _token = token;
        _validator_login = validator_login;
    }

    public async Task<ApiResponse<UserDTO>> CreateUser(UserRequestCreate userRequestCreate)
    {
        var valid = await _validator_create.ValidateAsync(userRequestCreate);
        if (!valid.IsValid)
        {
            return new ApiResponse<UserDTO>
            {
                Status = "invalid",
                Message = "Validation Error",
                Errors = valid.ErrorsHandle()
            };
        }

        User user = new(userRequestCreate.Name, userRequestCreate.Email, userRequestCreate.Password, userRequestCreate.Role);

        var createUser = await _repository.CreateUser(user);

        if(!createUser)
        {
            return new ApiResponse<UserDTO>
            {
                Status = "error",
                Message = "Email is already registered."
            };
        }

        return new ApiResponse<UserDTO>
        {
            Status = "success",
            Message = "User created.",
            Data = null
        };
    }

    public async Task<ApiResponse<UserDTO>> DeleteUser(Guid id)
    {
        bool deleted = await _repository.DeleteUser(id);

        if(!deleted)
            return new ApiResponse<UserDTO>
            {
                Status = "error",
                Message = "User not found."
            };

        return new ApiResponse<UserDTO>
        {
            Status = "Sucess",
            Message = "User deleted successfully."
        };
    }

    public async Task<ApiResponse<List<UserDTO>>> GetAllUser(int? page, int? limit)
    {
        var page_view = page ?? 1;
        var limit_view = limit ?? 10;

        List<UserDTO> user = await _repository.GetAllUsers(page_view, limit_view);

        return new ApiResponse<List<UserDTO>>
        {
            Status = "sucess",
            Message = "Users retrieved successfully.",
            Data = user
        };
    }

    public async Task<ApiResponse<UserDTO>> GetUserById(Guid id)
    {
        var user = await _repository.GetUserById(id);

        if(user == null)
            return new ApiResponse<UserDTO>
            {
                Status = "error",
                Message = "User not found.",

            };

        return new ApiResponse<UserDTO>
        {
            Status = "success",
            Message = "User retrieved successfully.",
            Data = user
        };
    }

    public async Task<ApiResponse<UserLoginDTO>> LoginUser(UserRequestLogin login)
    {
        var valid = await _validator_login.ValidateAsync(login);
        if(!valid.IsValid)
            return new ApiResponse<UserLoginDTO>
            {
                Status = "invalid",
                Message = "Validation Error",
                Errors = valid.ErrorsHandle()
            };

        var user = await _repository.LoginUser(login.Email, login.Password);

        if(user == null)
        {
            return new ApiResponse<UserLoginDTO>
            {
                Status = "error",
                Message = "Invalid email or password."
            };
        }

        string tokenizer = _token.GenerateToken(user);
        return new ApiResponse<UserLoginDTO>
        {
            Status = "success",
            Message = "Login succesfully.",
            Data = new UserLoginDTO
            {
                Token = tokenizer,
                User = user
            }
        };
    }

    public Task<ApiResponse<UpdateUserDTO>> UpdateUser(UserRequestUpdate userRequestUpdate)
    {
        throw new NotImplementedException();
    }
}
