using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/V100/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FacilitiesController(ApiDbContext context)
        {
            _context = context;
        }        

        // GET: api/V100/Facilities/1/10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facility>>> Getfacilities([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (_context.facilities == null)
            {
                return NotFound();
            }
            var totalCount = _context.facilities.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            return await _context.facilities.OrderByDescending(e => e.FacilityID).Skip((page - 1) * totalPages).Take(pageSize).ToListAsync();
        }

        // GET: api/V100/Facilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facility>> GetFacility(int id)
        {
          if (_context.facilities == null)
          {
              return NotFound();
          }
            var facility = await _context.facilities.FindAsync(id);

            if (facility == null)
            {
                return NotFound();
            }

            return facility;
        }

        // PUT: api/V100/Facilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacility(int id, Facility facility)
        {
            if (id != facility.FacilityID)
            {
                return BadRequest();
            }

            _context.Entry(facility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(id))
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

        // POST: api/V100/Facilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Facility>> PostFacility(Facility facility)
        {
          if (_context.facilities == null)
          {
              return Problem("Entity set 'ApiDbContext.facilities'  is null.");
          }
            _context.facilities.Add(facility);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacility", new { id = facility.FacilityID }, facility);
        }

        // DELETE: api/V100/Facilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            if (_context.facilities == null)
            {
                return NotFound();
            }
            var facility = await _context.facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            _context.facilities.Remove(facility);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacilityExists(int id)
        {
            return (_context.facilities?.Any(e => e.FacilityID == id)).GetValueOrDefault();
        }
    }
}
