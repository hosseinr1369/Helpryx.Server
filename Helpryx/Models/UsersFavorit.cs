using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UsersFavorit
    {
        [Key]
        public int UsersFavoritID { get; set; }
        public int ProfileID { get; set; } = 0;
        public int FacilityID { get; set; } = 0;
    }
}
