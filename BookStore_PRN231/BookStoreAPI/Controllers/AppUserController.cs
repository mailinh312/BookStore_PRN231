using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserRepository _repository;
        private readonly IMapper _mapper;
        public AppUserController(IAppUserRepository userRepository, IMapper mapper)
        {
            {
                _repository = userRepository;
                _mapper = mapper;
            }

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            //AppUser user = new AppUser();
            //user = Mapper.Map<AppUser>(model);
            var result = await _repository.RegisterAsync(model);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "User registered successfully. "
                });
            }
            // Trả về HTTP Status Code 400 Bad Request và một đối tượng JSON với danh sách lỗi
            return BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _repository.LoginAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized(new { Success = false, Message = "Invalid username or password." });
            }
            return Ok(new
            {
                Success = true,
                Token = result,
                Message = "Login successful."
            });
        }
    }
}
