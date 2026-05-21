using EmergencySupport.Entities;
using EmergencySupport.Repos;
using EmergencySupport.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySupport.Web.Controllers
{
    [Authorize]
    public class EmergencyRequestsController(
        EmergencyRequestsRepo repo,
        CurrentUserHelper currentUserHelper) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            if (result.HasError)
            {
                ViewBag.Error = result.Message;
            }

            return View(result.Data ??
                        new List<EmergencyRequests>());
        }

        public IActionResult Create(int dataId)
        {
            if (dataId == -1)
            {
                var model = new EmergencyRequests();

                model.UserId = currentUserHelper.UserId;

                return View(model);
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
        public IActionResult Create(EmergencyRequests model)
        {
            ModelState.Remove("User");

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
                "Emergency request saved successfully";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "emergency operator,responder")]
        public IActionResult UpdateStatus(int id, string status)
        {
            var result = repo.UpdateStatus(id, status);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] =
                    "Status updated successfully";
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "emergency operator")]
        public IActionResult SetPriority(int id, string priority)
        {
            var result = repo.SetPriority(id, priority);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] =
                    "Priority updated successfully";
            }

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