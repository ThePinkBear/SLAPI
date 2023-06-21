using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class AccessPointsController : ControllerBase
    {
        private readonly PersonsContext _context;

        public AccessPointsController(PersonsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessPoint>>> GetCards()
        {
          if (_context.Person == null)
          {
              return NotFound();
          }
            return await _context.AccessPoint.ToListAsync();
        }
    }
}
