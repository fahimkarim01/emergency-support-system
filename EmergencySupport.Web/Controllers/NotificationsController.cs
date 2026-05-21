using EmergencySupport.Entities;
using EmergencySupport.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySupport.Web.Controllers
{
    [Authorize]
    public class NotificationsController(NotificationsRepo repo, EmergencySupport.Shared.CurrentUserHelper currentUserHelper) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            if (result.HasError)
                ViewBag.Error = result.Message;

            return View(result.Data ?? new List<Notifications>());
        }


        public IActionResult Create(int dataId)
        {
            try
            {

                if (dataId == -1)
                {
                    return View(new Notifications());
                }


                var result = repo.GetById(dataId);


                if (result.HasError || result.Data == null)
                {

                    TempData["Error"] = result.Message ?? "Notification not found.";


                    return RedirectToAction("Index");
                }


                return View(result.Data);
            }
            catch (Exception ex)
            {

                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(Notifications model)
        {
            try
            {

                ModelState.Remove("CreatedAt");
                ModelState.Remove("User");
                ModelState.Remove("UserId");

                model.UserId = currentUserHelper.UserId;


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = repo.Save(model);

                if (result.HasError)
                {

                    ViewBag.Error = result.Message;

                    // Crucial: return the View(model) instead of redirecting so the user doesn't lose their input!
                    return View(model);
                }

                // If successful, show a success message and redirect back to the list
                TempData["Success"] = "Notification saved successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Catch any unexpected controller-level exception
                ViewBag.Error = "An unexpected error occurred: " + ex.Message;
                return View(model);
            }
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