using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;

namespace PracticeProject.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<IEnumerable<GetClientsDTO>> GetClients();
        Task<IdentityResult> CreateUser(ClientModel client);
        Task<GetClientsDTO> GetClientById(Guid id);
        Task<ServiceResult> UpdateClientByIdAsync(Guid clientId, UpdateClientDTO updateClientDto);
        Task<ServiceResult> DeleteClientById(Guid clientId);

    }
}
