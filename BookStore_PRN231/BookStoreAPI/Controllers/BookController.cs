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
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("AllBooks")]
        public IActionResult GetAllBooks()
        {
            try
            {
                List<BookDto> bookDTOs = _bookRepository.GetAllBooks();
                return Ok(bookDTOs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Author")]
        public IActionResult GetBooksByAuthor(int id)
        {
            try
            {
                List<BookDto> bookDTOs = _bookRepository.GetBooksByAuthor(id);
                return Ok(bookDTOs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Category")]
        public IActionResult GetBooksByCategory(int id)
        {
            try
            {
                List<BookDto> bookDTOs = _bookRepository.GetBooksByCategory(id);
                return Ok(bookDTOs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                BookDto book = _bookRepository.GetByBookById(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Title")]
        public IActionResult GetBookByTitle(string title)
        {
            try
            {
                List<BookDto> bookDTOs = _bookRepository.GetBooksByTitle(title);
                return Ok(bookDTOs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewBook(BookCreateDto book)
        {
            try
            {
                _bookRepository.AddNewBook(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateBook(BookDto book)
        {
            try
            {
                _bookRepository.UpdateBook(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
