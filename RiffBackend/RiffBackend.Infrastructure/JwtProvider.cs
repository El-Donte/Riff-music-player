using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RiffBackend.Infrastructure;

public sealed class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    private readonly string _key = configuration["Jwt:Key"]
               ?? throw new InvalidOperationException("JWT key is missing!");

    private readonly int _expires = configuration.GetValue("Jwt:ExpiresHours", 12);

    public string GenerateToken(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString())];

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddHours(_expires));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Guid GetGuidFromJwt(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        }, out SecurityToken validatedToken);

        var id = ((JwtSecurityToken)validatedToken).Claims.First().Value;
        return Guid.Parse(id);
    }
}
