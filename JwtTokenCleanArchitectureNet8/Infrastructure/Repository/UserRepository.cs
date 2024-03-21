using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repository;

public class UserRepository : IUser
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;

    public UserRepository(ApplicationDbContext applicationDbContext, IConfiguration configuration)
    {
        _applicationDbContext = applicationDbContext;
        _configuration = configuration;
    }
    public async Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO)
    {
        var getUser = await FindUserByEmail(loginDTO.Email!);
        if (getUser == null) return new LoginResponse(false, "User not found");

        bool chechPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
        if (chechPassword)
            return new LoginResponse(true, "Login sucess", GenerateJWTToken(getUser));
        else
            return new LoginResponse(false, "Invalid crendentials");
    }

    private string GenerateJWTToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: credentials
            );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<ApplicationUser> FindUserByEmail(string email) =>
        await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
    {
        var getUser = await FindUserByEmail(registerUserDTO.Email!);
        if (getUser != null) 
            return new RegistrationResponse(false, "User already exist");

        _applicationDbContext.Users.Add(new ApplicationUser()
        {
            Name = registerUserDTO.Name,
            Email = registerUserDTO.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
        });
        await _applicationDbContext.SaveChangesAsync();
        return new RegistrationResponse(true, "Registration completed");
    }
}
