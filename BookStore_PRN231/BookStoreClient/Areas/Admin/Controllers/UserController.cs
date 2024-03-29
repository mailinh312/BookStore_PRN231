﻿using BookStoreClient.ShareApiService;
using BusinessObjects.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Repository.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreClient.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin/user")]
	public class UserController : Controller
	{
		private readonly UserApiService userApiService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserController(IHttpContextAccessor httpContextAccessor)
		{
			userApiService = new UserApiService();
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<IActionResult> Index()
		{
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			ViewBag.UserName = getUserNameByToken();

			List<UserDto> users = await userApiService.GetUsers();
			return View(users);
		}
		[HttpGet("updateuser")]
		public async Task<IActionResult> UpdateUserForm(string userName)
		{

			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}
			List<string> roles = await userApiService.GetAllRoles();
			ViewBag.Roles = roles;
			ViewBag.UserNameUpdate = userName;
			ViewBag.userRoles = await userApiService.GetAllRolesByUsername(userName);
			ViewBag.UserName = getUserNameByToken();
			return View();
		}

		[HttpPost("Updateuser")]
		public async Task<IActionResult> UpdateRoleUser(List<string> selectRole, string username)
		{
			if (!IsAccess())
			{
				return Redirect("/forbidden");
			}

			if (selectRole != null)
			{
				HttpResponseMessage response = await userApiService.UpdateRoleUser(selectRole, username);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
			return RedirectToAction("UpdateUserForm");
		}

		private string getUserNameByToken()
		{
			string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
			var username = "";
			if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
			{
				Console.WriteLine("Chuỗi token không được để trống hoặc null.");
				token = "";
			}
			else
			{
				var handler = new JwtSecurityTokenHandler();
				var jwtToken = handler.ReadJwtToken(token);

				// Trích xuất tên người dùng từ payload
				username = jwtToken.Payload["username"]?.ToString();
			}
			return username;
		}

		private List<string> getRoleByToken()
		{
			string token = _httpContextAccessor.HttpContext.Session.Get<String>("token");
			if (string.IsNullOrEmpty(token)) // Kiểm tra chuỗi token có rỗng hoặc null không
			{
				Console.WriteLine("Chuỗi token không được để trống hoặc null.");
				token = "";
			}
			else
			{
				var handler = new JwtSecurityTokenHandler();
				var jwtToken = handler.ReadJwtToken(token);

				if (jwtToken != null)
				{
					// Lấy danh sách các Claims từ token
					var claims = jwtToken.Claims;

					// Lọc ra các Claims có loại là "role" và trích xuất giá trị
					var roles = claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

					return roles;
				}
			}
			return new List<string>();
		}

		private bool IsAccess()
		{
			List<string> roles = getRoleByToken();
			if (roles.Contains("Administrator"))
			{
				return true;
			}
			return false;
		}
	}
}
