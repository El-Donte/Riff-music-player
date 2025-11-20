using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace RiffBackend.API.Extensions;

public static class ApiExtensions
{
    private static string _key = "";
    private static string _coockieName = "";

    public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        _key = configuration["Jwt:Key"]
               ?? throw new InvalidOperationException("JWT key is missing!");

        _coockieName = configuration["Authentication:CookieName"]
               ?? throw new InvalidOperationException("CookieName is missing!");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[_coockieName];

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }
}
