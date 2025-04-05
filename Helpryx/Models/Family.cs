using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Family
    {
        [Key]
        public int FamilyID { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public int Insurance { get; set; } = 0;
        public string Diagnosis { get; set; } = string.Empty;
        public double HoursPerWeek { get; set; } = 0;
        public string ServicesRequest { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
