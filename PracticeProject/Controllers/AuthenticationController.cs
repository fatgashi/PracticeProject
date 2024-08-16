using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracticeProject.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationsService _authenticationsService;

        public AuthenticationController(IAuthenticationsService authenticationsService)
        {
            _authenticationsService = authenticationsService;
        }


        [HttpPost("register")]
        public async Task<IdentityResult> RegisterClients(ClientModel client)
        {

            var result = await _authenticationsService.AddClientToDatabase(client);

            return result;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = await _authenticationsService.Login(login);

            return user;
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task Logout()
        {
            await _authenticationsService.Logout();
        }
    }
}
