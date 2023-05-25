using DomainModel;
using Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IDataService<Category> _dataRepository;

        public CategoriesController(IDataService<Category> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _dataRepository.GetAll();
            return View(categories);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.IsZeroOrNull())
                return NotFound();

            var category = await _dataRepository.Get(id ?? 0);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Title,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _dataRepository.Add(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.IsNotZeroOrNull())
                return NotFound();

            var category = await _dataRepository.Get(id ?? 0);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Title,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Category category)
        {
            if (id != category.Id || id.IsZero() || category.Id.IsZero())
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataRepository.Update(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _dataRepository.Any(category.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.IsZeroOrNull())
                return NotFound();

            var category = await _dataRepository.Get(id ?? 0); ;
            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _dataRepository.Get(id);
            if (category != null)
                await _dataRepository.Delete(category);

            return RedirectToAction(nameof(Index));
        }
    }
}
