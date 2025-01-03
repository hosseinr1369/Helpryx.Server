using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels
{
    public class FacilityDetails
    {
        [Key]
        public int FacilityID { get; set; }
        public string UniqueGUID { get; set; } = Guid.NewGuid().ToString();
        public string FacilityManID { get; set; } = string.Empty;
        public string FacilityName { get; set; } = string.Empty;
        public string TypeFacility { get; set; } = string.Empty;
        public string NPINumber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Coordinate { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Caregiver { get; set; } = string.Empty;
        public int IsLock { get; set; } = 0;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ProfileImage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<FacilityImageList> ImageLists { get; set; }
        public FacilityDetails()
        {
            
        }
    }
}
