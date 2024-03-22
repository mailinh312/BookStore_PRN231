using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.IRepositories;
using System.Text;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportRepository _repository;
        public ImportController(IImportRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("AllImports")]
        public IActionResult GetAllImport()
        {
            try
            {
                return Ok(_repository.GetAllImports());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Import")]
        public IActionResult GetImportById(int id)
        {
            try
            {
                return Ok(_repository.GetImportsById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewImport(ImportCreateDto importCreateDto)
        {
            try
            {
                return Ok(_repository.AddNewImport(importCreateDto));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
