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
    public class ApplyFacilitiesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ApplyFacilitiesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplyFacilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplyFacility>>> GetApplyFacility()
        {
            return await _context.ApplyFacility.ToListAsync();
        }

        // GET: api/ApplyFacilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplyFacility>> GetApplyFacility(int id)
        {
            var applyFacility = await _context.ApplyFacility.FindAsync(id);

            if (applyFacility == null)
            {
                return NotFound();
            }

            return applyFacility;
        }        

        [HttpGet("{ProfileID}/{FacilityID}")]
        public async Task<ActionResult<ApplyFacility>?> GetApplyFacilityByProfileIDAndFacilityID(int ProfileID, int FacilityID)
        {
            var applyFacility = await _context.ApplyFacility.Where(a => a.ProfileID == ProfileID && a.FacilityID == FacilityID).FirstOrDefaultAsync();

            if (applyFacility == null)
            {
                return NotFound();
            }

            return applyFacility;
        }

        // GET: api/GetUsersFavoritByProfileID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ApplyFacility>>> GetApplyFacilityByProfileID(int id)
        {
            var applyFacility = await _context.ApplyFacility.Where(a => a.ProfileID == id).ToListAsync();

            if (applyFacility == null)
            {
                return NotFound();
            }

            return applyFacility;
        }

        // GET: api/GetUsersFavoritByProfileID/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ApplyFacility>>> GetApplyFacilityListByFacilityID(int id)
        {
            var applyFacility = await _context.ApplyFacility.Where(a => a.FacilityID == id).ToListAsync();

            if (applyFacility == null)
            {
                return NotFound();
            }

            return applyFacility;
        }        

        // PUT: api/ApplyFacilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplyFacility(int id, ApplyFacility applyFacility)
        {
            if (id != applyFacility.ApplyFacilityID)
            {
                return BadRequest();
            }

            _context.Entry(applyFacility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplyFacilityExists(id))
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

        // POST: api/ApplyFacilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplyFacility>> PostApplyFacility(ApplyFacility applyFacility)
        {
            _context.ApplyFacility.Add(applyFacility);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplyFacility", new { id = applyFacility.ApplyFacilityID }, applyFacility);
        }

        // DELETE: api/ApplyFacilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplyFacility(int id)
        {
            var applyFacility = await _context.ApplyFacility.FindAsync(id);
            if (applyFacility == null)
            {
                return NotFound();
            }

            _context.ApplyFacility.Remove(applyFacility);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplyFacilityExists(int id)
        {
            return _context.ApplyFacility.Any(e => e.ApplyFacilityID == id);
        }
    }
}
