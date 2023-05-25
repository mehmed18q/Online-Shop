using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Shop.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IDataService<Brand> _dataService;

        public BrandsViewComponent(IDataService<Brand> dataService) => _dataService = dataService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _dataService.GetAll();
            return await Task.FromResult((IViewComponentResult)View("Brands", model.Take(6)));
        }
    }
}