using EmergencySupport.Entities;
using EmergencySupport.Repos;
using EmergencySupport.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmergencySupport.Web.Controllers
{
    [Authorize]
    public class FeedbackController(FeedbackRepo repo, EmergencyRequestsRepo requestsRepo, UsersRepo usersRepo, CurrentUserHelper currentUserHelper) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            return View(result.Data ?? new List<Feedback>());
        }

        public IActionResult Create(int dataId)
        {
            var resultRequest = requestsRepo.GetAll();

            if (resultRequest.HasError)
            {
                TempData["Error"] = resultRequest.Message;
                return RedirectToAction("Index");
            }

            ViewBag.RequestList = resultRequest.Data.Where(e => e.Status == "Completed").Select(e =>
            new SelectListItem()
            {
                Text = e.EmergencyType,
                Value = e.RequestId.ToString()
            });

            if (dataId == -1)
            {
                var model = new Feedback();

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
        public IActionResult Create(Feedback model)
        {
            var resultRequest = requestsRepo.GetAll();

            if (resultRequest.HasError)
            {
                TempData["Error"] = resultRequest.Message;
                return RedirectToAction("Index");
            }

            ViewBag.RequestList = resultRequest.Data.Where(e => e.Status == "Completed").Select(e =>
            new SelectListItem()
            {
                Text = e.EmergencyType,
                Value = e.RequestId.ToString()
            });

            ModelState.Remove("CreatedAt");
            ModelState.Remove("User");
            ModelState.Remove("Request");

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var result = repo.Save(model);

            if (result.HasError)
            {
                ViewBag.Error = result.Message;
                return View(model);
            }
            else
            {
                if (result.Data != null)
                {
                    TempData["Success"] = $"Data# {result.Data.FeedbackId} saved successfully";
                    return RedirectToAction("Index");
                }
            }

            return View(result.Data);
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