using EmergencySupport.Entities;
using EmergencySupport.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmergencySupport.Web.Controllers
{
    [Authorize(Roles = "admin, emergency operator,responder")]
    public class RespondersController(RespondersRepo repo, UsersRepo usersRepo) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            if (result.HasError)
            {
                ViewBag.Error = result.Message;
            }

            return View(result.Data ?? new List<Responders>());
        }

        public IActionResult Create(int dataId)
        {
            var resultUser = usersRepo.GetAll();

            if (resultUser.HasError)
            {
                TempData["Error"] = resultUser.Message;
                return RedirectToAction("Index");
            }

            ViewBag.UserList = resultUser.Data
                .Where(e => e.Role.ToLower() == "responder")
                .Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.UserId.ToString()
                });

            if (dataId == -1)
            {
                return View(new Responders());
            }

            var result = repo.GetById(dataId);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction("Index");
            }

            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Create(Responders model)
        {
            var resultUser = usersRepo.GetAll();

            if (resultUser.HasError)
            {
                TempData["Error"] = resultUser.Message;
                return RedirectToAction("Index");
            }

            ViewBag.UserList = resultUser.Data
                .Where(e => e.Role.ToLower() == "responder")
                .Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.UserId.ToString()
                });

            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = repo.Save(model);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }

            TempData["Success"] = "Responder saved successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int dataId)
        {
            var result = repo.Delete(dataId);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] =
                    "Deleted successfully";
            }

            return RedirectToAction("Index");
        }
    }
}