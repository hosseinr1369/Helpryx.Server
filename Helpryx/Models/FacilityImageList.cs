using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class FacilityImageList
    {
        [Key]
        public int FacilityImageListID { get; set; }
        public int FacilityID { get; set; }
        public string FacilityUniqueID { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageCaption { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ImageListDescription { get; set; } = string.Empty;
        public int IsDelete { get; set; } = 0;
        //public virtual Facility Facility { get; set; } = new Facility();
        public FacilityImageList()
        {

        }
    }
}
