using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Models;

namespace PracticeProject.Services.Interfaces
{
    public interface IAuthenticationsService
    {
        Task AddClient(Client client);
        Task<IdentityResult> AddClientToDatabase(ClientModel client);

        Task<IActionResult> Login(LoginModel loginDto);
        Task Logout();
    }
}
