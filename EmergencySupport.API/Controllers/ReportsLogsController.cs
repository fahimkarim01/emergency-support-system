using EmergencySupport.Data;
using EmergencySupport.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmergencySupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsLogsController : ControllerBase
    {
        public readonly EsupportDbContext _context;
        public ReportsLogsController(EsupportDbContext context)
        {
            _context = context;
        }

        [HttpGet("getReports")]
        public IActionResult GetAll()
        {
            var list = _context.ReportsLogs.ToList();

            return Ok(list);
        }

        [HttpGet("byID/{id:int}")]
        public IActionResult GetByID(int id)
        {
            var reports = _context.ReportsLogs.Find(id);

            if (reports == null)
            {
                return NotFound("Invalid ID!");
            }
            else
            {
                return Ok(reports);
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] ReportsLogs reports)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = _context.ReportsLogs.AsNoTracking().FirstOrDefault(e => e.LogId == reports.LogId);

                if (existing == null)
                {
                    _context.ReportsLogs.Add(reports);
                }
                else
                {
                    _context.ReportsLogs.Update(reports);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(reports);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var reports = _context.ReportsLogs.Find(id);

                if (reports == null)
                {
                    return NotFound("Invalid ID!");
                }

                _context.ReportsLogs.Remove(reports);

                _context.SaveChanges();

                return Ok("Report deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
