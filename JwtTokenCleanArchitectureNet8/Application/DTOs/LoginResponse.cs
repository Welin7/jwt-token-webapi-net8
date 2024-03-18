namespace Application.DTOs;

public record LoginResponse(bool Flag, string Messagem = null!, string Token = null!);

