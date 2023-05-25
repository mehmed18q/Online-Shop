using DomainModel;
using Framework;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userRepository;
        public UserController(IUserService userRepository) => _userRepository = userRepository;

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email, password).Result;
            if (user != null)
            {
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
        public IActionResult SignUp(string email, string password)
        {
            if (email.IsEmpty() || password.IsEmpty())
                return BadRequest("User Name or Email is Null!");

            User user = new()
            {
                Password = password,
                Email = email,
                UserTypeId = (int)Enums.UserType.User,
            };

            _userRepository.Add(user);
            return RedirectToAction("SignIn");
        }
    }
}