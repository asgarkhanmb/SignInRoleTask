using System.ComponentModel.DataAnnotations;

namespace EducalBackend.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(100)]
        public string Name { get; set; }


        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }
}
