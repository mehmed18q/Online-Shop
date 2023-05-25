using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Shop.Areas.User.Controllers
{
    [Area("User")]
    public class UsersController : Controller
    {
        private readonly IUserService _context;

        public UsersController(IUserService context)
        {
            _context = context;
        }

        // GET: User/Users/Details/5
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();

            var User = await _context.Get(id.Value);

            if (User == null)
                return NotFound();

            return View(User);
        }

        // GET: User/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var User = await _context.Get(id.Value);
            if (User == null)
                return NotFound();
            return View(User);
        }

        // POST: User/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UserStaus,TotalPrice,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] DomainModel.User User)
        {
            if (id != User.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(User);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Any(User.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(User);
        }
    }
}
