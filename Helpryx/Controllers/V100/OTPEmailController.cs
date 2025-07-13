using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.V100
{
    [Authorize]
    [Route("api/V100/[controller]")]
    [ApiController]
    public class OTPEmailController : ControllerBase
    {

        // POST api/V100/<OTPEmailController>
        //[HttpPost]
        //public async Task PostOTPEmailWithMailKit(OTPEmail OTPemail)
        //{
        //    if (OTPemail != null)
        //    {
        //        try
        //        {
        //            var message = new MimeMessage();
        //            message.From.Add(new MailboxAddress("Carezaar", "info@carezaar.com"));
        //            message.To.Add(new MailboxAddress("Recipient", OTPemail.EmailTo));
        //            message.Subject = "Confirm Email Address";
        //            message.Body = new TextPart("html")
        //            {
        //                Text = "Your confirmation code: " + OTPemail.CodeMailed.ToString()
        //            };

        //            using (var client = new SmtpClient())
        //            {
        //                await client.ConnectAsync("accumail3.accuwebhosting.com", 465, SecureSocketOptions.SslOnConnect);
        //                await client.AuthenticateAsync("info@carezaar.com", "56wOm?78r");
        //                await client.SendAsync(message);
        //                await client.DisconnectAsync(true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: {ex.Message}");
        //        }
        //    }
        //}

        // POST api/V100/<OTPEmailController>
        [HttpPost]
        public async Task PostOTPEmailWithMailKit(OTPEmail OTPemail)
        {
            if (OTPemail != null)
            {
                try
                {
                    string htmlTemplate = System.IO.File.ReadAllText("template.html");
                    htmlTemplate = htmlTemplate.Replace("@Date", DateTime.Now.ToString("yyyy-MM-dd"))
                                               .Replace("@Year", DateTime.Now.ToString("yyyy"))
                                               .Replace("@CodeHere", OTPemail.CodeMailed.ToString());

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Carezaar", "info@carezaar.com"));
                    message.To.Add(new MailboxAddress("Recipient", OTPemail.EmailTo));
                    message.Subject = "Carezaar Confirm Email Address";
                    message.Body = new TextPart("html")
                    {
                        Text = htmlTemplate
                    };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("accumail3.accuwebhosting.com", 465, SecureSocketOptions.SslOnConnect);
                        await client.AuthenticateAsync("info@carezaar.com", "56wOm?78r");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
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
