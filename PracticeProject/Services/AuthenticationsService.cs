using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PracticeProject.Data;
using PracticeProject.DTOs;
using PracticeProject.Models;
using PracticeProject.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PracticeProject.Services
{
    public class AuthenticationsService : IAuthenticationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        public AuthenticationsService(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _configuration = configuration;
            _secretKey = _configuration["Jwt:Key"];
        }
        public async Task AddClient(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> AddClientToDatabase(ClientModel client)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var user = _mapper.Map<User>(client);
            user.Id = Guid.NewGuid();
            var clients = _mapper.Map<Client>(client);

            try
            {
                var result = await _userManager.CreateAsync(user, client.Password);
                if (!result.Succeeded) return result;
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Client");
                if (addToRoleResult.Succeeded)
                {
                    clients.ClientId = user.Id;
                    await AddClient(clients);
                    await transaction.CommitAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                else
                    return addToRoleResult;
                return result;


            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return IdentityResult.Failed();
            }
        }

        public async Task<IActionResult> Login(LoginModel loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new UnauthorizedResult();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("clientId", user.Id.ToString())
            // Add more claims as needed
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the token as part of an OkObjectResult
            return new OkObjectResult(new { token = tokenString });
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
