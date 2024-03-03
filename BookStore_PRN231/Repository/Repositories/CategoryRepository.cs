using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddNewCategory(CategoryDto categoryDto)
        {
            try
            {
                Category category = _context.Categories.FirstOrDefault(x => x.CategoryName.ToUpper().Equals(categoryDto.CategoryName.ToUpper()));
                if (category != null)
                {
                    throw new Exception("Category existed!");
                }
                category = _mapper.Map<Category>(categoryDto);
                category.CategoryId = 0;
                category.Active = true;
                category.Books = null;
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CategoryDto> GetAllCategories()
        {
            try
            {
                List<Category> categories = _context.Categories.ToList();
                if (categories.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }
                List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
                return categoriesDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CategoryDto> GetCategoriesWithActiveIsTrue()
        {
            try
            {
                List<Category> categories = _context.Categories.Where(x => x.Active == true).ToList();
                if (categories.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }
                List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
                return categoriesDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CategoryDto GetCategoryById(int id)
        {
            try
            {
                Category category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);

                if (category == null)
                {
                    throw new Exception("Category does not exist!");
                }
                CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
                return categoryDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCategory(CategoryDto categoryDto)
        {
            try
            {

                if (!_context.Categories.Any(x => x.CategoryId == categoryDto.CategoryId))
                {
                    throw new Exception("Category does not exist!");
                }

                Category category = _context.Categories.FirstOrDefault(x => x.CategoryId == categoryDto.CategoryId);
                _mapper.Map(categoryDto, category);
                _context.Categories.Update(category);
                if (!category.Active)
                {
                    List<Book> books = _context.Books.Where(x => x.CategoryId == category.CategoryId).ToList();
                    foreach (Book book in books)
                    {
                        book.Active = false;
                        _context.Update(book);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
