using AirCnC.Backend.Data;
using AirCnC.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCnC.Backend.Controllers
{
    [Route("/api/spots/{spotGuid:guid}/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "Index")]
        public async Task<ActionResult<IEnumerable<Booking>>> IndexAsync([FromRoute] Guid spotGuid, [FromHeader] Guid userGuid)
        {
            if (await _context.Spots.FindAsync(spotGuid) is Spot spot)
            {
                return await _context.Bookings
                    .Where(booking => booking.Spot.Guid == spotGuid)
                    .Include(booking => booking.User)
                    .ToListAsync();
            }

            return NotFound();
        }

        [HttpGet("{guid:guid}")]
        public async Task<ActionResult<Booking>> GetAsync([FromRoute] Guid spotGuid, Guid guid)
        {
            if (await _context.Bookings
                .Include(booking => booking.Spot)
                .Include(booking => booking.User)
                .FirstOrDefaultAsync(booking => booking.Guid == guid && booking.Spot.Guid == spotGuid) is Booking booking)
            {
                return booking;
            }

            return NotFound();
        }

        [HttpPut("{guid:guid}")]
        public async Task<IActionResult> PutAsync(Guid guid, Booking booking)
        {
            if (guid != booking.Guid)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(guid))
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
        public async Task<ActionResult<Booking>> PostAsync([FromBody] Booking booking, [FromHeader] Guid userGuid, [FromRoute] Guid spotGuid)
        {
            if ((await _context.Users.FindAsync(userGuid)) is User user && (await _context.Spots.FindAsync(spotGuid)) is Spot spot)
            {
                booking.User = user;
                booking.Spot = spot;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { spotGuid, guid = user.Guid }, booking);
            }

            return NotFound();
        }

        [HttpDelete("{guid:guid}")]
        public async Task<ActionResult<Booking>> DeleteAsync(Guid guid)
        {
            var booking = await _context.Bookings.FindAsync(guid);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        private bool BookingExists(Guid guid) => _context.Bookings.Any(e => e.Guid == guid);
    }
}
