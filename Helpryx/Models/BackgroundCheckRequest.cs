using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class BackgroundCheckRequest
    {
        [Key]
        public int BackgroundCheckRequestID { get; set; }
        public int ProfileID { get; set; } = 0;
        public int IsConfirm { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
    }
}
