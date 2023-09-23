using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Clean.Architecture.Core.Entities.Identity;
using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Architecture.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly ILogger<TokenService> _logger;
  public TokenService(IConfiguration config, ILogger<TokenService> logger)
  {    
    _logger = logger;
    _config = config ?? throw new ArgumentNullException(nameof(config));

    _logger.LogInformation("TokenService constructor called with config: {config} and logger: {logger}", config, logger);

    // Generate a random key if it's not configured in the app settings
    var tokenKey = _config["Token:Key"];
    if (string.IsNullOrEmpty(tokenKey))
    {
        // Generate a 64-byte (512-bit) random key for HMAC-SHA512
        var randomKeyBytes = new byte[64];
        RandomNumberGenerator.Fill(randomKeyBytes);
        
        tokenKey = Convert.ToBase64String(randomKeyBytes);
        Console.WriteLine("This is TOKEn KEY----------->"+tokenKey);
        _logger.LogInformation("Generated token key: {tokenKey}", tokenKey);
    }
    
    // Initialize _key after generating it
    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
    
    // Log the generated security key
    //_logger.LogInformation("Generated security key: {key}", _key);
  }  

  public string CreateToken(AppUser user)
  {
    if (user == null)
    {
        throw new ArgumentNullException(nameof(user), "User cannot be null");
    }

    var claims = new List<Claim>();

    if (!string.IsNullOrEmpty(user.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email)); 
        }
    
    if (!string.IsNullOrEmpty(user.DisplayName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.DisplayName));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Set token expiration time as needed
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature), // Use HmacSha256Signature here
                Issuer = _config["Token:Issuer"]
            };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);        
  }
}