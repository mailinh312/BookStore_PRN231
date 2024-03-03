using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;
using Repository.Repositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _repository;
        public AuthorController(IAuthorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("AllAuthors")]
        public IActionResult GetAllAuthors()
        {
            try
            {
                return Ok(_repository.GetAllAuthors());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("AuthorsActive")]
        public IActionResult GetAuthorsWithActiveIsTrue()
        {
            try
            {
                return Ok(_repository.GetAuthorsWithActiveIsTrue());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Author")]
        public IActionResult GetAuthorById(int id)
        {
            try
            {
                return Ok(_repository.GetAuthorById(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewAuthor(AuthorDto author)
        {
            try
            {
                _repository.AddNewAuthor(author);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateAuthor(AuthorDto author)
        {
            try
            {
                _repository.UpdateAuthor(author);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
