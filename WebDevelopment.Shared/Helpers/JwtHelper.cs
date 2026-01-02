using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebDevelopment.Shared.DTOs.Users;

namespace WebDevelopment.Shared.Helpers;

public class JwtHelper(IConfiguration configuration)
{
    public string GenerateToken(UserDto currentUser)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
            new (ClaimTypes.Name, currentUser.Name),
            new (ClaimTypes.Surname, currentUser.Surname),
            new (ClaimTypes.Email, currentUser.Email),
            new ("IsFirstLogin", currentUser.IsFirstLogin.ToString()),
        };

        var userClaims = claims.ToList();

        userClaims.AddRange(currentUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityToken = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:ExpireTimeInMinutes"]!)),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(securityToken);

        Console.WriteLine("Acces token granted.");

        return token;
    }

    public string GenerateRefreshToken(UserDto currentUser)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:RefreshKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString())
        }.ToList();

        var securityRefreshToken = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddHours(int.Parse(configuration["Jwt:ExpireTimeInHours"]!)),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var refreshToken = tokenHandler.WriteToken(securityRefreshToken);

        Console.WriteLine("Refresh token granted.");

        return refreshToken;
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

            var validationParameters = GetTokenValidationParameters(configuration);

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken)
            {
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    Console.WriteLine("Acces token expired.");
                    return false;
                }
                Console.WriteLine("Acces token valid.");
                return true;
            }
            return false;

        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine(ex);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
    public ClaimsPrincipal? ValidateRefreshToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.UTF8.GetBytes(configuration["Jwt:RefreshKey"]!);

            var validationParameters = GetRefreshTokenValidationParameters(configuration);

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken && validatedToken.ValidTo >= DateTime.UtcNow)
            {
                return principal;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
        };
    }

    public static TokenValidationParameters GetRefreshTokenValidationParameters(IConfiguration configuration)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:RefreshKey"]!))
        };
    }
}
