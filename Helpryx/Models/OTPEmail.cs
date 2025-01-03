namespace Api.Models
{
    public class OTPEmail
    {
        public string EmailTo { get; set; } = string.Empty;
        public string CodeMailed { get; set; } = "0000";
    }
}
