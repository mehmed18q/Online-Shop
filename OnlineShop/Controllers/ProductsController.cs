using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DomainModel;
using ViewModel;
using Repository;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private int ProductCount = 6;
        private readonly IDataRepository<Product> _dataRepository;
        private readonly IDataRepository<Category> _categoryDdataRepository;
        private readonly ProductRepository _productRepository;
        private readonly ProductImagesRepository _productImageDataRepository;

        public ProductsController(
              IDataRepository<Product> dataRepository
            , IDataRepository<Category> categoryDdataRepository
            , ProductRepository productRepository
            , ProductImagesRepository productImageDataRepository)
        {
            _dataRepository = dataRepository;
            _categoryDdataRepository = categoryDdataRepository;
            _productRepository = productRepository;
            _productImageDataRepository = productImageDataRepository;

        }

        [HttpGet]
        public IActionResult PreviewProducts(int CategoryId)
        {
            List<PreviewProductViewModel> products = new List<PreviewProductViewModel>();
            List<Product> models = _productRepository.GetByCategory(CategoryId, ProductCount).ToList();
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new PreviewProductViewModel();
                PreviewProduct.Id = item.Id;
                PreviewProduct.Title = item.Title;
                PreviewProduct.Price = item.Price;
                if(_productImageDataRepository.GetByProductId(item.Id)!=null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + _productImageDataRepository.GetByProductId(item.Id).Title;

                products.Add(PreviewProduct);
            }
            IEnumerable<PreviewProductViewModel> PreviewProducts = products;
            return Ok(PreviewProducts);
        }

        // GET: products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product entity = _dataRepository.Get((long)id);
            ProductDetailsViewModel model = new ProductDetailsViewModel();
            model.CategoryId = entity.CategoryId;
            model.Id = entity.Id;
            model.Title = entity.Title;
            model.Price = entity.Price;
            model.Description = entity.Description;
            model.MainImageTitle = "\\images\\products\\main\\" + _productImageDataRepository.GetByProductId(model.Id).Title;
            model.CategoryTitle = _categoryDdataRepository.Get(model.CategoryId).Title;
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult List(int CategoryId)
        {
            List<PreviewProductViewModel> products = new List<PreviewProductViewModel>();
            List<Product> models = _productRepository.GetByCategory(CategoryId).ToList();
            foreach (Product item in models)
            {
                PreviewProductViewModel PreviewProduct = new PreviewProductViewModel();
                PreviewProduct.Id = item.Id;
                PreviewProduct.Title = item.Title;
                PreviewProduct.Price = item.Price;
                if (_productImageDataRepository.GetByProductId(item.Id) != null)
                    PreviewProduct.MainImageTitle = "\\images\\products\\main\\" + _productImageDataRepository.GetByProductId(item.Id).Title;

                products.Add(PreviewProduct);
            }
            IEnumerable<PreviewProductViewModel> PreviewProducts = products;
            ViewBag.categoryTitle = _categoryDdataRepository.Get(CategoryId).Title; ;
            return View(PreviewProducts);
        }
    }
}
