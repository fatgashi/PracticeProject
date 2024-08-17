using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracticeProject.Controllers
{
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IEnumerable<GetClientsDTO>> GetClients()
        {
            return await _userManagementService.GetClients();
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientById(Guid clientId)
        {
            var client = await _userManagementService.GetClientById(clientId);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        public async Task<IdentityResult> AddClientByAdmin(ClientModel client)
        {
            var result = await _userManagementService.CreateUser(client);

            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientById(Guid id, [FromBody] UpdateClientDTO updateClientDto)
        {
            var result = await _userManagementService.UpdateClientByIdAsync(id, updateClientDto);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientById(Guid id)
        {
            var result = await _userManagementService.DeleteClientById(id);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
