using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.V100
{
    [Authorize]
    [Route("api/V100/[controller]")]
    [ApiController]
    public class FacilityImageListsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FacilityImageListsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/FacilityImageLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacilityImageList>>> GetfacilityImageLists()
        {
          if (_context.facilityImageLists == null)
          {
              return NotFound();
          }
            return await _context.facilityImageLists.ToListAsync();
        }

        // GET: api/FacilityImageLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityImageList>> GetFacilityImageList(int id)
        {
          if (_context.facilityImageLists == null)
          {
              return NotFound();
          }
            var facilityImageList = await _context.facilityImageLists.FindAsync(id);

            if (facilityImageList == null)
            {
                return NotFound();
            }

            return facilityImageList;
        }

        // PUT: api/FacilityImageLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacilityImageList(int id, FacilityImageList facilityImageList)
        {
            if (id != facilityImageList.FacilityImageListID)
            {
                return BadRequest();
            }

            _context.Entry(facilityImageList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityImageListExists(id))
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

        // POST: api/FacilityImageLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FacilityImageList>> PostFacilityImageList(FacilityImageList facilityImageList)
        {
          if (_context.facilityImageLists == null)
          {
              return Problem("Entity set 'ApiDbContext.facilityImageLists'  is null.");
          }
            _context.facilityImageLists.Add(facilityImageList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacilityImageList", new { id = facilityImageList.FacilityImageListID }, facilityImageList);
        }

        // DELETE: api/FacilityImageLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacilityImageList(int id)
        {
            if (_context.facilityImageLists == null)
            {
                return NotFound();
            }
            var facilityImageList = await _context.facilityImageLists.FindAsync(id);
            if (facilityImageList == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(facilityImageList.ImageName) && facilityImageList.ImageName != "")
            {
                Uri uri = new Uri(facilityImageList.ImageName);
                string imagename = Path.GetFileName(uri.LocalPath);
                string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilities", imagename);
                if (System.IO.File.Exists(FullPath))
                    System.IO.File.Delete(FullPath);
            }
            _context.facilityImageLists.Remove(facilityImageList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacilityImageListExists(int id)
        {
            return (_context.facilityImageLists?.Any(e => e.FacilityImageListID == id)).GetValueOrDefault();
        }
    }
}
