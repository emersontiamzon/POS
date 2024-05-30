using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;

namespace Point.Of.Sale.Persistence.Extensions;

public static class TokenExtension
{
    public static string GenerateToken(this TokenBuilderParameters parameters)
    {
        var audience = parameters.Configuration.General.ServiceName;
        var secret = Encoding.UTF8.GetBytes(parameters.Configuration.General.SecretKey);
        var jwtToken = new JwtSecurityToken(
            $"{audience}-{audience}",
            audience,
            parameters.Claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256),
            notBefore: DateTime.UtcNow,
            expires: parameters.ExpiresIn.TotalMinutes > 500_000 ? DateTime.UtcNow.AddDays(parameters.ExpiresIn.TotalDays) : DateTime.UtcNow.AddMinutes(parameters.ExpiresIn.TotalMinutes));

        var tokenHandler = new JwtSecurityTokenHandler();

        if (TryParseToken(tokenHandler.WriteToken(jwtToken), parameters.Configuration.General, out var claimsTest))
        {
            return tokenHandler.WriteToken(jwtToken);
        }

        return string.Empty;
    }

    public static List<Claim> CreateClaims(this ServiceUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Surname, user.LastName ?? ""),
            new(ClaimTypes.GivenName, user.FirstName ?? ""),
            new(ClaimTypes.Name, user.UserName ?? ""),
        };

        claims.Add(new Claim("http://kodelev8.com/userid", user.Id));
        claims.Add(new Claim("http://kodelev8.com/usertype", "user"));
        claims.Add(new Claim("http://kodelev8.com/tenant", user.TenantId.ToString()));
        claims.AddRange(SetAccessRoles(new List<string> {"read", "write", "delete"}));

        return claims;
    }

    public static List<Claim> CreateClaims(this Tenant tenant)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, tenant.Email ?? ""),
            new(ClaimTypes.Name, tenant.Name ?? ""),
        };

        claims.Add(new Claim("http://kodelev8.com/tenanttype", tenant.Type.ToString()));
        claims.Add(new Claim("http://kodelev8.com/usertype", "tenant"));
        claims.Add(new Claim("http://kodelev8.com/tenantid", tenant.Id.ToString()));
        claims.AddRange(SetAccessRoles(new List<string> {"read", "write", "delete"}));

        return claims;
    }

    private static IList<Claim> SetAccessRoles(IEnumerable<string> accessRoles)
    {
        return accessRoles.Select(role => new Claim("http://kodelev8.com/accessroles", role)).ToList();
    }

    private static bool TryParseToken(string token, General general, out IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"{general.ServiceName}-{general.ServiceName}",
                    ValidateAudience = true,
                    ValidAudience = general.ServiceName,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(general.SecretKey)),
                    RequireExpirationTime = true,
                },
                out var validatedToken);
            claims = tokenHandler.ReadJwtToken(token).Claims;
            return validatedToken.ValidTo > DateTime.UtcNow;
        }
        catch (SecurityTokenException ex)
        {
            claims = Array.Empty<Claim>();
            throw;
        }

        return false;
    }
}
