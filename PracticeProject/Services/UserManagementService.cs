using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.DTOs;
using PracticeProject.Models;
using PracticeProject.Services.Interfaces;

namespace PracticeProject.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public UserManagementService(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager) 
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUser(ClientModel client)
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
                    await _context.Clients.AddAsync(clients);
                    await _context.SaveChangesAsync();
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

        public async Task<GetClientsDTO> GetClientById(Guid id)
        {
            var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.ClientId == id);

            if(client == null) return null;

            var clientMapped = _mapper.Map<GetClientsDTO>(client);

            return clientMapped;
        }

        public async Task<IEnumerable<GetClientsDTO>> GetClients()
        {
            var clients = await _context.Clients.Include(user => user.User).ToListAsync();
            var clientGetModel = _mapper.Map<IEnumerable<GetClientsDTO>>(clients);

            return clientGetModel;
        }

        public async Task<ServiceResult> UpdateClientByIdAsync(Guid clientId, UpdateClientDTO updateClientDto)
        {
            var client = await _context.Clients.Include(c => c.User).FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Client not found." };
            }

            var user = client.User;

            // Update UserName and Email
            user.UserName = updateClientDto.UserName;
            user.Email = updateClientDto.Email;

            // Update User
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Failed to update user." };
            }

            // Update Role
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRoleResult.Succeeded)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Failed to remove existing roles." };
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, updateClientDto.Role);
            if (!addRoleResult.Succeeded)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Failed to add new role." };
            }

            // Save changes
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> DeleteClientById(Guid clientId)
        {
            try
            {
                // Retrieve the client with related entities
                var client = await _context.Clients.Include(c => c.Transactions)
                                                   .Include(c => c.User)
                                                   .FirstOrDefaultAsync(c => c.ClientId == clientId);

                if (client == null)
                {
                    return new ServiceResult { Success = false, ErrorMessage = "Client not found." };
                }

                // Remove the associated transactions if necessary
                if (client.Transactions.Any())
                {
                    _context.Transactions.RemoveRange(client.Transactions);
                }

                // Optionally delete the associated User
                if (client.User != null)
                {
                    var deleteUserResult = await _userManager.DeleteAsync(client.User);
                    if (!deleteUserResult.Succeeded)
                    {
                        return new ServiceResult { Success = false, ErrorMessage = "Failed to delete associated user." };
                    }
                }

                // Remove the client
                _context.Clients.Remove(client);

                // Save changes
                await _context.SaveChangesAsync();

                return new ServiceResult { Success = true };
            }
            catch (DbUpdateConcurrencyException)
            {
                // Since the client was already deleted, treat it as a success
                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return new ServiceResult { Success = false, ErrorMessage = "An error occurred: " + ex.Message };
            }
        }
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
