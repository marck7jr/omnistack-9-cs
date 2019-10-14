using AirCnC.Backend.Data;
using AirCnC.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCnC.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> IndexAsync()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{guid:guid}")]
        public async Task<ActionResult<User>> GetAsync(Guid guid)
        {
            if (await _context.Users.FindAsync(guid) is User user)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPut("{guid:guid}")]
        public async Task<ActionResult<User>> PutAsync(Guid guid, [FromBody] User user)
        {
            if (guid != user.Guid)
            {
                return BadRequest();
            }

            _context.Add(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(guid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostAsync([FromBody] User user)
        {
            if (!(await _context.Users.ToListAsync()).Any(x => x.Email == user.Email))
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { guid = user.Guid }, user);
            }

            return BadRequest(new { error = "Email in use." });
        }

        [HttpDelete("{guid:guid}")]
        public async Task<ActionResult<User>> DeleteAsync(Guid guid)
        {
            if (await _context.Users.FindAsync(guid) is User user)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        public bool UserExists(Guid id) => _context.Users.Any(e => e.Guid == id);
    }
}