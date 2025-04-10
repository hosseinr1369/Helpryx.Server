﻿using Api.Models;
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
        public DbSet<BackgroundCheckRequest> backgroundCheckRequests { get; set; }
        public DbSet<Api.Models.UsersFavorit> UsersFavorit { get; set; } = default!;
        public DbSet<Api.Models.ApplyFacility> ApplyFacility { get; set; } = default!;
        public DbSet<Family> Families { get; set; }
    }
}
