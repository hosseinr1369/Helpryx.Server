using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/V100/[controller]/[action]")]
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
            return await _context.facilities.Where(a => a.IsLock == 0).OrderByDescending(e => e.FacilityID).Skip((page - 1) * totalPages).Take(pageSize).ToListAsync();
        }        

        [HttpGet("{posterid}")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetFacilitiesByPosterID(int posterid)
        {
            if (_context.facilities == null)
            {
                return NotFound();
            }
            return await _context.facilities.Where(a => a.ProfileIDPoster == posterid).ToListAsync();
        }
        
        [HttpGet("{UniqueID}")]
        public async Task<ActionResult<IEnumerable<FacilityImageList>>> GetFacilityImagesList(string UniqueID)
        {
            if (_context.facilityImageLists == null)
            {
                return NotFound();
            }
            return await _context.facilityImageLists.Where(a => a.FacilityUniqueID == UniqueID).ToListAsync();
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
            var facilityImageList = await _context.facilityImageLists.Where(e => e.FacilityUniqueID == facility.UniqueGUID).ToListAsync();
            if (facilityImageList.Count > 0)
            {
                var result = new
                {
                    facility.FacilityID,
                    facility.FacilityManID,
                    facility.ProfileIDPoster,
                    facility.FacilityName,
                    facility.TypeFacility,
                    facility.NPINumber,
                    facility.State,
                    facility.City,
                    facility.Coordinate,
                    facility.PhoneNumber,
                    facility.EmailAddress,
                    facility.Caregiver,
                    facility.IsLock,
                    facility.ProfileImage,
                    facility.Message,
                    facility.Description,
                    FacilityImageList = facility.FacilityImageList.Select(fil => new
                    {
                        fil.FacilityImageListID,
                        fil.FacilityUniqueID,
                        fil.ImageName,
                        fil.ImageCaption,
                        fil.CreationDate,
                        fil.ImageListDescription
                    })
                };
                return Ok(result);
            }
            else
            {
                return Ok(facility);
            }
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetFavoriteFacilitiesByProfileID(int profileId)
        {
            var favoriteFacilityIds = await _context.UsersFavorit
                .Where(uf => uf.ProfileID == profileId)
                .Select(uf => uf.FacilityID)
                .ToListAsync();

            if (favoriteFacilityIds == null || !favoriteFacilityIds.Any())
            {
                return NotFound();
            }

            var favoriteFacilities = await _context.facilities
                .Where(f => favoriteFacilityIds.Contains(f.FacilityID))
                .ToListAsync();

            return favoriteFacilities;
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetApplyFacilitiesByProfileID(int profileId)
        {
            var applyFacilities = await _context.ApplyFacility
                .Where(uf => uf.ProfileID == profileId)
                .Select(uf => uf.FacilityID)
                .ToListAsync();

            if (applyFacilities == null || !applyFacilities.Any())
            {
                return NotFound();
            }

            var favoriteFacilities = await _context.facilities
                .Where(f => applyFacilities.Contains(f.FacilityID))
                .ToListAsync();

            return favoriteFacilities;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacilityProfileImage(int id, [FromForm] string facilityJson, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");
                // Deserialization of profile JSON
                var updatedFacilityProfile = JsonConvert.DeserializeObject<Facility>(facilityJson);
                if (updatedFacilityProfile == null)
                    return BadRequest("Invalid profile data.");

                // Save the file
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var hashedFileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images/facilityprofiles", hashedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/images/facilityprofiles/{hashedFileName}";
                var existingFacilityProfile = await _context.facilities.FindAsync(id);

                if (existingFacilityProfile == null)
                {
                    return NotFound();
                }

                if (existingFacilityProfile.ProfileImage != "")
                {
                    Uri uri = new Uri(existingFacilityProfile.ProfileImage);
                    string imagename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilityprofiles", imagename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                updatedFacilityProfile.ProfileImage = fileUrl;

                _context.Entry(existingFacilityProfile).CurrentValues.SetValues(updatedFacilityProfile);
                await _context.SaveChangesAsync();

                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteFacilityProfileImage(int id)
        {
            try
            {
                var existingFacilityProfile = await _context.facilities.FindAsync(id);
                if (existingFacilityProfile == null)
                {
                    return NotFound();
                }

                if (existingFacilityProfile.ProfileImage != "")
                {
                    Uri uri = new Uri(existingFacilityProfile.ProfileImage);
                    string imagename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilityprofiles", imagename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                existingFacilityProfile.ProfileImage = "";
                _context.Entry(existingFacilityProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacility([FromForm] string facilityData, IFormFile profileImage, List<IFormFile> facilityImageList)
        {
            try
            {
                string guid = Guid.NewGuid().ToString();

                if (string.IsNullOrEmpty(facilityData))
                    return BadRequest("No facility data provided.");

                var facilityProfile = JsonConvert.DeserializeObject<Facility>(facilityData);

                if (facilityProfile == null)
                    return BadRequest("Invalid facility data.");

                facilityProfile.UniqueGUID = guid;

                if (profileImage != null && profileImage.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(profileImage.FileName);
                    var fileExtension = Path.GetExtension(profileImage.FileName);
                    var hashedFileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images/facilityprofiles", hashedFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }

                    var fileUrl = $"{Request.Scheme}://{Request.Host}/images/facilityprofiles/{hashedFileName}";
                    facilityProfile.ProfileImage = fileUrl;
                }
                else
                {
                    facilityProfile.ProfileImage = "";
                }

                if (facilityImageList != null && facilityImageList.Count > 0)
                {
                    facilityProfile.FacilityImageList = new List<FacilityImageList>();
                    foreach (var file in facilityImageList)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var fileNameImgList = Path.GetFileNameWithoutExtension(file.FileName);
                            var fileExtensionImgList = Path.GetExtension(file.FileName);
                            var hashedFileNameImgList = Guid.NewGuid().ToString().Replace("-", "") + fileExtensionImgList;
                            var filePathImgList = Path.Combine(Directory.GetCurrentDirectory(), "images/facilities", hashedFileNameImgList);
                            var filesUrlList = $"{Request.Scheme}://{Request.Host}/images/facilities/{hashedFileNameImgList}";

                            using (var stream = new FileStream(filePathImgList, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var facilityImage = new FacilityImageList
                            {
                                FacilityUniqueID = guid,
                                ImageName = filesUrlList,
                                CreationDate = DateTime.UtcNow,
                                ImageListDescription = ""
                            };

                            facilityProfile.FacilityImageList.Add(facilityImage);
                        }
                    }
                }

                _context.facilities.Add(facilityProfile);

                if (facilityProfile.FacilityImageList != null && facilityProfile.FacilityImageList.Count > 0)
                {
                    foreach (var item in facilityProfile.FacilityImageList)
                        _context.facilityImageLists.Add(item);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFacility", new { id = facilityProfile.FacilityID }, facilityProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFacilityImages([FromForm]int id, List<IFormFile> facilityImageList)
        {
            try
            {
                Facility facility = new Facility();
                List<FacilityImageList> facilityImageLists = new List<FacilityImageList>();
                facilityImageLists.Clear();
                facility = await _context.facilities.FindAsync(id);
                if (facility != null)
                {
                    if (facilityImageList != null && facilityImageList.Count > 0)
                    {
                        foreach (var file in facilityImageList)
                        {
                            if (file != null && file.Length > 0)
                            {
                                var fileNameImgList = Path.GetFileNameWithoutExtension(file.FileName);
                                var fileExtensionImgList = Path.GetExtension(file.FileName);
                                var hashedFileNameImgList = Guid.NewGuid().ToString().Replace("-", "") + fileExtensionImgList;
                                var filePathImgList = Path.Combine(Directory.GetCurrentDirectory(), "images/facilities", hashedFileNameImgList);
                                var filesUrlList = $"{Request.Scheme}://{Request.Host}/images/facilities/{hashedFileNameImgList}";

                                using (var stream = new FileStream(filePathImgList, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                var facilityImage = new FacilityImageList
                                {
                                    FacilityID = facility.FacilityID,
                                    FacilityUniqueID = facility.UniqueGUID,
                                    ImageName = filesUrlList,
                                    ImageCaption = "",
                                    CreationDate = DateTime.UtcNow,
                                    ImageListDescription = ""
                                };
                                facilityImageLists.Add(facilityImage);
                            }
                        }
                    }
                    else
                        return BadRequest();

                    if (facilityImageLists != null && facilityImageLists.Count > 0)
                    {
                        foreach (var item in facilityImageLists)
                            _context.facilityImageLists.Add(item);
                    }

                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetFacility", new { id = facility.FacilityID }, facility);
                }
                else
                    { return NotFound(); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeFacilityLogoImage([FromForm]int id, IFormFile profileImage)
        {
            try
            {
                Facility facility = new Facility();
                facility = await _context.facilities.FindAsync(id);
                if(facility == null)
                    return BadRequest("No facility data provided.");

                if (profileImage != null && profileImage.Length > 0)
                {
                    Uri uri = new Uri(facility.ProfileImage);
                    string imagename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilityprofiles", imagename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);

                    var fileName = Path.GetFileNameWithoutExtension(profileImage.FileName);
                    var fileExtension = Path.GetExtension(profileImage.FileName);
                    var hashedFileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images/facilityprofiles", hashedFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }

                    var fileUrl = $"{Request.Scheme}://{Request.Host}/images/facilityprofiles/{hashedFileName}";
                    facility.ProfileImage = fileUrl;
                }
                else
                {
                    facility.ProfileImage = "";
                }

                _context.Entry(facility).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFacility", new { id = facility.FacilityID }, facility);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // PUT: api/V100/Facilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutFacility(Facility facility)
        {
            if (facility == null || facility.FacilityID <= 0)
                return BadRequest();

            var existingFacility = _context.facilities
                .AsNoTracking()
                .FirstOrDefault(a => a.FacilityID == facility.FacilityID);

            if (existingFacility == null)
                return NotFound();

            facility.ProfileImage = existingFacility.ProfileImage;
            _context.Entry(facility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(facility.FacilityID))
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
            string guid = Guid.NewGuid().ToString();
            if (_context.facilities == null)
            {
                return Problem("Entity set 'ApiDbContext.facilities'  is null.");
            }
            facility.UniqueGUID = guid; // Assuming UniqueGUID is a property of Facility
            _context.facilities.Add(facility);
            if (facility.FacilityImageList.Count > 0)
            {
                foreach (var item in facility.FacilityImageList)
                {
                    item.FacilityUniqueID = facility.UniqueGUID;
                    _context.facilityImageLists.Add(item);
                }
            }
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
            List<FacilityImageList> facilityImageLists = new List<FacilityImageList>();
            facilityImageLists = _context.facilityImageLists.Where(a => a.FacilityUniqueID ==  facility.UniqueGUID).ToList();
            if(facilityImageLists != null && facilityImageLists.Count > 0)
                foreach (FacilityImageList item in facilityImageLists)
                {
                    if (item.ImageName != "" && !item.ImageName.IsNullOrEmpty())
                    {
                        Uri uri = new Uri(item.ImageName);
                        string imagename = Path.GetFileName(uri.LocalPath);
                        string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilities", imagename);
                        if (System.IO.File.Exists(FullPath))
                            System.IO.File.Delete(FullPath);
                    }
                }
            if (facility.ProfileImage != "" && !facility.ProfileImage.IsNullOrEmpty())
            {
                Uri uri = new Uri(facility.ProfileImage);
                string imagename = Path.GetFileName(uri.LocalPath);
                string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\facilityprofiles", imagename);
                if (System.IO.File.Exists(FullPath))
                    System.IO.File.Delete(FullPath);
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
