using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IAppUserRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterDTO model);
        Task<string> LoginAsync(LoginDTO model);

        Task<List<UserDto>> GetAllUsers();

        Task<List<UserDto>> GetUsersByUserName(string name);

        Task<UserDto> GetUserById(string id);
        Task UpdateRole(List<String> roles, string username);
    }
}
