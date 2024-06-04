using EducalBackend.Models;
using EducalBackend.ViewModels.Categories;

namespace EducalBackend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryCourseVM>> GetAlWithProductCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task DeleteAsync(Category category);
    }
}
