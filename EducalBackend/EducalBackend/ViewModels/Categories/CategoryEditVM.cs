using System.ComponentModel.DataAnnotations;

namespace EducalBackend.ViewModels.Categories
{
    public class CategoryEditVM
    {
        [Required]
        public string Description { get; set; }
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
