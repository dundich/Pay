using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Web.Http;

namespace Ait.Auth.Api.Controllers
{
    [RoutePrefix("api/Email")]
    public class EmailController : ApiController
    { 
        public class EmailFormModel
        {
            [Display(Name = "От кого")]
            public string FromName { get; set; }
            [Display(Name = "email"), EmailAddress]
            public string FromEmail { get; set; }
            [Required]
            public string Message { get; set; }
        }


        [HttpPost]
        [Authorize]
        [Route("Send")]
        //[ValidateAntiForgeryToken]
        public IHttpActionResult Send(EmailFormModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //return BadRequest("sadsds");

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("xxxxxx@mail.ru"));  // to
            message.From = new MailAddress("xxxxxx@gmail.com");  // sender
            message.Subject = "Your email subject";
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;

            var credential = new NetworkCredential
            {
                UserName = "******@gmail.com",
                Password = "*******"
            };
            try
            {
                using (var smtp = new SmtpClient()
                {
                    Credentials = credential,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    //UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                })
                {

                    smtp.Send(message);

                    //await smtp.SendMailAsync(message);
                }
            }
            catch (Exception e)
            {
                throw e;
                //return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}