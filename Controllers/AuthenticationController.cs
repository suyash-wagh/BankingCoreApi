using BankingCoreApi.DTOs;
using BankingCoreApi.Infrastructure;
using BankingCoreApi.Models;
using BankingCoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankingCoreApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BankingDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            BankingDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please enter all details");
            }

            var userExists = await _userManager.FindByEmailAsync(userDTO.Email);
            if (userExists != null)
            {
                return BadRequest($"User with {userDTO.Email} already exists.");
            }

            User user;
            string confirmPass = userDTO.ConfirmPassword.Cipher();
            if (confirmPass == userDTO.Password.Cipher())
            {
                user = new User()
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Balance = userDTO.Balance,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.ConfirmPassword.Cipher(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = userDTO.FirstName + userDTO.LastName
                };
            }
            else return BadRequest("Passwords doesn't match.");

            var result = await _userManager.CreateAsync(user, userDTO.Password.Cipher());

            if (result.Succeeded) return Ok("User Created.");
            return BadRequest("User Could not be created.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please enter all details");
            }
            var userAvailable = await _userManager.FindByEmailAsync(userDTO.Email);

            if (userAvailable != null && await _userManager.CheckPasswordAsync(userAvailable, userDTO.Password.Cipher()))
            {
                var token = await GenerateJwtToken(userAvailable);
                return Ok(token);
            }
            return Unauthorized();
        }

        private async Task<TokenDTO> GenerateJwtToken(User user)
        {
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var loginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT: Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(loginKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var result = new TokenDTO
            {
                Token = jwtToken,
                ExiresAt = token.ValidTo
            };
            return result;
        }
    }
}
