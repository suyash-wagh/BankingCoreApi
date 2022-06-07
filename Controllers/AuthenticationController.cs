using BankingCoreApi.DTOs;
using BankingCoreApi.Infrastructure;
using BankingCoreApi.Models;
using BankingCoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            BankingDbContext context,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
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
                    UserName = userDTO.FirstName + userDTO.LastName + Guid.NewGuid().ToString()
                };
            }
            else return BadRequest("Passwords doesn't match.");

            var result = await _userManager.CreateAsync(user, userDTO.Password.Cipher());

            if (result.Succeeded)
            {
                if(userDTO.Role == Roles.User.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                    _context.Transactions.Add(new Transaction()
                    {
                        Amount = userDTO.Balance,
                        UserId = user.Id,
                        User = user,
                        Created = DateTime.UtcNow,
                        TransactionType = "D"
                    });
                    _context.SaveChanges();
                }
                else if (userDTO.Role == Roles.Admin.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                return Ok("User Created.");
            }
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
                var token = await GenerateJwtToken(userAvailable, null);
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO rtDTO)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await GenerateRefreshToken(rtDTO);
            return Ok(result);
        }

        private async Task<TokenDTO> GenerateRefreshToken(RefreshTokenDTO rtDTO)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var refreshTokenInDb = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == rtDTO.RefreshToken);
            var user = await _userManager.FindByIdAsync(refreshTokenInDb.UserId);
            try
            {
                var result = jwtTokenHandler.ValidateToken(rtDTO.Token, _tokenValidationParameters, out var validatedToken);
                return await GenerateJwtToken(user, refreshTokenInDb);
            }
            catch (SecurityTokenExpiredException)
            {
                if(refreshTokenInDb.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJwtToken(user, refreshTokenInDb);
                }
                else
                {
                    return await GenerateJwtToken(user, null);
                }
            }
        }

        private async Task<TokenDTO> GenerateJwtToken(User user, RefreshToken rToken)
        {
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var loginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(loginKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(rToken != null)
            {
                var resultHere = new TokenDTO
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExiresAt = token.ValidTo
                };
                return resultHere;
            }

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                DateCreated = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(2),
                UserId = user.Id,
                Token = Guid.NewGuid().ToString() + "-Suyash-Ujjay-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var result = new TokenDTO
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExiresAt = token.ValidTo
            };
            return result;
        }
    }
}
