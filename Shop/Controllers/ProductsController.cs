using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository;
using ViewModel;

namespace Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly int ProductCount = 6;
        private readonly IDataService<Category> _categoryService;
        private readonly IDataService<Brand> _brandService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageDataService;

        public ProductsController(
            IDataService<Category> categoryService,
            IProductService productService,
            IProductImageService productImageDataService,
            IDataService<Brand> brandService)
        {
            _categoryService = categoryService;
            _brandService = brandService;
            _productService = productService;
            _productImageDataService = productImageDataService;
        }
        [HttpGet]
        public async Task<IActionResult> ListByBrand(int id)
        {
            List<PreviewProductViewModel> products = new();
            var models = await _productService.GetByBrand(id);
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                };
                var img = await _productImageDataService.GetMainByProductId(item.Id);

                if (img != null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + img.Title;

                products.Add(PreviewProduct);
            }
            IEnumerable<PreviewProductViewModel> PreviewProducts = products;
            var Brand = await _brandService.Get(id);
            ViewBag.BrandTitle = Brand?.Title;
            return View(PreviewProducts);
        }

        // GET: products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

             var entity = await _productService.Get(id.Value);
            if (entity != null)
            {
                ProductDetailsViewModel model = new()
                {
                    CategoryId = entity.CategoryId,
                    BrandId = entity.BrandId,
                    Id = entity.Id,
                    Title = entity.Title,
                    Price = entity.Price,
                    Description = entity.Description,
                    Quantity = entity.Quantity,
                };

                var img = await _productImageDataService.GetMainByProductId(model.Id);
                var category = await _categoryService.Get(entity.CategoryId);
                var brand = await _brandService.Get(entity.BrandId);

                if (img != null)
                    model.MainImageTitle = "\\images\\products\\main\\" + img.Title;

                if(category != null)
                    model.CategoryTitle = category.Title;
                
                if(brand != null)
                    model.BrandTitle = brand.Title;

                if (model == null)
                    return NotFound();

                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> ListByCategory(int id)
        {
            List<PreviewProductViewModel> products = new();
            var models = await _productService.GetByCategory(id);
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                };
                var img = await _productImageDataService.GetMainByProductId(item.Id);

                if (img != null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + img.Title;

                products.Add(PreviewProduct);
            }
            IEnumerable<PreviewProductViewModel> PreviewProducts = products;
            var category = await _categoryService.Get(id);
            ViewBag.categoryTitle = category?.Title; ;
            return View(PreviewProducts);
        }

        public async Task<IActionResult> Search(string text)
        {
            List<PreviewProductViewModel> products = new();
            var models = await _productService.Search(text);
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price,
                };
                var img = await _productImageDataService.GetMainByProductId(item.Id);

                if (img != null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + img.Title;

                products.Add(PreviewProduct);
            }
            IEnumerable<PreviewProductViewModel> PreviewProducts = products;
            ViewBag.Text = text;
            return View(PreviewProducts);
        }
    }
}