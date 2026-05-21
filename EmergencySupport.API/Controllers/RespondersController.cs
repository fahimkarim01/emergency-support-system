using Azure;
using EmergencySupport.Data;
using EmergencySupport.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmergencySupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespondersController : ControllerBase
    {
        public readonly EsupportDbContext _context;
        public RespondersController(EsupportDbContext context)
        {
            _context = context;
        }

        [HttpGet("getResponders")]
        public IActionResult GetAll()
        {
            var list = _context.Responders.ToList();

            return Ok(list);
        }

        [HttpGet("byID/{id:int}")]
        public IActionResult GetByID(int id)
        {
            var responder = _context.Responders.Find(id);

            if (responder == null)
            {
                return NotFound("Invalid ID!");
            }
            else
            {
                return Ok(responder);
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody] Responders responder)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = _context.Responders.AsNoTracking().FirstOrDefault(e => e.ResponderId == responder.ResponderId);

                if (existing == null)
                {
                    _context.Responders.Add(responder);
                }
                else
                {
                    _context.Responders.Update(responder);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(responder);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var responder = _context.Responders.Find(id);

                if (responder == null)
                {
                    return NotFound("Invalid ID!");
                }

                _context.Responders.Remove(responder);

                _context.SaveChanges();

                return Ok("Responder deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
