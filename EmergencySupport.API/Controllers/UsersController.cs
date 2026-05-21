using EmergencySupport.Data;
using EmergencySupport.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmergencySupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly EsupportDbContext _context;
        public UsersController(EsupportDbContext context)
        {
            _context = context;
        }

        [HttpGet("getUsers")]
        public IActionResult GetAll()
        {
            var list = _context.Users.ToList();

            return Ok(list);
        }

        [HttpGet("byID/{id:int}")]
        public IActionResult GetByID(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound("Invalid ID!");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost]
        public IActionResult Save([FromBody]Users user)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = _context.Users.AsNoTracking().FirstOrDefault(e => e.UserId == user.UserId);

                if (existing == null)
                {
                    _context.Users.Add(user);
                }
                else
                {
                    _context.Users.Update(user);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _context.Users.Find(id);

                if (user == null)
                {
                    return NotFound("Invalid ID!");
                }

                _context.Users.Remove(user);

                _context.SaveChanges();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
