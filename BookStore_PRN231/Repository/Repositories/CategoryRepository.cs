using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
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
        public void AddNewCategory(CategoryCreateDto categoryDto)
        {
            try
            {
                Category category = _context.Categories.FirstOrDefault(x => x.CategoryName.ToUpper().Trim().Equals(categoryDto.CategoryName.ToUpper().Trim()));
                if (category != null)
                {
                    throw new Exception("Category existed!");
                }
                category = _mapper.Map<Category>(categoryDto);
                category.Active = true;
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Top3Category> GetTop3BestCategories()
        {
            try
            {
                List<Top3Category> list = _context.OrderDetails.Include(od => od.Book).GroupBy(od => od.Book.CategoryId).Select(x => new Top3Category
                {
                    CategoryId = x.Key,
                    SoldQuantity = x.Sum(od => od.Quantity)
                }).OrderByDescending(x => x.SoldQuantity).Take(3).ToList();

                foreach (var item in list)
                {
                    item.CategoryName = _context.Categories.FirstOrDefault(x => x.CategoryId == item.CategoryId).CategoryName;
                }
                return list;
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
                if (!categories.Any())
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
                if (!categories.Any())
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

                
                if (_context.Categories.Any(x => x.CategoryName.ToUpper().Trim().Equals(categoryDto.CategoryName.ToUpper().Trim())))
                {
                    throw new Exception("Category name existed!");
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
