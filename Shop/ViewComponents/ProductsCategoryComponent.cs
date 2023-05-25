using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository;
using ViewModel;

namespace Shop.ViewComponents
{
    public class ProductsCategoryViewComponent : ViewComponent
    {
        private readonly IDataService<Category> _dataService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly int ProductCount = 6;


        public ProductsCategoryViewComponent(IDataService<Category> dataService, IProductService productService, IProductImageService productImageService)
        {
            _dataService = dataService;
            _productService = productService;
            _productImageService = productImageService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int CategoryId, string? customTitle = null)
        {
            List<PreviewProductViewModel> products = new();
            var models = await _productService.GetByCategory(CategoryId, ProductCount);
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                };

                var img = await _productImageService.GetMainByProductId(item.Id);
                if (img != null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + img.Title;

                products.Add(PreviewProduct);
            }
            ViewBag.Category = await _dataService.Get(CategoryId);
            ViewBag.customTitle = customTitle;
            return await Task.FromResult((IViewComponentResult)View("ProductsCategory", products));
        }
    }
}