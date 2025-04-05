using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Facility
    {
        [Key]
        public int FacilityID { get; set; }
        public string UniqueGUID { get; set; } = string.Empty;
        public string FacilityManID { get; set; } = string.Empty;
        public int ProfileIDPoster { get; set; } = 0;
        public string FacilityName { get; set; } = string.Empty;
        public string TypeFacility { get; set; } = string.Empty;
        public string NPINumber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Coordinate { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Caregiver { get; set; } = string.Empty;
        public int IsLock { get; set; } = 1;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ProfileImage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int IsDelete { get; set; } = 0;
        public virtual List<FacilityImageList> FacilityImageList { get; set; } = new List<FacilityImageList>();        
    }
}
