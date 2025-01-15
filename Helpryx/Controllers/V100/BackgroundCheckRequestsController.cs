using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers.V100
{
    [Route("api/V100/[controller]/[action]")]
    [ApiController]
    public class BackgroundCheckRequestsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public BackgroundCheckRequestsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/BackgroundCheckRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BackgroundCheckRequest>>> GetbackgroundCheckRequests()
        {
          if (_context.backgroundCheckRequests == null)
          {
              return NotFound();
          }
            return await _context.backgroundCheckRequests.ToListAsync();
        }

        // GET: api/BackgroundCheckRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BackgroundCheckRequest>> GetBackgroundCheckRequest(int id)
        {
          if (_context.backgroundCheckRequests == null)
          {
              return NotFound();
          }
            var backgroundCheckRequest = await _context.backgroundCheckRequests.FindAsync(id);

            if (backgroundCheckRequest == null)
            {
                return NotFound();
            }

            return backgroundCheckRequest;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BackgroundCheckRequest>> GetBackgroundCheckRequestByProfileID(int id)
        {
            if (_context.backgroundCheckRequests == null)
            {
                return NotFound();
            }
            var backgroundCheckRequest = await _context.backgroundCheckRequests.Where(a => a.ProfileID == id).FirstOrDefaultAsync();

            if (backgroundCheckRequest == null)
            {
                return NotFound();
            }

            return backgroundCheckRequest;
        }

        // PUT: api/BackgroundCheckRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBackgroundCheckRequest(int id, BackgroundCheckRequest backgroundCheckRequest)
        {
            if (id != backgroundCheckRequest.BackgroundCheckRequestID)
            {
                return BadRequest();
            }

            _context.Entry(backgroundCheckRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BackgroundCheckRequestExists(id))
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

        // POST: api/BackgroundCheckRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BackgroundCheckRequest>> PostBackgroundCheckRequest(BackgroundCheckRequest backgroundCheckRequest)
        {
          if (_context.backgroundCheckRequests == null)
          {
              return Problem("Entity set 'ApiDbContext.backgroundCheckRequests'  is null.");
          }
            _context.backgroundCheckRequests.Add(backgroundCheckRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBackgroundCheckRequest", new { id = backgroundCheckRequest.BackgroundCheckRequestID }, backgroundCheckRequest);
        }

        // DELETE: api/BackgroundCheckRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBackgroundCheckRequest(int id)
        {
            if (_context.backgroundCheckRequests == null)
            {
                return NotFound();
            }
            var backgroundCheckRequest = await _context.backgroundCheckRequests.FindAsync(id);
            if (backgroundCheckRequest == null)
            {
                return NotFound();
            }

            _context.backgroundCheckRequests.Remove(backgroundCheckRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BackgroundCheckRequestExists(int id)
        {
            return (_context.backgroundCheckRequests?.Any(e => e.BackgroundCheckRequestID == id)).GetValueOrDefault();
        }
    }
}
