using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Collections.Generic;
using Framework;
using System.Linq;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        private readonly OnlineShopContext _onlineShopContext;
        private readonly IDataRepository<User> _dataRepository;
        public UserController(IDataRepository<User> dataRepository, OnlineShopContext onlineShopContext)
        {
            _dataRepository = dataRepository;
            _onlineShopContext = onlineShopContext;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        //POST: api/User
        [HttpPost]
        public IActionResult SignIn(string email, string pass)
        {
            if (_onlineShopContext.Users.Where(c => c.Email == email && c.Password == pass && c.IsDelete == false)
               .Any())
            {
                User user = _onlineShopContext.Users.Where(c => c.Email == email && c.Password == pass 
                && c.IsDelete == false).FirstOrDefault();
                ViewBag.userId = user.Id;
                if (user.UserTypeId == (int)Enums.UserType.User)
                    return RedirectToAction("Index", "Orders", new { area = "User" });
                else
                    return RedirectToAction("Index", "Products", new { area = "Admin" });
            }
            ViewBag.Message = "نام کاربری یا رمز عبور نادرست است.";
                return View();
        }

        //POST: api/User
        [HttpPost]
        public IActionResult SignUp(string email, string pass)
        {
            if (email == null || pass == null)
            {
                return BadRequest("User is null.");
            }
            User user=new User();
            user.Password = pass;
            user.Email = email;
            user.UserTypeId = (int)Enums.UserType.User;
            _dataRepository.Add(user);
            return RedirectToAction("SignIn");
        }       
    }
}

