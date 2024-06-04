using Microsoft.AspNetCore.Mvc;

namespace EducalBackend.ViewComponents
{

    public class FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
