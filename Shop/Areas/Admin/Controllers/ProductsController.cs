using DomainModel;
using Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;
using ViewModel;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IDataService<Product> _dataRepository;
        private readonly IDataService<Category> _categoryRepository;
        private readonly IProductImageService _productImageRepository;
        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public ProductsController(IDataService<Product> dataRepository, IDataService<Category> categoryRepository
            , IProductImageService productImageRepository
            , Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _dataRepository = dataRepository;
            _categoryRepository = categoryRepository;
            _productImageRepository = productImageRepository;
            _hostingEnvironment = environment;
        }

        // GET: Admin/products
        public async Task<IActionResult> Index()
        {
            List<ProductDetailsViewModel> models = new();

            var entities = await _dataRepository.GetAll();
            foreach (var item in entities)
            {
                var category = await _categoryRepository.Get(item.CategoryId);
                var img = await _productImageRepository.GetMainByProductId(item.Id);
                ProductDetailsViewModel model = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Description = item.Description,
                    CategoryTitle = category?.Title,
                    MainImageTitle = "\\images\\products\\main\\" + img?.Title,
                };
    
                models.Add(model);
            }

            return View(models.OrderByDescending(x => x.Id));
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.IsNotZeroOrNull())
                return NotFound();

            var entity = await _dataRepository.Get(id.Value);
            if (entity != null)
            {
                var img = await _productImageRepository.GetMainByProductId(entity.Id);
                var category = await _categoryRepository.Get(entity.CategoryId);
                ProductDetailsViewModel model = new()
                {
                    CategoryTitle = category?.Title,
                    Id = entity.Id,
                    Price = entity.Price,
                    Quantity = entity.Quantity,
                    Description = entity.Description,
                    Title = entity.Title,
                    MainImageTitle = "\\images\\products\\main\\" + img?.Title
                };
                return View(model);
            };

            return View();
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            var category = await _categoryRepository.GetAll();

            ViewData["Categories"] = new SelectList(category, "Id", "Title");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost, Obsolete]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            Product entity = new()
            {
                Title = model?.Title ?? string.Empty,
                Quantity = model?.Quantity ?? 1,
                Price = model?.Price ?? string.Empty,
                Description = model?.Description ?? string.Empty,
                CategoryId = model?.CategoryId ?? 1,
                CreatedBy = 1,
            };

            await _dataRepository.Add(entity);

            //Save Image in Folder
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products\\main");

            if (model?.MainImage?.Length > 0)
            {
                var filePath = Path.Combine(uploads, model.MainImage.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.MainImage?.CopyToAsync(fileStream);
            }

            //Save Image Name in db
            ProductImage productImages = new()
            {
                ProductId = entity.Id,
                IsMain = true,
                Title = model?.MainImage?.FileName ?? string.Empty,
            };

            await _productImageRepository.Add(productImages);

            //Save Other Images in folder
            var OtherUploads = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products\\other");
            if (model?.OtherImages != null)
                foreach (var file in model.OtherImages)
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(OtherUploads, file.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        model.MainImage?.CopyToAsync(fileStream);
                    }

            //Save other images in db
            if (model?.OtherImages != null)
                foreach (var file in model.OtherImages)
                {
                    ProductImage productOtherImages = new();
                    productOtherImages.ProductId = entity.Id;
                    productOtherImages.IsMain = false;
                    productOtherImages.Title = file.FileName;
                    await _productImageRepository.Add(productOtherImages);
                }

            var category = await _categoryRepository.GetAll();

            ViewData["Categories"] = new SelectList(category, "Id", "Title");
            return View();
        }


        // GET: Admin/Products/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var entity = await _dataRepository.Get(id.Value);
            if (entity != null)
            {
                var img = await _productImageRepository.GetMainByProductId(entity.Id);
                ViewBag.MainImageTitle = "\\images\\products\\main\\" + img?.Title;
                var category = await _categoryRepository.GetAll();
                ViewData["Categories"] = new SelectList(category, "Id", "Title");
            }

            return View(entity);
        }

        // POST: Admin/Products/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Title,Quantity,Price,Description,CategoryId")] Product model)
        {
            await _dataRepository.Update(model);

            ViewBag.Message = "ویرایش با موفقیت انجام شد";
            return RedirectToAction("Index", "Products");
        }

        // GET: Admin/Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var entity = await _dataRepository.Get(id.Value);
            if (entity != null)
            {
                var category = await _categoryRepository.Get(entity.CategoryId);
                var img = await _productImageRepository.Get(entity.Id);
                ProductDetailsViewModel model = new()
                {
                    CategoryTitle = category?.Title,
                    Id = entity.Id,
                    Price = entity.Price,
                    Quantity = entity.Quantity,
                    Description = entity.Description,
                    Title = entity.Title,
                    MainImageTitle = "\\images\\products\\main\\" + img?.Title,
                };
                return View(model);
            }
            return View();
        }

        // POST: Admin/Products/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteById(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var entity = await _dataRepository.Get(id.Value);
            if (entity != null)
            {
                await _dataRepository.Delete(entity);
                ViewBag.Message = "حذف با موفقیت انجام شد";
            }

            ViewBag.Message = "حذف با شکست مواجه شد";
            return RedirectToAction("Index", "Products");
        }
    }
}
