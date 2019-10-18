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
    [ApiController]
    [Route("/dashboard")]
    public class DashboardController
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IEnumerable<Spot>> GetAsync([FromHeader] Guid userGuid)
        {
            return await _context.Spots
                .Include(spot => spot.User)
                .Where(spot => spot.User.Guid == userGuid)
                .ToListAsync();
        }
    }
}
