using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.ConfigurationModels;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.DTOs.TokenDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Identity;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Identity;

/// <summary>
///     Manages the authentication of users.
/// </summary>
public class AuthenticationManager : IAuthenticationManager
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    private ApplicationUser? _user;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthenticationManager"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="jwtSettingsOptions">The JWT settings options.</param>
    public AuthenticationManager(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication)
    {
        _user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == userForAuthentication.PhoneNumber);

        if (_user is null || string.IsNullOrEmpty(userForAuthentication.Password))
            return false;

        var passwordCheck = await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password);

        return passwordCheck;
    }

    /// <inheritdoc />
    public async Task<TokenDto> CreateTokenAsync(ApplicationUser? user = null, bool populateExpiration = false)
    {
        _user ??= user;

        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;
        
        if (populateExpiration)
            _user!.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <inheritdoc />
    public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var userId = int.Parse(principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null 
            || user.RefreshToken != tokenDto.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new SecurityTokenException("Invalid try to refresh token. Token DTO has some invalid values.");
        }

        return await CreateTokenAsync(user);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var validIssuer = _jwtSettings.ValidIssuer;
        var validAudience = _jwtSettings.ValidAudience;
        var expires = DateTime.Now
            .AddMinutes(Convert.ToDouble(_jwtSettings.Expires));

        return new JwtSecurityToken(
            validIssuer,
            validAudience,
            claims,
            expires: expires,
            signingCredentials: signingCredentials
        );
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, _user!.Id.ToString()),
                new("HospitalId", _user!.HospitalId.ToString()!)
            };

        var roles = await _userManager.GetRolesAsync(_user);

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.ValidIssuer,
            ValidAudience = _jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),
            ValidateLifetime = true,
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
