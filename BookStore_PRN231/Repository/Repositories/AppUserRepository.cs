﻿using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AppUserRepository(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                List<AppUser> users = _userManager.Users.ToList();

                var usersDto = _mapper.Map<List<UserDto>>(users);
                foreach (var userDto in usersDto)
                {
                    AppUser user = _userManager.Users.FirstOrDefault(x => x.Id.Equals(userDto.Id));
                    userDto.Roles = (await _userManager.GetRolesAsync(user)) as List<string>;
                }
                return usersDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetTotalUser()
        {
            return _userManager.Users.Count();
        }

        public async Task<UserDto> GetUserById(string id)
        {
            try
            {
                AppUser user = _userManager.Users.FirstOrDefault(x => x.Id.Equals(id));
                if (user == null)
                {
                    throw new Exception("Not found user!");
                }
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = (await _userManager.GetRolesAsync(user)) as List<string>;
                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserDto>> GetUsersByUserName(string name)
        {
            try
            {
                List<AppUser> users = _userManager.Users.Where(x => x.UserName.ToUpper().Contains(name.ToUpper())).ToList();
                if (!users.Any())
                {
                    throw new Exception("Not found any user!");
                }
                var usersDto = _mapper.Map<List<UserDto>>(users);
                foreach (var userDto in usersDto)
                {
                    AppUser user = _userManager.Users.FirstOrDefault(x => x.Id.Equals(userDto.Id));
                    userDto.Roles = (await _userManager.GetRolesAsync(user)) as List<string>;
                }
                return usersDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            var authClaims = new List<Claim>
            {
                new Claim("username", user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("role", userRole));
            }
            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
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

        public async Task UpdateRole(List<String> roles, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var oldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            // so sánh giữa role cũ và role mới chọn 

            // nếu có trong role cũ mà ko có trong role mới vừa chọn -> thì đấy là những cái role cần phải xóa
            var deleteRoles = oldRoleNames.Where(x => !roles.Contains(x));
            // những cái role có ở trong list role mới nhưng ko có trong role cũ -> thì đấy là những cái role cần thêm vào
            var addRoles = roles.Where(x => !oldRoleNames.Contains(x));

            //delete role thừa
            var rsDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);

            //thêm một mảng các role cho user
            var rsAdd = await _userManager.AddToRolesAsync(user, addRoles);
        }

        public async Task UpdateUser(UserDto user)
        {
            var userExist = await _userManager.FindByNameAsync(user.UserName);

            if (userExist != null)
            {
                userExist.Fullname = user.UserName;
                userExist.PhoneNumber = user.PhoneNumber;
                userExist.Address = user.Address;
                var result = await _userManager.UpdateAsync(userExist);
            }
        }
    }
}
