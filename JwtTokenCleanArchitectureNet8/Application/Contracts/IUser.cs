using Application.DTOs;

namespace Application.Contracts;

public interface IUser
{
    Task<RegistrationResponse> RegisterUserAsync(RegisterUerDTO registerUerDTO);
    Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO);
}
