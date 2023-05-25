using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Shop.Areas.User.Controllers
{
    [Area("User")]
    public class OrdersController : Controller
    {
        private readonly IDataService<Order> _context;
        private readonly IUserService _usercontext;

        public OrdersController(IDataService<Order> context, IUserService usercontext)
        {
            _context = context;
            _usercontext = usercontext;
        }

        // GET: User/Orders
        public async Task<IActionResult> Index()
        {
            var Orders = await _context.GetAll();
            return View(Orders);
        }

        // GET: User/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Get(id.Value);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: User/Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _usercontext.GetAll(), "Id", "Id");
            return View();
        }

        // POST: User/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OrderStaus,TotalPrice,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Order order)
        {
            if (ModelState.IsValid)
            {
                await _context.Add(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(await _usercontext.GetAll(), "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: User/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Get(id.Value);
            if (order == null)
                return NotFound();
            ViewData["UserId"] = new SelectList(await _usercontext.GetAll(), "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: User/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderStaus,TotalPrice,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Any(order.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(await _usercontext.GetAll(), "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: User/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Get(id.Value);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: User/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Get(id);
            if (order != null)
                await _context.Delete(order);

            return RedirectToAction(nameof(Index));
        }
    }
}
