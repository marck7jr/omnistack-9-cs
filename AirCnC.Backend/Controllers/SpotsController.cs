using AirCnC.Backend.Data;
using AirCnC.Backend.Helpers;
using AirCnC.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AirCnC.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SpotsController(ApplicationDbContext context) => _context = context ?? throw new ArgumentNullException(nameof(context));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spot>>> IndexAsync([FromQuery] string techs = null)
        {
            var spots = await _context.Spots
                .Include(spot => spot.User)
                .ToListAsync();

            if (techs is null)
            {
                return spots;
            }

            return spots
                .Where(spot => techs.GetWords().Any(tech => spot.Techs.GetWords().Contains(tech)))
                .ToList();
        }

        [HttpGet("{guid:guid}")]
        public async Task<ActionResult<Spot>> GetAsync(Guid guid)
        {
            if ((await _context.Spots.FindAsync(guid)) is Spot spot)
            {
                return spot;
            }

            return NotFound();
        }

        [HttpPut("{guid:guid}")]
        public async Task<ActionResult<Spot>> PutAsync(Guid guid, [FromForm] Spot spot)
        {
            if (guid != spot.Guid)
            {
                return BadRequest();
            }

            _context.Entry(spot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotExists(guid))
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
        public async Task<ActionResult<Spot>> PostAsync([FromHeader] Guid userGuid, [FromForm] Spot spot)
        {
            if (await _context.Users.FindAsync(userGuid) is User user)
            {
                spot.User = user;

                if (Request.Form.Files[0] is IFormFile file && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot", "images", $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    spot.Thumbnail = filePath
                        .Replace("wwwroot", string.Empty)
                        .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }

                _context.Spots.Add(spot);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Get", new { guid = spot.Guid }, spot);
            }

            return BadRequest(new { error = "User didn't find." });
        }

        [HttpDelete("{guid:guid}")]
        public async Task<ActionResult<Spot>> DeleteAsync(Guid guid)
        {
            if (await _context.Spots.FindAsync(guid) is Spot spot)
            {
                _context.Spots.Remove(spot);
                await _context.SaveChangesAsync();

                return spot;
            }

            return NotFound();
        }

        public bool SpotExists(Guid id) => _context.Spots.Any(e => e.Guid == id);
    }
}
