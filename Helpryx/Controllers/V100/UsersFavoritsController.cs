using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.V100
{
    [Authorize]
    [Route("api/V100/[controller]/[action]")]
    [ApiController]
    public class UsersFavoritsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsersFavoritsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/UsersFavorits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersFavorit>>> GetUsersFavorit()
        {
            return await _context.UsersFavorit.ToListAsync();
        }

        // GET: api/UsersFavorits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersFavorit>> GetUsersFavorit(int id)
        {
            var usersFavorit = await _context.UsersFavorit.FindAsync(id);

            if (usersFavorit == null)
            {
                return NotFound();
            }

            return usersFavorit;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersFavorit>> GetUsersFavoritByFacilityID(int id)
        {
            var usersFavorit = await _context.UsersFavorit.Where(a => a.FacilityID == id).FirstOrDefaultAsync();

            if (usersFavorit == null)
            {
                return NotFound();
            }

            return usersFavorit;
        }

        [HttpGet("{ProfileID}/{FacilityID}")]
        public async Task<ActionResult<UsersFavorit>?> GetUsersFavoritByProfileIDAndFacilityID(int ProfileID, int FacilityID)
        {
            var usersFavorit = await _context.UsersFavorit.Where(a => a.ProfileID == ProfileID && a.FacilityID == FacilityID).FirstOrDefaultAsync();

            if (usersFavorit == null)
            {
                return NotFound();
            }

            return usersFavorit;
        }

        // GET: api/GetUsersFavoritByProfileID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UsersFavorit>>> GetUsersFavoritListByProfileID(int id)
        {
            var usersFavorit = await _context.UsersFavorit.Where(a => a.ProfileID == id).ToListAsync();

            if (usersFavorit == null)
            {
                return NotFound();
            }

            return usersFavorit;
        }

        // GET: api/GetUsersFavoritByProfileID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UsersFavorit>>> GetUsersFavoritListByFacilityID(int id)
        {
            var usersFavorit = await _context.UsersFavorit.Where(a => a.FacilityID == id).ToListAsync();

            if (usersFavorit == null)
            {
                return NotFound();
            }

            return usersFavorit;
        }

        // PUT: api/UsersFavorits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersFavorit(int id, UsersFavorit usersFavorit)
        {
            if (id != usersFavorit.UsersFavoritID)
            {
                return BadRequest();
            }

            _context.Entry(usersFavorit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersFavoritExists(id))
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

        // POST: api/UsersFavorits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersFavorit>> PostUsersFavorit(UsersFavorit usersFavorit)
        {
            _context.UsersFavorit.Add(usersFavorit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersFavorit", new { id = usersFavorit.UsersFavoritID }, usersFavorit);
        }

        // DELETE: api/UsersFavorits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersFavorit(int id)
        {
            var usersFavorit = await _context.UsersFavorit.FindAsync(id);
            if (usersFavorit == null)
            {
                return NotFound();
            }

            _context.UsersFavorit.Remove(usersFavorit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersFavoritExists(int id)
        {
            return _context.UsersFavorit.Any(e => e.UsersFavoritID == id);
        }
    }
}
