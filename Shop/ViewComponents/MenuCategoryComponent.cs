using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Shop.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly IDataService<Category> _dataService;

        public MenuCategoryViewComponent(IDataService<Category> dataService) => _dataService = dataService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _dataService.GetAll();
            return await Task.FromResult((IViewComponentResult)View("MenuCategory", model));
        }
    }
}