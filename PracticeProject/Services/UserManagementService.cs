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
        public async Task<IEnumerable<GetAllClientsDTO>> GetClients()
        {
            var clients = await _context.Clients.Include(user => user.User).ToListAsync();
            var clientGetModel = _mapper.Map<IEnumerable<GetAllClientsDTO>>(clients);

            return clientGetModel;
        }
    }
}
