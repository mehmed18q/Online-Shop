using DomainModel;
using Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly IDataService<Brand> _dataRepository;

        public BrandsController(IDataService<Brand> dataRepository)=> _dataRepository = dataRepository;

        // GET: Admin/Brands
        public async Task<IActionResult> Index()
        {
            var Brands = await _dataRepository.GetAll();
            return View(Brands);
        }

        // GET: Admin/Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.IsZeroOrNull())
                return NotFound();

            var Brand = await _dataRepository.Get(id ?? 0);

            if (Brand == null)
                return NotFound();

            return View(Brand);
        }

        // GET: Admin/Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Title,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Brand Brand)
        {
            if (ModelState.IsValid)
            {
                await _dataRepository.Add(Brand);
                return RedirectToAction(nameof(Index));
            }

            return View(Brand);
        }

        // GET: Admin/Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.IsNotZeroOrNull())
                return NotFound();

            var Brand = await _dataRepository.Get(id ?? 0);

            if (Brand == null)
                return NotFound();

            return View(Brand);
        }

        // POST: Admin/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Title,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Brand Brand)
        {
            if (id != Brand.Id || id.IsZero() || Brand.Id.IsZero())
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataRepository.Update(Brand);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _dataRepository.Any(Brand.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(Brand);
        }

        // GET: Admin/Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.IsZeroOrNull())
                return NotFound();

            var Brand = await _dataRepository.Get(id ?? 0); ;
            if (Brand == null)
                return NotFound();

            return View(Brand);
        }

        // POST: Admin/Brands/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Brand = await _dataRepository.Get(id);
            if (Brand != null)
                await _dataRepository.Delete(Brand);

            return RedirectToAction(nameof(Index));
        }
    }
}
