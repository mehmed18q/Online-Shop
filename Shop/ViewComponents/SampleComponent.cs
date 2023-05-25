using Microsoft.AspNetCore.Mvc;

namespace Shop.ViewComponents
{
    public class SampleComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("Sample"));
        }
    }
}