using DomainModel;
using Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _context;
        private readonly IDataService<UserType> _userTypecontext;

        public UsersController(IUserService context, IDataService<UserType> userTypecontext)
        {
            _context = context;
            _userTypecontext = userTypecontext;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var ShopContext = await _context.GetAll();
            return View(ShopContext);
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Get(id.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: Admin/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserTypeId"] = new SelectList(await _userTypecontext.GetAll(), "Id", "Id");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserTypeId,FullName,Email,Password,MobileNumber,Address,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] DomainModel.User user)
        {
            if (ModelState.IsValid)
            {
                await _context.Add(user);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeId"] = new SelectList(await _userTypecontext.GetAll(), "Id", "Id", user.UserTypeId);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Get(id.Value); 
            if (user == null)
                return NotFound();

            ViewData["UserTypeId"] = new SelectList(await _userTypecontext.GetAll(), "Id", "Id", user.UserTypeId);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserTypeId,FullName,Email,Password,MobileNumber,Address,CreatedAt,CreatedBy,EditedAt,EditedBy,IsDelete")] DomainModel.User user)
        {
            if (id != user.Id)
            return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Any(user.Id))
                    return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeId"] = new SelectList(await _userTypecontext.GetAll(), "Id", "Id", user.UserTypeId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            return NotFound();

            var user = await _context.Get(id.Value);
            if (user == null)
            return NotFound();

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Get(id);
            if (user != null)
                await _context.Delete(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
