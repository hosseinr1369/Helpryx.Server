using Api.Models;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<Profile> profiles { get; set; }
        public DbSet<Facility> facilities { get; set; }
        public DbSet<FacilityImageList> facilityImageLists { get; set; }
    }
}
