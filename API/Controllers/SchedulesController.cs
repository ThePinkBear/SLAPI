using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly PersonsContext _context;

        public SchedulesController(PersonsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
          if (_context.Schedule == null)
          {
              return NotFound();
          }
            return await _context.Schedule.ToListAsync();
        }
    }
}
