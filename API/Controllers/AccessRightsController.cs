using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class AccessRightsController : ControllerBase
    {
        private readonly PersonsContext _context;

        public AccessRightsController(PersonsContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccessRight>> GetAccessRight(string id)
        {
          if (_context.AccessRight == null)
          {
              return NotFound();
          }
            var AccessRight = await _context.AccessRight.FindAsync(id);

            if (AccessRight == null)
            {
                return NotFound();
            }

            return AccessRight;
        }
        
        [HttpPost]
        public async Task<ActionResult<Person>> CreateCard(AccessRight accessRight)
        {
          if (_context.Person == null)
          {
              return Problem("Entity set 'PeopleContext.Person'  is null.");
          }
            _context.AccessRight.Add(accessRight);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccessRightExists(accessRight.PersonPrimaryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccessRight", new { id = accessRight.PersonPrimaryId }, accessRight);
        }

        private bool AccessRightExists(string id)
        {
            return (_context.AccessRight?.Any(e => e.PersonPrimaryId == id)).GetValueOrDefault();
        }
    }
}
