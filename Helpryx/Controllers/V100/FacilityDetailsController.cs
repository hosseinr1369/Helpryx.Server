using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.ViewModels;

namespace Api.Controllers.V100
{
    [Route("api/V100/[controller]")]
    [ApiController]
    public class FacilityDetailsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public FacilityDetailsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/V100/FacilityDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityDetails>> GetFacilityDetails(int id)
        {
          if (_context.facilities == null || _context.facilityImageLists == null)
          {
              return NotFound();
          }
            var facilityDetails = await _context.facilities.Select(p => new FacilityDetails()
            {
                FacilityID = p.FacilityID,
                UniqueGUID = p.UniqueGUID,
                FacilityManID = p.FacilityManID,
                FacilityName = p.FacilityName,
                TypeFacility = p.TypeFacility,
                NPINumber = p.NPINumber,
                State = p.State,
                City = p.City,
                Coordinate = p.Coordinate,
                PhoneNumber = p.PhoneNumber,
                EmailAddress = p.EmailAddress,
                Caregiver = p.Caregiver,
                IsLock = p.IsLock,
                CreationDate = p.CreationDate,
                ProfileImage = p.ProfileImage,
                Message = p.Message,
                Description = p.Description,
                ImageLists = p.FacilityImageList
            }).Where(q => q.FacilityID == id).FirstOrDefaultAsync();

            if (facilityDetails == null)
            {
                return NotFound();
            }
            return facilityDetails;
        }
    }
}
