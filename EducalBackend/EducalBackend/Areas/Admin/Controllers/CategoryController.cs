using EducalBackend.Data;
using EducalBackend.Helpers.Extensions;
using EducalBackend.Models;
using EducalBackend.Services.Interfaces;
using EducalBackend.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EducalBackend.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ICategoryService categoryService
                                 ,IWebHostEnvironment env
                                 ,AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
            _env = env;



        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAlWithProductCountAsync());
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This name already exist");
                return View();
            }
            if (!category.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Accept only image format");
                return View();
            }
            if (!category.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be 200 KB");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + category.Image.FileName;
            string path = _env.GenerateFilePath("assets/images", fileName);

            await category.Image.SaveFileToLocalAsync(path);
            await _categoryService.CreateAsync(new Category { Name = category.Name, Description = category.Description, Image = fileName });

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var category = await _categoryService.GetByIdAsync((int)id);
            if (category is null) return NotFound();
            string path = _env.GenerateFilePath("assets/images", category.Image);
            path.DeleteFileFromLocal();
            await _categoryService.DeleteAsync(category);
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Category category = await _categoryService.GetByIdAsync((int)id);


            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryEditVM
            {
                Image = category.Image,
                Description = category.Description,
                Name = category.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            if (request.NewImage != null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Accept only image format");
                    return View(request);
                }
                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB");
                    return View(request);
                }

                string oldPath = _env.GenerateFilePath("assets/images", category.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("assets/images", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                category.Image = fileName;
            }

            category.Description = request.Description;
            category.Name = request.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }

}
