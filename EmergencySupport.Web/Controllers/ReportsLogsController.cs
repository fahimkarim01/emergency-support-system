using EmergencySupport.Entities;
using EmergencySupport.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmergencySupport.Web.Controllers
{
    [Authorize(Roles = "emergency operator")]
    public class ReportsLogsController(ReportsLogsRepo repo) : Controller
    {
        public IActionResult Index()
        {
            var result = repo.GetAll();

            return View(result.Data ?? new List<ReportsLogs>());
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