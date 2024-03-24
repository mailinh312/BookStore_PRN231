using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ICategoryRepository
    {
        List<CategoryDto> GetAllCategories();

        List<CategoryDto> GetCategoriesWithActiveIsTrue();

        CategoryDto GetCategoryById(int id);
        void AddNewCategory(CategoryCreateDto categoryDto);
        void UpdateCategory(CategoryDto categoryDto);
        List<Top3Category> GetTop3BestCategories();

    }
}
