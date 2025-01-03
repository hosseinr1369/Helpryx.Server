using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class FacilityImageList
    {
        [Key]
        public int FacilityImageListID { get; set; }
        public int FacilityID { get; set; } = 0;
        public string ImageName { get; set; } = string.Empty;
        public string ImageCaption { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ImageListDescription { get; set; } = string.Empty;
        public virtual Facility Facility { get; set; }
        public FacilityImageList()
        {
            
        }
    }
}
