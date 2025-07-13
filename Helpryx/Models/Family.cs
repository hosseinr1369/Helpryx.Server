using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Family
    {
        [Key]
        public int FamilyID { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string BehalfName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string Insurance { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public double HoursPerWeek { get; set; } = 0;
        public string ServicesRequest { get; set; } = string.Empty;        
        public string County { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
