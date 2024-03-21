using Application.DTOs;

namespace Application.Contracts;

public interface IUser
{
    Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUerDTO);
    Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO);
}
