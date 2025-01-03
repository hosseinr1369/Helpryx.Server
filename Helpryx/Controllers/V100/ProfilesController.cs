using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace Api.Controllers
{
    [Route("api/V100/[controller]/[action]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProfilesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/V100/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> Getprofiles()
        {
          if (_context.profiles == null)
          {
              return NotFound();
          }
            return await _context.profiles.ToListAsync();
        }

        // GET: api/V100/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(int id)
        {
          if (_context.profiles == null)
          {
              return NotFound();
          }
            var profile = await _context.profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // GET: api/V100/Profiles/Email
        //[Route("GetByEmail")]
        [HttpGet("{Email}")]
        public async Task<ActionResult<int>> GetProfileByEmail(string Email)
        {
            if (_context.profiles == null)
            {
                return NotFound();
            }
            int profile = await _context.profiles.Where(a => a.EmailAddress == Email).CountAsync();

            return profile;
        }

        // GET: api/V100/Profiles/user/pass
        //[Route("Loging")]
        [HttpGet("{user}/{pass}")]
        public async Task<ActionResult<Profile>?> GetProfileByUserPass(string user, string pass)
        {
            if (_context.profiles == null)
            {
                return null;
            }
            var profile = await _context.profiles.Where(a => a.EmailAddress == user && a.PasswordHash == HashGenerator.SHA1(pass)).FirstOrDefaultAsync();

            if (profile == null)
            {
                return null;
            }

            return profile;
        }

        // PUT: api/V100/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, Profile profile)
        {
            if (id != profile.ProfileID)
            {
                return BadRequest();
            }
            profile.PasswordHash = HashGenerator.SHA1(profile.PasswordHash);
            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddressUpdate(int id, Profile profile)
        {
            if (id != profile.ProfileID)
            {
                return BadRequest();
            }
            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileImage(int id, [FromForm] string profileJson, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");
                // Deserialization of profile JSON
                var updatedProfile = JsonConvert.DeserializeObject<Profile>(profileJson);
                if (updatedProfile == null)
                    return BadRequest("Invalid profile data.");
                
                // Save the file
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var hashedFileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images/profiles", hashedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/images/profiles/{hashedFileName}";
                var existingProfile = await _context.profiles.FindAsync(id);
                
                if (existingProfile == null)
                {
                    return NotFound();
                }

                if (existingProfile.ProfilePicAddress != "")
                {
                    Uri uri = new Uri(existingProfile.ProfilePicAddress);
                    string imagename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\profiles", imagename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                updatedProfile.ProfilePicAddress = fileUrl;

                _context.Entry(existingProfile).CurrentValues.SetValues(updatedProfile);
                await _context.SaveChangesAsync();

                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteProfileImage(int id)
        {
            try
            {                
                var existingProfile = await _context.profiles.FindAsync(id);
                if (existingProfile == null)
                {
                    return NotFound();
                }

                if (existingProfile.ProfilePicAddress != "")
                {
                    Uri uri = new Uri(existingProfile.ProfilePicAddress);
                    string imagename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "images\\profiles", imagename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                existingProfile.ProfilePicAddress = "";
                _context.Entry(existingProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfileResume(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Save the file
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var hashedFileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "files/resumes", hashedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"{Request.Scheme}://{Request.Host}/files/resumes/{hashedFileName}";
                var existingProfile = await _context.profiles.FindAsync(id);

                if (existingProfile == null)
                {
                    return NotFound();
                }

                if (existingProfile.ResumeAddress != "")
                {
                    Uri uri = new Uri(existingProfile.ResumeAddress);
                    string filename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "files\\resumes", filename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                existingProfile.ResumeAddress = fileUrl;

                _context.Entry(existingProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteResume(int id)
        {
            try
            {
                var existingResume = await _context.profiles.FindAsync(id);
                if (existingResume == null)
                {
                    return NotFound();
                }

                if (existingResume.ResumeAddress != "")
                {
                    Uri uri = new Uri(existingResume.ResumeAddress);
                    string filename = Path.GetFileName(uri.LocalPath);
                    string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "files\\resumes", filename);
                    if (System.IO.File.Exists(FullPath))
                        System.IO.File.Delete(FullPath);
                }
                existingResume.ResumeAddress = "";
                _context.Entry(existingResume).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/V100/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
          if (_context.profiles == null)
          {
              return Problem("Entity set 'ApiDbContext.profiles'  is null.");
          }
            profile.PasswordHash = HashGenerator.SHA1(profile.PasswordHash);
            _context.profiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.ProfileID }, profile);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostUpdatePassword(int id, [FromBody] string newpassword)
        {
            if (_context.profiles == null)
            {
                return Problem("Entity set 'ApiDbContext.profiles'  is null.");
            }
            var existingProfile = await _context.profiles.FindAsync(id);            
            if (existingProfile == null)
            {
                return NotFound();
            }            
            existingProfile.PasswordHash = HashGenerator.SHA1(newpassword);
            _context.Entry(existingProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/V100/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            if (_context.profiles == null)
            {
                return NotFound();
            }
            var profile = await _context.profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<bool> IsUploadedResume(int id)
        {
            bool isUploaded = false;
            isUploaded = await _context.profiles.Where(a => a.ProfileID == id && a.ResumeAddress != "").CountAsync() > 0;
            return isUploaded;
        }

        private bool ProfileExists(int id)
        {
            return (_context.profiles?.Any(e => e.ProfileID == id)).GetValueOrDefault();
        }               
    }
}
