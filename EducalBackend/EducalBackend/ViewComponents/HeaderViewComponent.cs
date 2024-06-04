using EducalBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducalBackend.ViewComponents
{
   
    public class HeaderViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
            {
                return View();
            }
        
    }
}
