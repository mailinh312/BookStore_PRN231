using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AppUserRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || !passwordValid)
            {
                return string.Empty;
            }
            // định nghĩa ra các claim cho user
            var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<IdentityResult> RegisterAsync(RegisterDTO model)
        {
            var IsExist = await _userManager.FindByNameAsync(model.UserName);
            if (IsExist != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "UsernameAlreadyTaken", Description = "Username is already taken." });
            }

            AppUser user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Fullname = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            // sau khi tạo được user,thì addRole mặc định cho user mới này
            if (!result.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Code = "RegisterFailed", Description = "User creation failed! Please check user details and try again." });

            }
            else
            {
                // check Role mặc định đã tồn tại trong DB chưa
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                // đăng kí role mặc định đó cho user
                await _userManager.AddToRoleAsync(user, "Customer");
            }

            return result;
        }
    }
}
