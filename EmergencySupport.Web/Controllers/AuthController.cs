using EmergencySupport.Entities;
using EmergencySupport.Models;
using EmergencySupport.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmergencySupport.Web.Controllers
{
    public class AuthController(UsersRepo repo) : Controller
    {
        public IActionResult Signup(int dataId)
        {
            if (dataId == 0)
            {
                return View(new Users());
            }

            var result = repo.GetById(dataId);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction("Login");
            }

            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Signup(Users model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = repo.Save(model);

            if (result.HasError)
            {
                ViewBag.Error = result.Message;

                return View(model);
            }

            TempData["Success"] =
                "User registered successfully";

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = repo.Authenticate(
                model.Email,
                model.Password
            );

            if (result.HasError || result.Data == null)
            {
                ViewBag.Error = result.Message;

                return View(model);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,
                    result.Data.Name),

                new Claim(ClaimTypes.Role,
                    result.Data.Role),

                new Claim("UserId",
                    result.Data.UserId.ToString()),

                new Claim("Email",
                    result.Data.Email)
            };

            var identity =
                new ClaimsIdentity(claims, "EsAuth");

            var principal =
                new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                "EsAuth",
                principal
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        public IActionResult Denied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("EsAuth");

            return RedirectToAction("Login");
        }
    }
}