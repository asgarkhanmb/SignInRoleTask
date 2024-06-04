using EducalBackend.Services.Interfaces;
using EducalBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EducalBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;

        public HomeController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                Categories = await _categoryService.GetAllAsync()
            };


            return View(model);
        }

    }
}
