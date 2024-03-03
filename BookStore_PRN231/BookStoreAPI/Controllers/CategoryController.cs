﻿using BusinessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("AllCategories")]
        public IActionResult GetAllCategories()
        {
            try
            {
                return Ok(_categoryRepository.GetAllCategories());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("CategoriesActive")]
        public IActionResult GetCategoriesWithActiveIsTrue()
        {
            try
            {
                return Ok(_categoryRepository.GetCategoriesWithActiveIsTrue());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("Category")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                return Ok(_categoryRepository.GetCategoryById(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public IActionResult AddNewCategory(CategoryDto categoryDto)
        {
            try
            {
                _categoryRepository.AddNewCategory(categoryDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateCategory(CategoryDto categoryDto)
        {
            try
            {
                _categoryRepository.UpdateCategory(categoryDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
