using PracticeProject.DTOs;

namespace PracticeProject.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<IEnumerable<GetAllClientsDTO>> GetClients();


    }
}
