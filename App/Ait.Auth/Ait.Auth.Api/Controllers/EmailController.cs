using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Send(EmailFormModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //return BadRequest("sadsds");

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("xxx@gmail.com"));  // to
            message.From = new MailAddress("xxx@gmail.com");  // sender
            message.Subject = "Your email subject";
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "xxx@gmail.com",  // replace with valid value
                        Password = "XXXX"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.ru";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.Timeout = 3000;
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