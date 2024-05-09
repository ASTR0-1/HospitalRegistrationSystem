using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.ConfigurationModels;
using HospitalRegistrationSystem.Application.DTOs.AuthenticationDTOs;
using HospitalRegistrationSystem.Application.Interfaces.Identity;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HospitalRegistrationSystem.Infrastructure.Persistence.Identity;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    private ApplicationUser? _user;

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

        if (_user is null)
            return false;
        var passwordCheck = await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password);

        return passwordCheck;
    }

    /// <inheritdoc />
    public async Task<string> CreateTokenAsync(ApplicationUser? user = null)
    {
        _user ??= user;

        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokeOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private JwtSecurityToken GenerateTokeOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
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
}
