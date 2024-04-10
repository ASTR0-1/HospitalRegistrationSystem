using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HospitalRegistrationSystem.Application.DTOs;
using HospitalRegistrationSystem.Application.Interfaces;
using HospitalRegistrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HospitalRegistrationSystem.Infrastructure.Identity;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    private ApplicationUser? _user;

    public AuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuthentication)
    {
        _user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == userForAuthentication.PhoneNumber);

        if (_user is null) 
            return false;
        var passwordCheck = await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password);

        return passwordCheck;
    }

    public async Task<string> CreateTokenAsync()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokeOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private JwtSecurityToken GenerateTokeOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        return new JwtSecurityToken(
            _configuration.GetSection("JwtSettings").GetSection("validIssuer").Value,
            _configuration.GetSection("JwtSettings").GetSection("validAudience").Value,
            claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(_configuration.GetSection("JwtSettings").GetSection("expires").Value)),
            signingCredentials: signingCredentials
        );
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, _user!.Id)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSecret").Value!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
}
