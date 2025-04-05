using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ApplyFacility
    {
        [Key]
        public int ApplyFacilityID { get; set; }
        public int ProfileID { get; set; } = 0;
        public int FacilityID { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
    }
}
