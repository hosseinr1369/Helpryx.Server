using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace Api.Controllers.V100
{
    [Route("api/V100/[controller]")]
    [ApiController]
    public class OTPEmailController : ControllerBase
    {

        // POST api/V100/<OTPEmailController>
        [HttpPost]
        public async Task PostOTPEmailWithMailKit(OTPEmail OTPemail)
        {
            if (OTPemail != null)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Helpryx", "info@shalizarsoft.ir"));
                    message.To.Add(new MailboxAddress("Helpryx", OTPemail.EmailTo));
                    message.Subject = "Confirm Email Address";
                    message.Body = new TextPart("html")
                    {
                        Text = OTPemail.CodeMailed.ToString()
                    };
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("mail.shalizarsoft.ir", 465, true);
                        await client.AuthenticateAsync("info@shalizarsoft.ir", "sal14569");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                }                
            }
        }

        //public async Task PostOTPEmail(OTPEmail OTPemail)
        //{
        //    if (OTPemail != null) {
        //        var smtpClient = new SmtpClient("mail.shalizarsoft.ir", 465);
        //        smtpClient.EnableSsl = true;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.Credentials = new NetworkCredential("info@shalizarsoft.ir", "sal14569");
        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("info@shalizarsoft.ir", "Helpryx"),
        //            Subject = "Confirm Email Address",
        //            Body = OTPemail.CodeMailed.ToString(),
        //            IsBodyHtml = true // Set to true if sending HTML email
        //        };
        //        mailMessage.To.Add(OTPemail.EmailTo);
        //        await smtpClient.SendMailAsync(mailMessage);
        //    }
        //}        
    }
}
