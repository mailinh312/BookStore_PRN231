using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDetailController : ControllerBase
    {
        private readonly IImportDetailRepository _repository;
        public ImportDetailController(IImportDetailRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("Import")]
        public IActionResult GetImportByImportId(int id)
        {
            try
            {
                return Ok(_repository.GetImportDetailsByImportId(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewImport(ImportDetailCreateDto importDetailCreateDto)
        {
            try
            {
                _repository.AddNewImportDetail(importDetailCreateDto);
                return Ok(); 
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
