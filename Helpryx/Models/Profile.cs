using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Profile
    {
        [Key]
        public int ProfileID { get; set; }
        public string ProfileManID { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } = DateTime.Now;
        public int Sex { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Coordinate { get; set; } = string.Empty;
        public string SSN { get; set; } = string.Empty;
        public string LicenseDoc { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ProfilePicAddress { get; set; } = string.Empty;
        public string ResumeAddress { get; set; } = string.Empty;
        public string TBTestAddress { get; set; } = string.Empty;
        public int IsLock { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
    }
}
