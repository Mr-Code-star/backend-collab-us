using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend_collab_us.IAM.Application.Internal.OutboundServices;
using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.Infrastructure.Token.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backend_collab_us.IAM.Infrastructure.Token.JWT.Services;

public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;
    public string GenerateToken(User user)
    {
        var secret = _tokenSettings.Secret;
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            var userId = int.Parse(principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
            return await Task.FromResult(userId);
        }
        catch
        {
            return null;
        }
    }
}