using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DomainModel;
using Repository;
using Framework;
using ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using static System.Net.WebRequestMethods;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IDataRepository<Product> _dataRepository;
        private readonly IDataRepository<ProductImages> _productImageDataRepository;
        private IHostingEnvironment _hostingEnvironment;

        private readonly OnlineShopContext _context;

        public ProductsController(OnlineShopContext context
            , IDataRepository<Product> dataRepository
            , IDataRepository<ProductImages> productImageDataRepository
            , IHostingEnvironment environment)
        {
            _context = context;
            _dataRepository = dataRepository;
            _productImageDataRepository = productImageDataRepository;
            _hostingEnvironment = environment;

        }

        // GET: Admin/products
        public IActionResult Index()
        {
            List<ProductDetailsViewModel> models = new List<ProductDetailsViewModel>();

            List<Product> entities = _context.Products.OrderByDescending(c => c.Id).Where(c => c.IsDelete == false).ToList();
            foreach (var item in entities)
            {
                ProductDetailsViewModel model = new ProductDetailsViewModel();
                model.Id = item.Id;
                model.Title = item.Title;
                model.Quantity = item.Quantity;
                model.Price = item.Price;
                model.Description = item.Description;
                model.CategoryTitle = _context.Categories.Where(c => c.Id == item.CategoryId && c.IsDelete == false)
                    .Select(c=>c.Title).FirstOrDefault();
                model.MainImageTitle = "\\images\\products\\main\\" + _context.GetProductImages.Where(c => c.ProductId == item.Id && c.IsMain).Select(c => c.Title)
                    .FirstOrDefault();
                models.Add(model);
            }

            return View(models);
        }

        //// GET: Admin/products
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<Product> data= _context.Products.OrderByDescending(c => c.Id);
            DataSourceResult result = data.ToDataSourceResult(request,
                model => new ProductDetailsViewModel
                {
                    Id = model.Id,
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    Quantity = model.Quantity,
                    Price=model.Price,
                    Description=model.Description,
                });
            return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        //public ActionResult LoadProductsForGrid([DataSourceRequest] DataSourceRequest request)
        //{
        //    IQueryable<Product> logins = _context.Products.OrderByDescending(c => c.Id);
        //
        //    DataSourceResult result = logins.ToDataSourceResult(request, c => new Product
        //    {
        //        Id = c.Id,
        //        Title = c.Title,
        //        Description = c.Description
        //    });
        //
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        // GET: Admin/Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            Product entity = _dataRepository.Get((long)id);
            ProductDetailsViewModel model = new ProductDetailsViewModel();
            model.CategoryTitle = _context.Categories.Where(c => c.Id == entity.CategoryId).Select(c => c.Title)
                .FirstOrDefault();
            model.Id = entity.Id;
            model.Price = entity.Price;
            model.Quantity = entity.Quantity;
            model.Description = entity.Description;
            model.Title = entity.Title;
            model.MainImageTitle = "\\images\\products\\main\\" + _context.GetProductImages.Where(c => c.ProductId == entity.Id && c.IsMain && c.IsDelete == false)
                .Select(c => c.Title).FirstOrDefault();
            return View(model);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            Product entity = new Product();
            entity.Title = model.Title;
            entity.Quantity =model.Quantity;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.CategoryId = model.CategoryId;
            //entity.CreateBy = model.UserId;            
            entity.CreateBy = 1;            
            _dataRepository.Add(entity);

            //Save Image in Folder
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products\\main");
            
            if (model.MainImage.Length > 0)
            {
                var filePath = Path.Combine(uploads, model.MainImage.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                model.MainImage.CopyToAsync(fileStream);
                }
            }

            //Save Image Name in db
            ProductImages productImages = new ProductImages();
            productImages.ProductId = entity.Id;
            productImages.IsMain = true;
            productImages.Title = model.MainImage.FileName;
            _productImageDataRepository.Add(productImages);

            //Save Other Images in folder
            //var OtherUploads = Path.Combine(_hostingEnvironment.WebRootPath, "images\\products\\other");
            //
            //foreach (var file in model.OtherImages)
            //{
            //    if (file.Length > 0)
            //    {
            //        var filePath = Path.Combine(OtherUploads, file.FileName);
            //        using (var fileStream = new FileStream(filePath, FileMode.Create))
            //        {
            //            model.MainImage.CopyToAsync(fileStream);
            //        }
            //    }
            //}  
            ////Save other images in db
            //foreach (var file in model.OtherImages)
            //{
            //    ProductImages productOtherImages = new ProductImages();
            //    productOtherImages.ProductId = entity.Id;
            //    productOtherImages.IsMain = false;
            //    productOtherImages.Title = file.FileName;
            //    _productImageDataRepository.Add(productOtherImages);
            //}
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }


        // GET: Admin/Products/Edit
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            Product entity = _dataRepository.Get((long)id);
            ViewBag.MainImageTitle = "\\images\\products\\main\\" + _context.GetProductImages.Where(c => c.ProductId == entity.Id && c.IsMain && c.IsDelete == false)
                .Select(c => c.Title).FirstOrDefault();

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Title");


            return View(entity);
        }

        // POST: Admin/Products/Edit
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Title,Quantity,Price,Description,CategoryId")] Product model)
        {
            Product entity = _dataRepository.Get((long)id);
            _dataRepository.Update(entity, model);
                       

            ViewBag.Message = "ویرایش با موفقیت انجام شد";
            return RedirectToAction("Index", "Products");
        }




        // GET: Admin/Products/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            Product entity = _dataRepository.Get((long)id);
            ProductDetailsViewModel model = new ProductDetailsViewModel();
            model.CategoryTitle = _context.Categories.Where(c => c.Id == entity.CategoryId).Select(c => c.Title)
                .FirstOrDefault();
            model.Id = entity.Id;
            model.Price = entity.Price;
            model.Quantity = entity.Quantity;
            model.Description = entity.Description;
            model.Title = entity.Title;
            model.MainImageTitle = "\\images\\products\\main\\" + _context.GetProductImages.Where(c => c.ProductId == entity.Id && c.IsMain && c.IsDelete == false)
                .Select(c => c.Title).FirstOrDefault();
            return View(model);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost]
        public IActionResult DeleteById(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();
            Product entity = _dataRepository.Get((long)id);
            _dataRepository.Delete(entity);
            ViewBag.Message = "حذف با موفقیت انجام شد";
            return RedirectToAction("Index", "Products");
        }
    }
}
