using EmergencySupport.Entities;
using EmergencySupport.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmergencySupport.Web.Controllers
{
    [Authorize(Roles = "admin, emergency operator, responder")]
    public class AssignmentsController(AssignmentsRepo repo, EmergencyRequestsRepo requestsRepo, RespondersRepo respondersRepo) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            if (result.HasError)
                ViewBag.Error = result.Message;

            return View(result.Data ?? new List<Assignments>());
        }

        public IActionResult Create(int dataId)
        {
            var resultRequest = requestsRepo.GetAll();
            var resultResponder = respondersRepo.GetAll();

            if (resultRequest.HasError)
            {
                TempData["Error"] = resultRequest.Message;
                return RedirectToAction("Index");
            }

            if (resultResponder.HasError)
            {
                TempData["Error"] = resultResponder.Message;
                return RedirectToAction("Index");
            }

            ViewBag.RequestList = resultRequest.Data.Select(e =>
            new SelectListItem()
            {
                Text = e.EmergencyType,
                Value = e.RequestId.ToString()
            });

            ViewBag.UserList = resultResponder.Data
                .Select(e => new SelectListItem()
                {
                    Text = e.User?.Name,
                    Value = e.ResponderId.ToString()
                });

            if (dataId == -1)
                return View(new Assignments());

            var result = repo.GetById(dataId);

            if (result.HasError)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Create(Assignments model)
        {
            var resultRequest = requestsRepo.GetAll();
            var resultResponder = respondersRepo.GetAll();

            if (resultRequest.HasError)
            {
                TempData["Error"] = resultRequest.Message;
                return RedirectToAction("Index");
            }

            if (resultResponder.HasError)
            {
                TempData["Error"] = resultResponder.Message;
                return RedirectToAction("Index");
            }

            ViewBag.RequestList = resultRequest.Data.Select(e =>
            new SelectListItem()
            {
                Text = e.EmergencyType,
                Value = e.RequestId.ToString()
            });

            ViewBag.UserList = resultResponder.Data
                .Select(e => new SelectListItem()
                {
                    Text = e.User?.Name,
                    Value = e.ResponderId.ToString()
                });

            var result = repo.Save(model);

            if (result.HasError)
            {
                ViewBag.Error = result.Message;
                return View(model);
            }

            TempData["Success"] = "Saved successfully";
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